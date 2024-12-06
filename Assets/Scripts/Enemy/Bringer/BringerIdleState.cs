using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerIdleState : EnemyState
{
    public Bringer enemy;
    public BringerIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName,Bringer _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTimer;
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

        enemy.SetVelocity(Vector2.zero);
        if (stateTimer < 0)
        {
            enemy.stateMachine.ChangeState(enemy.moveState);
        }

    }

}
