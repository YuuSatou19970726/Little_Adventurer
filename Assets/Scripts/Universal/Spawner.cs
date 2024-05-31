using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private List<SpawnPoint> spawnPoints;
    private bool hasSpawned;

    [SerializeField]
    private Collider _collider;

    void Awake()
    {
        var spawnPointArray = transform.parent.GetComponentsInChildren<SpawnPoint>();
        spawnPoints = new List<SpawnPoint>(spawnPointArray);
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
