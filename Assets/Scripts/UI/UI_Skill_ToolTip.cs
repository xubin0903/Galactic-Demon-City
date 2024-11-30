using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Skill_ToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillPrice;
   
    public void ShowDescription(string description,string _skillName,string _skillPrice)
    {
        skillName.text=_skillName;
        skillDescription.text=description;
        skillPrice.text="Price: "+_skillPrice;
        gameObject.SetActive(true);
    }
    public void HideToolTip()=>gameObject.SetActive(false);
}
