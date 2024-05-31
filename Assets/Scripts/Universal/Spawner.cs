using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    private List<SpawnPoint> spawnPoints;
    private List<Character> spawnerCharacters;
    private bool hasSpawned;

    [SerializeField]
    private Collider _collider;

    public UnityEvent OnAllSpawnedCharacterEliminated;

    void Awake()
    {
        var spawnPointArray = transform.parent.GetComponentsInChildren<SpawnPoint>();
        spawnPoints = new List<SpawnPoint>(spawnPointArray);
        spawnerCharacters = new List<Character>();
    }

    void Update()
    {
        if (!hasSpawned || spawnerCharacters.Count == 0)
            return;

        bool allSpawnedAreDead = true;

        foreach (Character character in spawnerCharacters)
        {
            if (character.currentState != Character.CharacterState.DEAD)
            {
                allSpawnedAreDead = false;
                break;
            }
        }

        if (allSpawnedAreDead)
        {
            if (OnAllSpawnedCharacterEliminated != null)
                OnAllSpawnedCharacterEliminated.Invoke();

            spawnerCharacters.Clear();
        }
    }

    public void SpawnCharacters()
    {
        if (hasSpawned)
            return;

        hasSpawned = true;

        foreach (SpawnPoint point in spawnPoints)
        {
            if (point.EnemyToSpawn != null)
            {
                GameObject spawnedGameObject = Instantiate(point.EnemyToSpawn, point.transform.position, Quaternion.identity);
                spawnerCharacters.Add(spawnedGameObject.GetComponent<Character>());
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.PLAYER))
            SpawnCharacters();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, _collider.bounds.size);
    }
}
