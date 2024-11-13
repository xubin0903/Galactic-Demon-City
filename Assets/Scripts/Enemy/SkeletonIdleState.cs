using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class SkeletonIdleState : EnemyState
{
   
    public SkeletonIdleState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName) : base(_enemy, _stateMachine, _animName)
    {
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
    }
    public override void Update()
    {
        base.Update();
        stateTimer -= Time.deltaTime;
       enemy.SetVelocity(Vector2.zero);
        if (stateTimer < 0)
        {
           stateMachine.ChangeState(enemy.moveState);
        }
        
    }

}
