using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot_Controller : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public InventoryItem item;
    public Image image;
    public TextMeshProUGUI itemamount;
    protected  virtual void Awake()
    {
        image=GetComponent<Image>();
        itemamount=GetComponentInChildren<TextMeshProUGUI>();
        image.color = Color.clear;
        itemamount.text = "";
        

    }
    public  virtual void OnPointerClick(PointerEventData eventData)
    {
        if (item != null && item.itemData != null && Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.itemData);
            return;
        }
       
        if (item!= null && item.itemData!= null && item.itemData.itemType == ItemType.Equipment)
        {
           
            Inventory.instance.EquipItem(item.itemData);
            UI.instance.itemToolTip.HideTip();
        }
        else
        {
            return;
        }
    }

    public  virtual void UpdateSlot(InventoryItem _item)
    {
        if (_item == null||_item.itemData==null||image==null)
        {
            
            return;
        }
            this.gameObject.SetActive(true);
            item=_item;
            image.color = Color.white;
            image.sprite = item.itemData.itemIcon;
            if(item.amount>1)
            itemamount.text = item.amount.ToString();
            else
            itemamount.text = "";
            
        
    }
    public void ClearSlot()
    {
        item = null;
        image.color = Color.clear;
        image.sprite = null;

        itemamount.text = "";
    }
    protected virtual void Update()
    {
        if (item == null)
        {
            image.color = Color.clear;
            itemamount.text = "";
        }
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null &&item.itemData != null)
        {
            UI.instance.itemToolTip.ShowTip(item.itemData as ItemData_Equipment);
        }
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (item != null &&item.itemData != null)
        {
            UI.instance.itemToolTip.HideTip();
        }
    }
}
