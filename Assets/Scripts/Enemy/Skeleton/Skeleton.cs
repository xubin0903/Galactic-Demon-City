using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    #region ×´Ì¬»ú
    public EnemyStateMachine stateMachine;
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }
    public SkeletonStundState stundState { get; private set; }
    public SkeletonDieState dieState { get; private set; }
    #endregion
    [Header("Ñ£ÔÎ")]
    public float stundDuration;
    public bool isStund;
    public Vector2 stundDir;


    public override void Awake()
    {
        base.Awake();
        idleState = new SkeletonIdleState(this, stateMachine, "Idle", this);
        moveState = new SkeletonMoveState(this, stateMachine, "Move", this);
        attackState = new SkeletonAttackState(this, stateMachine, "Attack", this);
        stundState = new SkeletonStundState(this, stateMachine, "Stund", this);
        stateMachine= new EnemyStateMachine();
        dieState = new SkeletonDieState(this, stateMachine, "Die", this);
    }
    public override void Start()
    {
        base.Start();
        currentSpeed = beginSpeed;
        stateMachine.Initialize(idleState);
        
    }
    public override void Update()
    {
        if (isDead)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        CanAttack();
        base.Update();
        stateMachine.currentState.Update();
        if (stateMachine.currentState == moveState)
        {
            Move();
        }
        CollisionCheck();
        //µ÷ÊÔÓÃ
        //if (Input.GetKeyUp(KeyCode.Mouse0))
        //{
        //    stateMachine.ChangeState(stundState);
        //}


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
                //Debug.Log("skeleton accelerate");
            }
            else if (playerCheck.distance < 1)
            {
                SetZeroVelocity();
                //Debug.Log("skeleton stop");

                if (stateMachine.currentState != attackState)
                {
                    if (canAttack)
                    {
                        stateMachine.ChangeState(attackState);
                        lastAttackTime = Time.time;
                    }
                    //else
                    //{
                    //    stateMachine.ChangeState(idleState);
                    //}
                  
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
    public override bool CanStun()
    {
        if (base.CanStun())
        {
            Debug.Log("skeleton begin stun");
            stateMachine.ChangeState(stundState);
            return true;
        }else
        {
            return false;
        }
    }
    public override void OnDie()
    {
        base.OnDie();

        stateMachine.ChangeState(dieState);
    }

    
}
