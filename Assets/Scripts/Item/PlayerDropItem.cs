using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDropItem : EnemyDropItem
{
    public float dropItemChance;
    public virtual void GenerateDropItem()
    {
        Inventory inventory = Inventory.instance;
        List<InventoryItem> equippedItems = inventory.GetEquippedItems();
        if(equippedItems.Count == 0)
        {
            return;
        }
       for(int i = 0; i < equippedItems.Count; i++)
        {
            if(Random.value >= dropItemChance)
            {
                DropItem(equippedItems[i].itemData);
                inventory.UnEquipItem(equippedItems[i].itemData as ItemData_Equipment);
            }
        }
    }
}
