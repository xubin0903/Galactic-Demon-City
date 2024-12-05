using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherJumpState : EnemyState
{
    public Archer enemy;
    private float backTime;
    public ArcherJumpState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName,Archer _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.rb.velocity = new Vector2(enemy.jumpBackOPos.x * (-enemy.faceDir), enemy.jumpBackOPos.y);
        backTime = 0.2f;

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
        enemy.anim.SetFloat("yVelocity", enemy.rb.velocity.y);
        if (enemy.isGrounded&&backTime<=0)
        {
            Debug.Log("change");
            enemy.stateMachine.ChangeState(enemy.idleState);
        }
        backTime -= Time.deltaTime;
        if (backTime <= 0)
        {
            enemy.SetZeroVelocity();
        }
    }
}
