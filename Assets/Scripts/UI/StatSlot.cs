using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class StatSlot : MonoBehaviour
{
    [SerializeField] private string statName;
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI  textValue;
    [SerializeField] private TextMeshProUGUI textName;
    private void OnValidate()
    {
        gameObject.name = "StatSlot_"+statName;
        if(textName!= null)
        {
            textName.text = statName+":";
        }
    }
    private void Start()
    {
        UpdateStatValue();
    }
    public void UpdateStatValue()
    {
        CharacterStats stats = PlayerManager.instance.player.stats;
        if(stats!= null)
        {
            textValue.text = stats.GetStat(statType).GetValue().ToString();
        }
    }
}
