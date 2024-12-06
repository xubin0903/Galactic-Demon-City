using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class SkeletonIdleState : EnemyState
{
    public Skeleton enemy;
   
    public SkeletonIdleState(Enemy enemyBase, EnemyStateMachine _stateMachine, string _animName,Skeleton _enemy) : base(enemyBase, _stateMachine, _animName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer =enemy.idleTimer;
    }

    public override void Exit()
    {
        base.Exit();
        if (enemy.isWall || !enemy.isGrounded)
        {
            enemy.Flip();
        }
        AudioManager.instance.PlaySFX(24,enemy.transform);
    }
    public override void Update()
    {
        base.Update();
       
       enemy.SetVelocity(Vector2.zero);
        if (stateTimer < 0)
        {
           enemy.stateMachine.ChangeState(enemy.moveState);
        }
        
    }

}
