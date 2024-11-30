using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;


public class InGame_UI : MonoBehaviour
{
    private SkillManager skill;
    public Image dashCooldown;
    public Image crystalCooldown;
    public Image blackHoleCoolDown;
    public Image swordCooldown;
    public Image flaskCooldown;
    public TextMeshProUGUI currentcy;
    private void Awake()
    {
        
    }
    private void Start()
    {
        skill = SkillManager.instance;
    }
    private void Update()
    {
        if (skill.dash_skill.dashUnlocked)
        {
            if (skill.dash_skill.coolTimer <= 0)
            {
                dashCooldown.fillAmount = 0;
                
            }
            else
            dashCooldown.fillAmount = skill.dash_skill.coolTimer / skill.dash_skill.coolDown;

        }
        else
        {
            dashCooldown.fillAmount = 1;
        }
        if (skill.crystal.crystalUnlocked)
        {
            if (skill.crystal.coolTimer <= 0)
            {
                crystalCooldown.fillAmount = 0;
                
            }
            else
            crystalCooldown.fillAmount = skill.crystal.coolTimer / skill.crystal.coolDown;
        }
        else
        {
            crystalCooldown.fillAmount = 1;
        }
        if (skill.blackhole.blackHoleUnlocked)
        {
            if(skill.blackhole.coolTimer <= 0)
            {
                blackHoleCoolDown.fillAmount = 0;
                
            }
            else
            blackHoleCoolDown.fillAmount = skill.blackhole.coolTimer / skill.blackhole.coolDown;
        }
        else
        {
            blackHoleCoolDown.fillAmount = 1;
        }
        if (skill.sword.canThrow)
        {
            if (skill.sword.coolTimer <= 0)
            {
                swordCooldown.fillAmount = 0;
                
            }
            else
            swordCooldown.fillAmount = skill.sword.coolTimer / skill.sword.coolDown;
        }
        else
        {
            swordCooldown.fillAmount = 1;
        }
        if (skill.crystal.crystalUnlocked)
        {
            if (skill.crystal.coolTimer <= 0)
            {
                crystalCooldown.fillAmount = 0;
                
            }
            else
            crystalCooldown.fillAmount = skill.crystal.coolTimer / skill.crystal.coolDown;
        }
        else
        {
            crystalCooldown.fillAmount = 1;
        }
        if (Inventory.instance.GetEquippedment(EquipmentType.Flask) != null)
        {
            if (Inventory.instance.flaskCoolTimer <= 0)
            {
                flaskCooldown.fillAmount = 0;
            }
            else
            {
                flaskCooldown.fillAmount = Inventory.instance.flaskCoolTimer / Inventory.instance.GetEquippedment(EquipmentType.Flask).cooldown;
            }
        }
        else
        {
            flaskCooldown.fillAmount = 1;
        }
        currentcy.text=PlayerManager.instance.currency.ToString("#,#");//格式化看起来更舒服
    }
}
