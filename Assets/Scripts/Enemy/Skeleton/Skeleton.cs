using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    public EnemyStateMachine stateMachine;
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }
    public override void Awake()
    {
        base.Awake();
        idleState = new SkeletonIdleState(this, stateMachine, "Idle", this);
        moveState = new SkeletonMoveState(this, stateMachine, "Move", this);
        attackState = new SkeletonAttackState(this, stateMachine, "Attack", this);
        stateMachine= new EnemyStateMachine();

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
