using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Object : MonoBehaviour
{
    public ItemData item;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
        
    }
    private void OnValidate()
    {
        if (item == null)
        {
            return;
        }
        spriteRenderer=GetComponent<SpriteRenderer>();
        spriteRenderer.sprite=item.itemIcon;
        gameObject.name="Item_Object:"+item.itemName;
        if (item == null)
        {
            Debug.Log("Item is null");
        }
        
    }
    public void SetUp(ItemData item,Vector2 dropForce)
    {
        this.item=item;
        spriteRenderer=GetComponent<SpriteRenderer>();
        spriteRenderer.sprite=item.itemIcon;
        gameObject.name="Item_Object:"+item.itemName;
        rb.velocity=dropForce;
    }

    public void PickupItem()
    {
        Debug.Log("Pick up item");
        Inventory.instance.AddItem(item);
        Destroy(gameObject);
    }
}
