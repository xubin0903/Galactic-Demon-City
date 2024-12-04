using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAttackState : EnemyState
{
    public Archer enemy;
    public ArcherAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName,Archer _enemy) : base(_enemyBase, _stateMachine, _animName)
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
