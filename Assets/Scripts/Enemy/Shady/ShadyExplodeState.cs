using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyExplodeState : EnemyState
{
    public Shady enemy;
    public ShadyExplodeState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName,Shady _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Trigger()
    {
        base.Trigger();
    }

    public override void Update()
    {
        base.Update();
    }
}
