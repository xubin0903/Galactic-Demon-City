using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeDied : EnemyState
{
    public Slime enemy;
    private float backTime;
    public SlimeDied(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName,Slime _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        enemy=_enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.rb.velocity = new Vector2(5 * enemy.faceDir, 8);
        backTime = 0.15f;
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
        backTime -= Time.deltaTime;
        if (backTime <= 0)
        {
            enemy.rb.velocity=new Vector2(0,0);
        }

    }
}
