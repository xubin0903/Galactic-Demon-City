using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
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
    public string itemID;
    [Range(0, 1)]
    public float itemDropChance;
    protected StringBuilder sb = new StringBuilder();
    public virtual string GetDescription()
    {
        return "";
    }
    private void OnValidate()
    {
// 在Unity编辑器中获取资产的路径并转换为GUID
#if UNITY_EDITOR
        string path = UnityEditor.AssetDatabase.GetAssetPath(this);
        itemID = AssetDatabase.AssetPathToGUID(path);
#endif

    }

}
