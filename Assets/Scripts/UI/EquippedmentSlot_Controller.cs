using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquippedmentSlot_Controller : ItemSlot_Controller
{
    public EquipmentType equipmentType;
    private void OnValidate()
    {
        gameObject.name = "Equippedmentobject _"+equipmentType.ToString();
    }
    
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (item == null || item.itemData == null)
        {
            return;
        }

        ItemData_Equipment Equipment = item.itemData as ItemData_Equipment;
        Inventory.instance.UnEquipItem(Equipment);
        Inventory.instance.AddItem(Equipment);
        UI.instance.itemToolTip.HideTip();
        ClearSlot();
    }
}
