using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
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
                targetCharacter.ApplyDamage(Damage, transform.parent.position);

                PlayerVFXManager playerVFXManager = transform.parent.GetComponent<PlayerVFXManager>();

                if (playerVFXManager != null)
                {
                    if (_damageCasterCollider == null)
                        _damageCasterCollider = GetComponent<Collider>();

                    RaycastHit hit;

                    Vector3 originalPos = transform.position + (-_damageCasterCollider.bounds.extents.z) * transform.forward;
                    bool isHit = Physics.BoxCast(originalPos, _damageCasterCollider.bounds.extents / 2, transform.forward,
                    out hit, transform.rotation, _damageCasterCollider.bounds.extents.z, 1 << 6);

                    if (isHit)
                    {
                        playerVFXManager.PlaySlash(hit.point + new Vector3(0, 0.5f, 0));
                    }
                }
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

    // void OnDrawGizmos()
    // {
    //     if (_damageCasterCollider == null)
    //         _damageCasterCollider = GetComponent<Collider>();

    //     RaycastHit hit;

    //     Vector3 originalPos = transform.position + (-_damageCasterCollider.bounds.extents.z) * transform.forward;
    //     bool isHit = Physics.BoxCast(originalPos, _damageCasterCollider.bounds.extents / 2, transform.forward,
    //     out hit, transform.rotation, _damageCasterCollider.bounds.extents.z, 1 << 6);

    //     if (isHit)
    //     {
    //         Gizmos.color = Color.yellow;
    //         Gizmos.DrawWireSphere(hit.point, 0.3f);
    //     }
    // }
}
