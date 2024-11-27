using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]// This is a serializable class
public class InventoryItem 
{
    public  InventoryItem(ItemData itemData, int amount)
    {
        this.itemData = itemData;
        this.amount = amount;
    }
   public ItemData itemData;
   public int amount;
   public void Add()=>amount++;
   public void Remove()=>amount--;
}
