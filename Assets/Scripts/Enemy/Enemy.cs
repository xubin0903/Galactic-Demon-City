using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : EnemyEntity
{
    public EnemyStateMachine stateMachine;
    [SerializeField] public float currentSpeed;
    [SerializeField] public float maxSpeed;
    [SerializeField] public float beginSpeed;
    [SerializeField] public float lastAttackTime;
    [SerializeField] private float attckCoolDown;
    public float idleTimer;
    [Header("状态")]
    public bool isMove;
    public bool canAttack=true;
    [SerializeField] private float stateTimer;
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }
    public override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        moveState = new SkeletonMoveState(this, stateMachine,"Move");
        idleState = new SkeletonIdleState(this, stateMachine,"Idle");
        attackState = new SkeletonAttackState(this, stateMachine,"Attack");
        
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
                    Debug.Log(currentSpeed);

                }
            }
            else if (playerCheck.distance < 1)
            {
                rb.velocity = Vector2.zero;
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
    public  void Move()
    {
        SetVelocity(new Vector2(currentSpeed * faceDir, rb.velocity.y));
    }
    public void CanAttack()
    {
        if (Time.time > lastAttackTime + attckCoolDown)
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }
    }
    public void AttackFinish()
    {
        stateMachine.currentState.Trigger();
    }
}
