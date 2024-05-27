using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int MaxHealth;
    private int CurrentHealth;
    private Character _character;

    void Awake()
    {
        CurrentHealth = MaxHealth;
        _character = GetComponent<Character>();
    }

    public void ApplyDamage(int damage)
    {
        CurrentHealth -= damage;
        Debug.Log(gameObject.name + " took damage: " + damage);
        Debug.Log(gameObject + " currentHealth: " + CurrentHealth);

        CheckHealth();
    }

    private void CheckHealth()
    {
        if (CurrentHealth <= 0)
        {
            _character.SwitchStateTo(Character.CharacterState.DEAD);
        }
    }
}
