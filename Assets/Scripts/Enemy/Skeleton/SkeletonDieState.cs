using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class SkeletonDieState : EnemyState
{
    public Skeleton enemy;
    public SkeletonDieState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName,Skeleton _enemy) : base(_enemyBase, _stateMachine, _animName)
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
