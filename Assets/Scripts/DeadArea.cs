using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null || collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<CharacterStats>().currentHealth = 0;
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}
