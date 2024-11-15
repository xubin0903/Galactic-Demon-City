using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;

public class EnemyState 
{
    protected Enemy enemyBase;
    protected EnemyStateMachine stateMachine;
    public string animName;
    protected float stateTimer;
    protected bool triggerCalled;

    public  EnemyState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName)
    {
        this.enemyBase = _enemy;
        this.stateMachine = _stateMachine;
        this.animName = _animName;
    }

    public virtual void Enter()
    {
        
        Debug.Log("Enter " + animName);
        enemyBase.anim.SetBool(animName, true);
        triggerCalled = false;
        enemyBase.currentState = this;
    }

    public virtual void Exit()
    {
      Debug.Log("Exit " + animName);
      enemyBase.anim.SetBool(animName, false);
    }

    public virtual void Update()
    {
        Debug.Log("Update " + animName);
        stateTimer-=Time.deltaTime;
    }
    public virtual void Trigger()
    {
        triggerCalled = true;
    }
}
