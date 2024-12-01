using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    public bool dashUnlocked;
    public SkillSlot_UI dashUnlockedSlot;
    public bool dashCloneOnStartUnlocked;
    public SkillSlot_UI dashCloneOnStartUnlockedSlot;
    public override void UseSkill()
    {
        
        base.UseSkill();    
        //Debug.Log("Dash Skill");
    }
    public override bool CanUseSkill()
    {
        if(dashUnlocked == false)
        {
            return false;
        }
        return base.CanUseSkill();
    }
    protected override void Start()
    {
        base.Start();
        dashUnlockedSlot.GetComponent<Button>().onClick.AddListener(UnlockDash);
        dashCloneOnStartUnlockedSlot.GetComponent<Button>().onClick.AddListener(UnlockDashCloneOnStart);
    }

    private void UnlockDashCloneOnStart()
    {
        if(dashCloneOnStartUnlockedSlot != null && dashCloneOnStartUnlockedSlot.unlocked == true)
        {
            dashCloneOnStartUnlocked = true;
        }
    }

    private void UnlockDash()
    {
        if(dashUnlockedSlot!= null && dashUnlockedSlot.unlocked == true)
        {
            dashUnlocked = true;
        }
        
    }
    public void CreateCloneOnDashSatrt(Transform target, Vector3 offset)
    {
        if (dashCloneOnStartUnlocked)
        {
           SkillManager .instance.clone.CreateClone(target, offset, damage);
        }
    }
    protected override void CheckUnlocked()
    {
       UnlockDash();
       UnlockDashCloneOnStart();
    }
}
