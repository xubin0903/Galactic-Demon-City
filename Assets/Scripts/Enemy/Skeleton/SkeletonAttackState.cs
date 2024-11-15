using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    public Skeleton enemy;
    public SkeletonAttackState(Enemy enemyBase, EnemyStateMachine _stateMachine, string _animName,Skeleton _enemy) : base(enemyBase, _stateMachine, _animName)
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
        enemy.rb.velocity = Vector2.zero;
        
    }

}