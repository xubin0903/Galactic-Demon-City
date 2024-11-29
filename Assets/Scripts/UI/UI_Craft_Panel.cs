using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_Craft_Panel : ItemSlot_Controller
{
    //public Image imageChildren;
    //public TextMeshProUGUI itemName;
    
    protected override void Awake()
    {
        
    }
    public void SetupCraftSlot(ItemData_Equipment _itemData)
    {
        if (_itemData == null)
        {
            return;
        }

        item.itemData = _itemData;
        image.color = Color.white;
        image.sprite = item.itemData.itemIcon;
        itemamount.text = item.itemData.itemName;//这里父类是用来计数的为了头方便这把直接用来显示名字

    }
    public  override void OnPointerClick(PointerEventData eventData)
    {
        if(item == null||item.itemData == null)
        {
            Debug.Log("No item selected");
            return;
        }
        ItemData_Equipment craftItem = item.itemData as ItemData_Equipment;
        UI.instance.craftWindow.SetUpCraftWindow(craftItem);
        
    }
}
