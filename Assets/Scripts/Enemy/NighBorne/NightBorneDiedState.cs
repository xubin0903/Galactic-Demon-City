using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightBorneDiedState : EnemyState
{
    public NightBorne enemy;
    public NightBorneDiedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName,NightBorne _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        this.enemy = _enemy;
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
