using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : EnemyEntity
{
    public EnemyStateMachine stateMachine;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float beginSpeed;
    [Header("状态")]
    public bool isMove;
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
        base.Update();
        SetVelocity(new Vector2(currentSpeed*faceDir, rb.velocity.y));
        CollisionCheck();
        
        
        if (rb.velocity.x!=0&&!isMove)
        {
            
            stateMachine.ChangeState(moveState);
            Debug.Log("Move");
        }
        
        if (playerCheck.collider != null)
        {
            if (playerCheck.distance > 1)
            {
                while (currentSpeed < maxSpeed)
                {
                    currentSpeed += Time.deltaTime*0.8f;

                }
            }
            else if(playerCheck.distance < 1)
            {
                rb.velocity = Vector2.zero;
                stateMachine.ChangeState(attackState);
            }
        }
        else
        {
            while (currentSpeed > beginSpeed)
            {
                currentSpeed -= Time.deltaTime;
            }
            stateMachine.ChangeState(moveState);
        }
        


    }

}
