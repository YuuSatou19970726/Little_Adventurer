using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCaster : MonoBehaviour
{
    private Collider _damageCasterCollider;
    [SerializeField]
    private int Damage = 30;
    [SerializeField]
    private string TargetTag;
    private List<Collider> _damagedTargetList;

    void Awake()
    {
        _damageCasterCollider = GetComponent<Collider>();
        _damageCasterCollider.enabled = false;
        _damagedTargetList = new List<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TargetTag) && !_damagedTargetList.Contains(other))
        {
            Character targetCharacter = other.GetComponent<Character>();

            if (targetCharacter != null)
            {
                targetCharacter.ApplyDamage(Damage);
            }

            _damagedTargetList.Add(other);
        }
    }

    public void EnableDamageCaster()
    {
        _damagedTargetList.Clear();
        _damageCasterCollider.enabled = true;
    }

    public void DisableDamageCaster()
    {
        _damagedTargetList.Clear();
        _damageCasterCollider.enabled = false;
    }
}
