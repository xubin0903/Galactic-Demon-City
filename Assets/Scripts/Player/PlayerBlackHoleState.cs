using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    private float flyTime=.4f;
    private bool usedSkill;
    private float defaultGravity;
    public PlayerBlackHoleState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        defaultGravity = player.rb.gravityScale;
        base.Enter();
        usedSkill = false;
        stateTimer = flyTime;
        player.rb.gravityScale = 0;
        player.isBlackHole = true;
        AudioManager.instance.PlaySFX(3, null);
        
    }

    public override void Exit()
    {
        base.Exit();
        player.rb.gravityScale = defaultGravity;
      
       
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer > 0)
        {
            rb.velocity =new Vector2(0, 15);
        }
        else
        {
            rb.velocity=new Vector2(0, -0.1f);
            if (!usedSkill)
            {
                if (SkillManager.instance.blackhole.CanUseSkill())
                {
                    usedSkill = true;
                }
            }
        }
        
    }
    /*状态的退出在BlackHole_Skill_Controller中当攻击完成时调用*/
}
