using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Craft_Panel : ItemSlot_Controller
{
    private void Update()
    {
        UpdateSlot(item);
        
    }
    public  override void OnPointerClick(PointerEventData eventData)
    {
        if(item == null||item.itemData == null)
        {
            Debug.Log("No item selected");
            return;
        }
        ItemData_Equipment craftItem = item.itemData as ItemData_Equipment;
        List<InventoryItem> materials = craftItem.materials;
        if (Inventory.instance.CraftItem(craftItem, materials))
        {

        }
    }
}
