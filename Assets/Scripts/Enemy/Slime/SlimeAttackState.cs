using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttackState : EnemyState
{
    public Slime enemy;
    public SlimeAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName,Slime _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Trigger()
    {
        base.Trigger();
    }

    public override void Update()
    {
        base.Update();
        //if (enemy.playerCheck.collider == null)
        //{
        //    enemy.stateMachine.ChangeState(enemy.moveState);
        //}
        if (triggerCalled)
        {
            enemy.stateMachine.ChangeState(enemy.idleState);
        }
        //∞‘ÃÂ

        enemy.SetZeroVelocity();
    }
}
