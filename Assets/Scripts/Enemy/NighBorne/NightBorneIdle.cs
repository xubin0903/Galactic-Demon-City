using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightBorneIdle : EnemyState
{
    public NightBorne enemy;
    public NightBorneIdle(Enemy enemyBase, EnemyStateMachine _stateMachine, string _animName, NightBorne _enemy) : base(enemyBase, _stateMachine, _animName)
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
