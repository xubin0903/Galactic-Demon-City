using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot_Controller : MonoBehaviour,IPointerClickHandler
{
    public InventoryItem item;
    public Image slotImage;
    public TextMeshProUGUI itemamount;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item!= null && item.itemData!= null && item.itemData.itemType == ItemType.Equipment)
        {
           
            Inventory.instance.EquipItem(item.itemData);
        }
        else
        {
            return;
        }
    }

    public void UpdateSlot(InventoryItem _item)
    {
            
            this.gameObject.SetActive(true);
            item=_item;
            slotImage.color = Color.white;
            slotImage.sprite = item.itemData.itemIcon;
            if(item.amount>1)
            itemamount.text = item.amount.ToString();
            else
            itemamount.text = "";
            
        
    }
    public void ClearSlot()
    {
        item = null;
        slotImage.color = Color.clear;
        slotImage.sprite = null;

        itemamount.text = "";
    }
    private void Update()
    {
        if (item == null)
        {
            slotImage.color = Color.clear;
            itemamount.text = "";
        }
    }

}
