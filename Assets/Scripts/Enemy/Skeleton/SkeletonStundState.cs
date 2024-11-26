using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStundState : EnemyState
{
    public Skeleton enemy;
    public float backTimer;
    public SkeletonStundState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName,Skeleton  _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.stundDuration;
        backTimer = 0.15f;
        enemy.rb.velocity = new Vector2(enemy.stundDir.x * (-enemy.faceDir), enemy.stundDir.y);
        enemy.fx.InvokeRepeating("RedColorBlink", 0, .1f);
        enemy.Stunned = true;

        
    }

    public override void Exit()
    {
        base.Exit();
        enemy.fx.Invoke("CancelColorBlink", 0);
        Debug.Log("Exit Stund State");
        enemy.Stunned = false;
    }

    

    public override void Update()
    {
        base.Update();
        if (stateTimer <= 0)
        {
            enemy.stateMachine.ChangeState(enemy.idleState);
            
        }
        backTimer -= Time.deltaTime;
        if (backTimer <= 0)
        {
            enemy.rb.velocity = new Vector2(0, 0);
            
        }
       
        
    }
    
}
