using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropItem : MonoBehaviour
{
    public GameObject dropPrefab;
    public ItemData[] baseItems;
    public List<ItemData> possibleItemsList;
    public int maxDropCount;
    public float dropBackX;
    public float mindropBackY;
    public float maxDropBackY;

    private void Start()
    {
        possibleItemsList = new List<ItemData>();
    }

   public  virtual void GenerateDropItems()
    {
        foreach (var item in baseItems)
        {
            if (Random.Range(0, 1f) >=item.itemDropChance)
            {
                possibleItemsList.Add(item);
            }
        }
        for (int i = 0; i < maxDropCount; i++)
        {
            if (possibleItemsList.Count > 0)
            {
                var itemData = possibleItemsList[Random.Range(0, possibleItemsList.Count)];
                possibleItemsList.Remove(itemData);
                DropItem(itemData);
            }
        }

    }
    protected void DropItem(ItemData itemData)
    {
        if (dropPrefab == null|| itemData == null)
        {
            return;
        }
        Debug.Log("DropItem");
        var newItem = Instantiate(dropPrefab, transform.position, transform.rotation);
        var item=newItem.GetComponent<Item_Object>();
        Vector2 randomDropForce=new Vector2(Random.Range(-dropBackX,dropBackX),Random.Range(mindropBackY,maxDropBackY));
        item.SetUp(itemData,randomDropForce);
    }
}
