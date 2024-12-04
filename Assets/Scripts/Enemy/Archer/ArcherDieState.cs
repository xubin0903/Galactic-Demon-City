using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherDieState : EnemyState
{
    public Archer enemy;
    public ArcherDieState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName,Archer _enemy) : base(_enemyBase, _stateMachine, _animName)
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
        enemy.SetZeroVelocity();

    }
}
