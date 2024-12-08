using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour,ISaveManager
{
    public List<ItemData> startingItems= new List<ItemData>();//初始物品
    static public Inventory instance;
    public List<InventoryItem> inventoryItems = new List<InventoryItem>();//材料
    public Dictionary<ItemData,InventoryItem> inventoryDictionary = new Dictionary<ItemData, InventoryItem>();//材料
    public List<InventoryItem> equipmentItems = new List<InventoryItem>();//装备
    public Dictionary<ItemData, InventoryItem> equipmentDictionary = new Dictionary<ItemData, InventoryItem>();//装备
    public List<InventoryItem> equippedItems = new List<InventoryItem>();//装备中的物品
    public Dictionary<ItemData_Equipment, InventoryItem> equippedDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();//装备中的物品
    [Header("UI")]
    public Transform itemSlotParent;//材料
    public Transform EquipmentSlotParent;//装备栏
    public Transform equippedSlotParent;//装备UI
    public Transform statsSlot;//数值栏

    private ItemSlot_Controller[] itemSlots;
    public ItemSlot_Controller[] equipmentSlots;//待装备栏
    private ItemSlot_Controller[] equippedSlots;//装备中的栏
    private StatSlot[] statSlots;
    [Header("CoolDown")]
    public float flaskCoolTimer;
    public float armorLastUseTime = -100000;
    [Header("Data Base")]
    public List<ItemData> itemDataBase;
    public List<InventoryItem> loadItems;

    
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
        statSlots = statsSlot.GetComponentsInChildren<StatSlot>();
    }
    private void Start()
    {
        AssingStartingItems();
        armorLastUseTime = -100000;
    }

    private void AssingStartingItems()
    {
        if (loadItems.Count > 0)
        {
            for (int i = 0; i < loadItems.Count; i++)
            {
                for(int j = 0; j < loadItems[i].amount; j++)
                {
                    AddItem(loadItems[i].itemData);
                }
            }
            return;
        }
        for (int i = 0; i < startingItems.Count; i++)
        {
            AddItem(startingItems[i]);
        }
    }

    private void Update()
    {
        flaskCoolTimer -= Time.deltaTime;
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
    public void UnEquipItem(ItemData_Equipment itemData)
    {
        if(equippedDictionary.TryGetValue(itemData,out InventoryItem valune))
        {
            Debug.Log(valune.itemData.name + "已经卸下");
            equippedItems.Remove(valune);
            equippedDictionary.Remove(itemData);
            //移除装备属性
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
        for(int i = 0; i < statSlots.Length; i++)
        {
            statSlots[i].UpdateStatValue();
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
    public bool CraftItem(ItemData_Equipment _craftEquipment, List<InventoryItem> requierMaterials)
    {
        //判断是否有足够的材料
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
            Debug.Log("材料不足");
            UI.instance.ShowPop("制作失败，材料不足");
            return false;
        }
        //消耗材料
        ConsumeMaterial(consumMaterials,requierMaterials);
        UI.instance.ShowPop("制作成功，返回Character面板装备上新装备吧");
      
     
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
       
        if(flask == null||flaskCoolTimer>0)
        {
            if(flask == null)
                PlayerManager.instance.player.fx.GeneratePopToolTip("没有药水");
            else
            PlayerManager.instance.player.fx.GeneratePopToolTip("药水冷却中");
            return false;
        }
        else
        {
            flask.ExecuteEffects(null);
            flaskCoolTimer = flask.cooldown;
            return true;
        }
    }
    public bool CanUseArmor()
    {
        ItemData_Equipment armor = GetEquippedment(EquipmentType.Armor);
        if (armor == null||Time.time<armor.cooldown+armorLastUseTime)
        {
            //Debug.Log("装备冷却中");
            return false;
        }
        else
        {
            armorLastUseTime = Time.time;
            return true;
        }
    }

    void ISaveManager.LoadData(GameData _gameData)
    {
        Debug.Log("加载背包数据");
       foreach(KeyValuePair<string,int> pair in _gameData.Inventory)
        {
            foreach(var item in itemDataBase)
            {
                if(item!=null&&item.itemID == pair.Key)
                {
                    InventoryItem inventoryItem = new InventoryItem(item, pair.Value);
                    loadItems.Add(inventoryItem);
                }
            }
        }
    }

    void ISaveManager.SaveData(ref GameData _gameData)
    {
        Debug.Log("保存背包数据");
       _gameData.Inventory.Clear();
        foreach(KeyValuePair<ItemData,InventoryItem> pair in inventoryDictionary)
        {
            _gameData.Inventory.Add(pair.Key.itemID,pair.Value.amount);
        }
        foreach (KeyValuePair<ItemData, InventoryItem> pair in equipmentDictionary)
        {
            _gameData.Inventory.Add(pair.Key.itemID, pair.Value.amount);
        }
        foreach(KeyValuePair<ItemData_Equipment,InventoryItem> pair in equippedDictionary)
        {
            _gameData.Inventory.Add(pair.Key.itemID, pair.Value.amount);
        }
    }
#if UNITY_EDITOR
    [ContextMenu("生成基础物品（所有物品）")]
    private void FiilUpItemDataBase()=>itemDataBase = new List<ItemData>(GetItemDataBase());
    private List<ItemData> GetItemDataBase()
    {
        itemDataBase = new List<ItemData>();
        string[] assetNames = AssetDatabase.FindAssets("", new[] {"Assets/Data/Item"});
        foreach(string SOname in assetNames)
        {
            var SOpath= AssetDatabase.GUIDToAssetPath(SOname);
            var iteData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            itemDataBase.Add(iteData);
        }
        return itemDataBase;
    }
#endif

}
