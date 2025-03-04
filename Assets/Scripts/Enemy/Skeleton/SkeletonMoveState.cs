using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : EnemyState
{
    public Skeleton enemy;
    public SkeletonMoveState(Enemy enemyBase, EnemyStateMachine _stateMachine, string _animName,Skeleton _enemy) : base(_enemy, _stateMachine, _animName)
    {
        enemy=_enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.isMove = true;
    }

    public override void Exit()
    {
        base.Exit();
        enemy.isMove = false;
    }

    public override void Update()
    {
        base.Update();
       
        
        if (!enemy.isGrounded || enemy.isWall)
        {
           
            enemy.stateMachine.ChangeState(enemy.idleState);
            
        }

    }
}
