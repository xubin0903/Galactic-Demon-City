using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightBorne : Enemy
{
    public EnemyStateMachine stateMachine;
    public NightBorneAttack attackState;
    public NightBorneMove moveState;
    public NightBorneIdle idleState;

    public override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        idleState = new NightBorneIdle(this, stateMachine,"Idle",this);
        moveState = new NightBorneMove(this, stateMachine,"Move",this);
        attackState = new NightBorneAttack(this, stateMachine,"Attack",this);
        
    }
    public override void Start()
    {
        base.Start();
        currentSpeed = beginSpeed;
        stateMachine.Initialize(idleState);

    }
    public override void Update()
    {
        CanAttack();
        base.Update();
        stateMachine.currentState.Update();
        if (stateMachine.currentState == moveState)
        {
            Move();
        }
        CollisionCheck();
        FindPlayerAccelerate();

    }

    private void FindPlayerAccelerate()
    {
        if (isFrozen)
        {
            return;
        }
        if (playerCheck.collider != null)
        {
            if (playerCheck.distance > 1)
            {
                if (currentSpeed < maxSpeed)
                {
                    currentSpeed += Time.deltaTime * 0.8f;
                    //Debug.Log(currentSpeed);

                }
            }
            else if (playerCheck.distance < 1)
            {
                SetZeroVelocity();
                if (stateMachine.currentState != attackState)
                {
                    if (canAttack)
                    {
                        stateMachine.ChangeState(attackState);
                        lastAttackTime = Time.time;
                    }
                }

            }
        }
        else
        {
            if (currentSpeed > beginSpeed)
            {
                currentSpeed -= Time.deltaTime;
            }

        }
    }

    public void AttackFinish()
    {
        stateMachine.currentState.Trigger();
    }
}
