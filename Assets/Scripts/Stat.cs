using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Stat 
{
    [SerializeField] private float baseValune;
    [SerializeField] public List<float> modifiers;
    public float GetValue ()
    {
        float finalValune = baseValune;
        foreach (float modifier in modifiers)
        {
            finalValune += modifier;
        }
        return finalValune;
    }
    public void AddModifier(float modifier)
    {
        modifiers.Add(modifier);
        //Debug.Log("Added modifier: " + modifier);
    }
    public void RemoveModifier(float modifier)
    {
        modifiers.Remove(modifier);
        //Debug.Log("Removed modifier: " + modifier);
    }

}
