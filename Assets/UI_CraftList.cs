using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftList : MonoBehaviour
{
    [SerializeField] private GameObject craftSlotPrefab;
    [SerializeField] private Transform craftSlotsParent;
    [SerializeField] private List<ItemData_Equipment> craftEquipmentList=new List<ItemData_Equipment>();
    [SerializeField] private List<UI_Craft_Panel> craftLists;
    private void Awake()
    {
        craftLists = new List<UI_Craft_Panel>();
       
    }
    private void Start()
    {
        AssingCraftSlots();
        SetupCraftList();
        SetDefaultCraftWindow();
    }
    private void AssingCraftSlots()
    {
        for(int i = 0; i < craftSlotsParent.childCount; i++)
        {
            craftLists.Add(craftSlotsParent.GetChild(i).GetComponent<UI_Craft_Panel>());
        }
    }
    public void SetupCraftList()
    {
        for(int i = 0; i < craftLists.Count; i++)
        {
            Destroy(craftLists[i].gameObject);
        }
        craftLists.Clear();
        craftLists = new List<UI_Craft_Panel>();
        for (int i = 0; i < craftEquipmentList.Count; i++)
        {
            GameObject craftSlot = Instantiate(craftSlotPrefab, craftSlotsParent);
            craftLists.Add(craftSlot.GetComponentInChildren<UI_Craft_Panel>());
            craftLists[i].SetupCraftSlot(craftEquipmentList[i]);
        }
    }
    private void SetDefaultCraftWindow()
    {
        if (craftEquipmentList.Count == 0)
        {
            Debug.Log("No craft equipment found");
            return;
        }
        UI.instance.craftWindow.SetUpCraftWindow(craftEquipmentList[0]);
    }

  
}
