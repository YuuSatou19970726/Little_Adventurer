using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOrb : MonoBehaviour
{
    [SerializeField]
    private float Speed = 2f;
    [SerializeField]
    private int Damage = 10;
    [SerializeField]
    private ParticleSystem HitVFX;
    private Rigidbody _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        _rigidbody.MovePosition(transform.position + transform.forward * Speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        Character character = other.gameObject.GetComponent<Character>();
        if (character != null && character.isPlayer)
        {
            character.ApplyDamage(Damage, transform.position);
        }

        Instantiate(HitVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
