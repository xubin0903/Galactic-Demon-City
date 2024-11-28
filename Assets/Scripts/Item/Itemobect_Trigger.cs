using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Itemobect_Trigger : MonoBehaviour
{
    private Item_Object item;
    private void Awake()
    {
        item = GetComponentInParent<Item_Object>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if (collision.GetComponent<CharacterStats>().isDead)
            {
                return;
            }
            item.PickupItem();
        }
    }
}
