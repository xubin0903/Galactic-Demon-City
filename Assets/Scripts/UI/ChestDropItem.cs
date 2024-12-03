using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestDropItem : EnemyDropItem
{
    public virtual void GenerateDropItem()
    {
        //从可能得到的道具中随机选择一些掉落
        int dropAmount = Random.Range(1, maxDropCount + 1);
        for (int i = 0; i < dropAmount; i++)
        {
            int itemIndex = Random.Range(0, baseItems.Length);
            possibleItemsList.Add(baseItems[itemIndex]);
        }
        foreach (ItemData item in possibleItemsList)
        {
            DropItem(item);
        }
    }
    
}
