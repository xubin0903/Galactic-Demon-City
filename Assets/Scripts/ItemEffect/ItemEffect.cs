using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item Effect", menuName = "Data/Item Effect")]
public class ItemEffect : ScriptableObject
{
    public string effectDescription;
    public virtual void ExecuteEffect(Transform target)
    {
        Debug.Log("Item Effect Executed");
    }
}
