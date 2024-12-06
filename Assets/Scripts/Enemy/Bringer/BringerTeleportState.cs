using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerTeleportState : EnemyState
{
    public Bringer enemy;
    public BringerTeleportState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName,Bringer _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("BringerTeleportState:Enter");
        stateTimer = 1;
       
       
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
        if (stateTimer <= 0)
        {
            enemy.stateMachine.ChangeState(enemy.idleState);
        }
    }
}
