using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerAttackState : EnemyState
{
    public Bringer enemy;
    public BringerAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName,Bringer _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.teleportChance += 6;
        AudioManager.instance.PlaySFX(20, enemy.transform);
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
        if (triggerCalled&&enemy.CanTeleport())
        {
            Debug.Log("teleport100");
            enemy.stateMachine.ChangeState(enemy.teleportState);
        }else if (triggerCalled)
        {
            Debug.Log("ÎÞ·¨¹¥»÷");
            enemy.stateMachine.ChangeState(enemy.moveState);
        }
        //°ÔÌå

        enemy.SetZeroVelocity();
    }
}
