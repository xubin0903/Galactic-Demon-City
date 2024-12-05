using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyIdleState : EnemyState
{
    public Shady enemy;
    public ShadyIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName,Shady _enemy) : base(_enemyBase, _stateMachine, _animName)
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
        AudioManager.instance.PlaySFX(24, enemy.transform);
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
