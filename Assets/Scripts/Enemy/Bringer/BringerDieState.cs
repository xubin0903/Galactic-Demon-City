using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerDieState : EnemyState
{
    public Bringer enemy;
    public BringerDieState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName,Bringer _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        enemy=_enemy;
    }
    public override void Enter()
    {
        enemy.anim.SetBool(enemy.lastStateName, true);
        stateTimer = 0.3f;
        enemy.anim.speed = 0;
        enemy.rb.velocity = new Vector2(0, 20);
        enemy.cd.enabled = false;
    }



    public override void Update()
    {
        base.Update();
        if (stateTimer > 0)
        {
            enemy.rb.velocity = new Vector2(0, 20);

        }
        if (stateTimer <= 0)
        {
            enemy.rb.velocity = new Vector2(0, -10);
        }
    }
}
