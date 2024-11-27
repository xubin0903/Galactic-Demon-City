using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    Equipment,
    Material,
}
[CreateAssetMenu(fileName = "New Item", menuName = "Data/ItemData")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemIcon;

}
