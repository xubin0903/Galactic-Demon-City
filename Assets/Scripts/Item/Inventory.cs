using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<ItemData> startingItems= new List<ItemData>();//��ʼ��Ʒ
    static public Inventory instance;
    public List<InventoryItem> inventoryItems = new List<InventoryItem>();//����
    public Dictionary<ItemData,InventoryItem> inventoryDictionary = new Dictionary<ItemData, InventoryItem>();//����
    public List<InventoryItem> equipmentItems = new List<InventoryItem>();//װ��
    public Dictionary<ItemData, InventoryItem> equipmentDictionary = new Dictionary<ItemData, InventoryItem>();//װ��
    public List<InventoryItem> equippedItems = new List<InventoryItem>();//װ���е���Ʒ
    public Dictionary<ItemData_Equipment, InventoryItem> equippedDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();//װ���е���Ʒ
    [Header("UI")]
    public Transform itemSlotParent;//����
    public Transform EquipmentSlotParent;//װ����
    public Transform equippedSlotParent;//װ��UI
    private ItemSlot_Controller[] itemSlots;
    private ItemSlot_Controller[] equipmentSlots;
    private ItemSlot_Controller[] equippedSlots;
    [Header("CoolDown")]
    public float flaskLastUseTime = -100000;
    public float armorLastUseTime = -100000;
    
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
    private void Start()
    {
        for(int i = 0; i < startingItems.Count; i++)
        {
            AddItem(startingItems[i]);
        }
        flaskLastUseTime = -100000;
        armorLastUseTime = -100000;
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
        //����ת��������
        ItemData_Equipment newitemData = (ItemData_Equipment)itemData;
        //�ж��Ƿ��Ѿ�װ��ͬ����װ��
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
            Debug.Log("�Ѿ�װ����ͬ����װ��");
            UnEquipItem(olditemData);
            AddItem(olditemData);


        }
        InventoryItem inventoryItem = new InventoryItem(itemData, 1);
        //����װ������
        newitemData.AddModifer();
        equippedItems.Add(inventoryItem);
        equippedDictionary.Add(newitemData, inventoryItem);
        RemoveItem(itemData);
      
    }
    public void UnEquipItem(ItemData_Equipment itemData)
    {
        if(equippedDictionary.TryGetValue(itemData,out InventoryItem valune))
        {
            Debug.Log(valune.itemData.name + "�Ѿ�ж��");
            equippedItems.Remove(valune);
            equippedDictionary.Remove(itemData);
            //�Ƴ�װ������
            itemData.RemoveModifer();
            UpdateSlotUI();

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
                Debug.Log("���ϲ���������");
                break;
            }
            itemSlots[i].UpdateSlot(inventoryItems[i]);
        }
        for(int i = 0; i < equipmentItems.Count; i++)
        {
           if (i >= equipmentSlots.Length)
            {
                Debug.Log("װ������������");
                break;

            }
            equipmentSlots[i].UpdateSlot(equipmentItems[i]);
        }
        for (int i = 0; i < equippedItems.Count; i++)
        {
            if(i >= equippedSlots.Length)
            {
                Debug.Log("װ������������");
                break;
            }
            equippedSlots[i].UpdateSlot(equippedItems[i]);
        }
        //����δʹ�õĲ�λ
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
    public bool CraftItem(ItemData_Equipment _craftEquipment, List<InventoryItem> requierMaterials)
    {
        //�ж��Ƿ����㹻�Ĳ���
        bool hasEnoughMaterial = true;
        List<InventoryItem> consumMaterials = new List<InventoryItem>();
        foreach(InventoryItem item in requierMaterials)
        {
            if(inventoryDictionary.TryGetValue(item.itemData,out InventoryItem valune))
            {
                if(valune.amount < item.amount)
                {
                    hasEnoughMaterial = false;
                    break;
                }
                else
                {
                   
                    consumMaterials.Add(valune);

                }
            }
            else
            {
                hasEnoughMaterial = false;
                break;
            }
            
        }
        if(!hasEnoughMaterial)
        {
            Debug.Log("���ϲ���");
            return false;
        }
        //���Ĳ���
        ConsumeMaterial(consumMaterials,requierMaterials);
      
     
        AddItem(_craftEquipment);
        return true;
    }
    public void ConsumeMaterial(List<InventoryItem> consumMaterials,List<InventoryItem> craftMaterials)
    {
        for (int i = 0; i < consumMaterials.Count; i++)
        {
            consumMaterials[i].amount -= craftMaterials[i].amount;
            if(consumMaterials[i].amount <= 0)
            {
                inventoryDictionary.Remove(consumMaterials[i].itemData);
                inventoryItems.Remove(consumMaterials[i]);
            }
        }
        UpdateSlotUI();
    }
    public List<InventoryItem> GetEquippedItems()
    {
        return equippedItems;
    }
    public ItemData_Equipment GetEquippedment(EquipmentType equipmentType)
    {
       ItemData_Equipment getEquipment = null;
        foreach(ItemData_Equipment key in equippedDictionary.Keys)
        {
            if(key.equipmentType == equipmentType)
            {
                getEquipment = key;
                break;
            }
        }
        return getEquipment;
    }
    public bool CanUseFlask()
    {
        ItemData_Equipment flask= GetEquippedment(EquipmentType.Flask);
       
        if(flask == null||Time.time<flaskLastUseTime+flask.cooldown)
        {
            
            return false;
        }
        else
        {
            flask.ExecuteEffects(null);
            flaskLastUseTime = Time.time;
            return true;
        }
    }
    public bool CanUseArmor()
    {
        ItemData_Equipment armor = GetEquippedment(EquipmentType.Armor);
        if (armor == null||Time.time<armor.cooldown+armorLastUseTime)
        {
            Debug.Log("װ����ȴ��");
            return false;
        }
        else
        {
            armorLastUseTime = Time.time;
            return true;
        }
    }
}
