using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkeletonAttackState : EnemyState
{

    public SkeletonAttackState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName) : base(_enemy, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (enemy.playerCheck.collider == null)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }

}