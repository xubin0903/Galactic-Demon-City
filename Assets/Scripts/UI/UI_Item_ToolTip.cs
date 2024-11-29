using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Item_ToolTip : MonoBehaviour
{
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemType;
    public TextMeshProUGUI itemDesc;
   
    
    public void ShowTip(ItemData_Equipment itemData)
    {
        if(itemData == null)
        {
            return;
        }
        itemName.text = itemData.itemName;
        itemType.text = itemData.equipmentType.ToString();
        itemDesc.text = itemData.GetDescription();
        this.gameObject.SetActive(true);
    }
    public void HideTip()
    {
        this.gameObject.SetActive(false);
    }
}
