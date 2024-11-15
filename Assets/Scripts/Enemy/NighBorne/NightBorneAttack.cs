using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightBorneAttack :EnemyState
{ 
    public NightBorne enemy;
    public NightBorneAttack(Enemy enemyBase, EnemyStateMachine _stateMachine, string _animName, NightBorne _enemy) : base(enemyBase, _stateMachine, _animName)
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
