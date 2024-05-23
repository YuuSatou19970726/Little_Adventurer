using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int MaxHealth;
    private int CurrentHealth;
    private Character character;

    void Awake()
    {
        CurrentHealth = MaxHealth;
        character = GetComponent<Character>();
    }

    public void ApplyDamage(int damage)
    {
        CurrentHealth -= damage;
        Debug.Log(gameObject.name + " took damage: " + damage);
        Debug.Log(gameObject + " currentHealth: " + CurrentHealth);
    }
}
