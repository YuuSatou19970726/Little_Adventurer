using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField]
    private Transform ShootingPoint;
    [SerializeField]
    private GameObject DamageOrb;
    private Character _character;

    void Awake()
    {
        _character = GetComponent<Character>();
    }

    public void ShootTheDamageOrb()
    {
        Instantiate(DamageOrb, ShootingPoint.position, Quaternion.LookRotation(ShootingPoint.forward));
    }

    void Update()
    {
        _character.RotationToTaget();
    }
}
