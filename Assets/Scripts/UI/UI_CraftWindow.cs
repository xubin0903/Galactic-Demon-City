using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image[] materialsImages;
    [SerializeField] private Button craftButton;
    public void SetUpCraftWindow(ItemData_Equipment _itemData)
    {
        craftButton.onClick.RemoveAllListeners();
        for(int i = 0; i < materialsImages.Length; i++)
        {
            materialsImages[i].color=Color.clear;
            materialsImages[i].sprite=null;
            materialsImages[i].GetComponentInChildren<TextMeshProUGUI>().color=Color.clear;
        }
        for(int i = 0; i < _itemData.materials.Count; i++)
        {
            if (_itemData.materials.Count > materialsImages.Length)
            {
                Debug.Log("Too many materials for this item");
            }
            else
            {
                for(int j = 0; j < _itemData.materials.Count; j++)
                {
                    materialsImages[i].color = Color.white;
                    materialsImages[i].sprite = _itemData.materials[i].itemData.itemIcon;
                    TextMeshProUGUI textMesh = materialsImages[i].GetComponentInChildren<TextMeshProUGUI>();
                    textMesh.color =Color.white;
                    textMesh.text=_itemData.materials[i].amount.ToString();
                }
            }
            itemImage.color = Color.white;
            itemImage.sprite = _itemData.itemIcon;
            itemName.color = Color.white;
            itemName.text = _itemData.itemName;
            itemDescription.color = Color.white;
            itemDescription.text = _itemData.GetDescription();
        }
        craftButton.onClick.AddListener( () =>Inventory.instance.CraftItem(_itemData,_itemData.materials));
    }
}
