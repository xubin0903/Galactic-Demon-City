using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStunnedState : EnemyState
{
    public Slime enemy;
    private float backTimer;
    private bool isTrigger;
    public SlimeStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName,Slime _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        this.enemy=_enemy;

    }

    public override void Enter()
    {
        base.Enter();
        
        backTimer = 0.15f;
        enemy.rb.velocity = new Vector2(enemy.stundDir.x * (-enemy.faceDir), enemy.stundDir.y);
       
        enemy.Stunned = true;
        enemy.vulnerableTimer=enemy.vunlerabilityDuration;
        enemy.isVulnerable = true;


    }

    public override void Exit()
    {
        base.Exit();
      
        enemy.Stunned = false;
    }



    public override void Update()
    {
        stateTimer+=Time.deltaTime;
        
        if (stateTimer >= enemy.stundDuration && !isTrigger)
        {
            isTrigger=true;
            enemy.anim.SetTrigger("Stun");
            enemy.isVulnerable = false;
        }
        backTimer -= Time.deltaTime;
        if (backTimer <= 0)
        {
            enemy.rb.velocity = new Vector2(0, 0);

        }


    }
}
