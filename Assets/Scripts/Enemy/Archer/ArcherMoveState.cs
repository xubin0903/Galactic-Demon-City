using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherMoveState : EnemyState
{
    public Archer enemy;
    public ArcherMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName,Archer _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        enemy = _enemy;
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
