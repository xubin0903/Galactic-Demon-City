using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(float _damage)
    {
        currentHealth -= _damage;
        Debug.Log(gameObject.name + "is taking damage:" + _damage);
        if (currentHealth <= 0)
        {

            Debug.Log("You Died");
        }
    
    }
}
