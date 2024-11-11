using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState 
{
    protected Enemy enemy;
    protected EnemyStateMachine stateMachine;
    protected string animName;
    protected float stateTimer;
    protected bool triggerCalled;

    public  EnemyState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName)
    {
        this.enemy = _enemy;
        this.stateMachine = _stateMachine;
        this.animName = _animName;
    }

    public virtual void Enter()
    {
        Debug.Log("Enter " + animName);
        enemy.anim.SetBool(animName, true);
    }

    public virtual void Exit()
    {
      Debug.Log("Exit " + animName);
      enemy.anim.SetBool(animName, false);
    }

    public virtual void Update()
    {
        Debug.Log("Update " + animName);
        stateTimer -= Time.deltaTime;
    }
}
