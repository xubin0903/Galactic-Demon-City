using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Object : MonoBehaviour
{
    public ItemData item;
    private SpriteRenderer spriteRenderer;
    private void OnValidate()
    {
        spriteRenderer=GetComponent<SpriteRenderer>();
        spriteRenderer.sprite=item.itemIcon;
        gameObject.name="Item_Object:"+item.itemName;
        if (item == null)
        {
            Debug.Log("Item is null");
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            Debug.Log("Pick up item");
            Inventory.instance.AddItem(item);
            Destroy(gameObject);
        }
    }
}
