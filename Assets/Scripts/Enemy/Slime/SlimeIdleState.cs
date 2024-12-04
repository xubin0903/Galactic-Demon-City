using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeIdleState : EnemyState
{
    public Slime enemy;
    public SlimeIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName,Slime _enemy) : base(_enemyBase, _stateMachine, _animName)
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
        stateTimer -= Time.deltaTime;
        enemy.SetVelocity(Vector2.zero);
        if (stateTimer < 0)
        {
            enemy.stateMachine.ChangeState(enemy.moveState);
        }

    }
}
