using System.Collections;
using System.Collections.Generic;
using System.Text;
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
    [Range(0, 1)]
    public float itemDropChance;
    protected StringBuilder sb = new StringBuilder();
    public virtual string GetDescription()
    {
        return "";
    }

}
