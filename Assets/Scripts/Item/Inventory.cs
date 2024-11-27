using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    static public Inventory instance;
    public List<InventoryItem> inventoryItems = new List<InventoryItem>();//材料
    public Dictionary<ItemData,InventoryItem> inventoryDictionary = new Dictionary<ItemData, InventoryItem>();//材料
    public List<InventoryItem> equipmentItems = new List<InventoryItem>();//装备
    public Dictionary<ItemData, InventoryItem> equipmentDictionary = new Dictionary<ItemData, InventoryItem>();//装备
    public List<InventoryItem> equippedItems = new List<InventoryItem>();//装备中的物品
    public Dictionary<ItemData_Equipment, InventoryItem> equippedDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();//装备中的物品
    [Header("UI")]
    public Transform itemSlotParent;//材料
    public Transform EquipmentSlotParent;//装备
    public Transform equippedSlotParent;//装备中的物品
    private ItemSlot_Controller[] itemSlots;
    private ItemSlot_Controller[] equipmentSlots;
    private ItemSlot_Controller[] equippedSlots;
    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);    
        }
        itemSlots = itemSlotParent.GetComponentsInChildren<ItemSlot_Controller>();
        equipmentSlots = EquipmentSlotParent.GetComponentsInChildren<ItemSlot_Controller>();
        equippedSlots = equippedSlotParent.GetComponentsInChildren<ItemSlot_Controller>();
    }
    public void AddItem(ItemData itemData)
    {
        if (itemData.itemType == ItemType.Material)
        {
            AddMaterial(itemData);

        }
        else if(itemData.itemType == ItemType.Equipment)
        {
            AddEquipment(itemData);
        }
        
        UpdateSlotUI();
    }
    public void EquipItem(ItemData itemData)
    {
        //父类转换成子类
        ItemData_Equipment newitemData = (ItemData_Equipment)itemData;
        //判断是否已经装备同类型装备
        ItemData_Equipment olditemData = null;
        foreach(ItemData_Equipment key in equippedDictionary.Keys)
        {
            if(key.equipmentType == newitemData.equipmentType)
            {
                olditemData = key;
            }
            
        
        }
        if(olditemData!= null)
        {
            Debug.Log("已经装备了同类型装备");
            UnEquipItem(olditemData);
            AddItem(olditemData);

        }
        InventoryItem inventoryItem = new InventoryItem(itemData, 1);
        //增加装备属性
        newitemData.AddModifer();
        equippedItems.Add(inventoryItem);
        equippedDictionary.Add(newitemData, inventoryItem);
        RemoveItem(itemData);
      
    }
    private void UnEquipItem(ItemData_Equipment itemData)
    {
        if(equippedDictionary.TryGetValue(itemData,out InventoryItem valune))
        {
            Debug.Log(valune.itemData.name + "已经卸下");
            equippedItems.Remove(valune);
            equippedDictionary.Remove(itemData);
            //移除装备属性
            itemData.RemoveModifer();
        }
       
    }
    private void AddEquipment(ItemData itemData)
    {
        if (equipmentDictionary.ContainsKey(itemData))
        {
            equipmentDictionary[itemData].amount++;
        }
        else
        {
            InventoryItem inventoryItem = new InventoryItem(itemData, 1);
            equipmentDictionary.Add(itemData, inventoryItem);
            equipmentItems.Add(inventoryItem);
        }
    }

    private void AddMaterial(ItemData itemData)
    {
        if (inventoryDictionary.ContainsKey(itemData))
        {
            inventoryDictionary[itemData].amount++;
        }
        else
        {
            InventoryItem inventoryItem = new InventoryItem(itemData, 1);
            inventoryDictionary.Add(itemData, inventoryItem);
            inventoryItems.Add(inventoryItem);
        }
    }

    public void RemoveItem(ItemData itemData)
    {
        if(inventoryDictionary.ContainsKey(itemData))
        {
            inventoryDictionary[itemData].amount--;
            if(inventoryDictionary[itemData].amount <= 0)
            {
                InventoryItem inventoryItem = inventoryDictionary[itemData];
                inventoryDictionary.Remove(itemData);
                inventoryItems.Remove(inventoryItem);
            }
        }
        else if (equipmentDictionary.ContainsKey(itemData))
        {
            equipmentDictionary[itemData].amount--;
            if (equipmentDictionary[itemData].amount <= 0)
            {
                InventoryItem inventoryItem = equipmentDictionary[itemData];
                equipmentDictionary.Remove(itemData);
                equipmentItems.Remove(inventoryItem);
            }
        }
        UpdateSlotUI();
    }
    public void UpdateSlotUI()
    {
        for(int i = 0; i <inventoryItems.Count; i++)
        {
            if (i >= itemSlots.Length)
            {
                Debug.Log("材料槽数量不足");
                break;
            }
            itemSlots[i].UpdateSlot(inventoryItems[i]);
        }
        for(int i = 0; i < equipmentItems.Count; i++)
        {
           if (i >= equipmentSlots.Length)
            {
                Debug.Log("装备槽数量不足");
                break;

            }
            equipmentSlots[i].UpdateSlot(equipmentItems[i]);
        }
        for (int i = 0; i < equippedItems.Count; i++)
        {
            if(i >= equippedSlots.Length)
            {
                Debug.Log("装备槽数量不足");
                break;
            }
            equippedSlots[i].UpdateSlot(equippedItems[i]);
        }
        //清理未使用的槽位
        for(int i = inventoryItems.Count; i < itemSlots.Length; i++)
        {
            itemSlots[i].ClearSlot();
        }
        for (int i = equipmentItems.Count; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].ClearSlot();
        }
        for (int i = equippedItems.Count; i < equippedSlots.Length; i++)
        {
            equippedSlots[i].ClearSlot();
        }
    }
}
