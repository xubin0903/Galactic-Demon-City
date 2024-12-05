using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Enemy
{

    #region 状态机
    public ArcherAttackState attackState;
    public ArcherIdle idleState;
    public ArcherMoveState moveState;
    public ArcherDieState dieState;
    public ArcherJumpState jumpState;
    public EnemyStateMachine stateMachine;


    #endregion

    public bool isBattle;
    private CapsuleCollider2D coll;
    [Header("Special InFo")]
    public Vector2 jumpBackOPos;
    [SerializeField] private float jumpcoolDown;
    [SerializeField] private float lastJumpTime;
    public GameObject arrowPrefab;
    public float safeDistance;
    public float attackDistance;
    public Transform groundBehindCheck;

    public override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
       
        idleState = new ArcherIdle(this, stateMachine, "Idle", this);
        attackState = new ArcherAttackState(this, stateMachine, "Attack", this);
        moveState = new ArcherMoveState(this, stateMachine, "Move", this);
        dieState = new ArcherDieState(this, stateMachine, "Die", this);
        jumpState = new ArcherJumpState(this, stateMachine, "Jump", this);

        coll = GetComponent<CapsuleCollider2D>();
    }
    public override void Start()
    {
        base.Start();
        lastJumpTime=-jumpcoolDown;
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
        
        Debug.Log(stateMachine.currentState.animName);

        FindPlayerAccelerate();
        if (isVulnerable && coll.enabled)
        {
            coll.enabled = false;
            //固定在地面上
            rb.gravityScale = 0;
            Debug.Log("slime collider off");
        }
        else if (!isVulnerable && !coll.enabled)
        {
            rb.gravityScale = 1;
            coll.enabled = true;
        }


        if (isBattle)
        {
            //获取一定范围
            Collider2D coll = Physics2D.OverlapCircle(transform.position, 7f, playerLayer);
            if (coll != null)
            {
                //如果玩家走到敌人身后
                if (PlayerManager.instance.player.transform.position.x - transform.position.x > 0 && faceDir == -1&&PlayerManager.instance.player.isGrounded|| PlayerManager.instance.player.transform.position.x - transform.position.x < 0 && faceDir == 1&&PlayerManager.instance.player.isGrounded)
                {
                    //翻转敌人
                    Flip();
                }
            }
            else
            {
                isBattle = false;
            }

        }


    }
    private void FindPlayerAccelerate()
    {
        if (isFrozen || Stunned)
        {
            return;
        }
        if (playerCheck.collider != null)
        {
            isBattle = true;
            if (playerCheck.distance > attackDistance)
            {
                if (currentSpeed < maxSpeed)
                {
                    currentSpeed += Time.deltaTime * 0.8f;
                   

                }
                
            }
            else if (playerCheck.distance <= attackDistance&&playerCheck.distance>safeDistance)
            {
               

                if (stateMachine.currentState != attackState)
                {
                    if (canAttack)
                    {
                        Debug.Log("Archer attack");
                        stateMachine.ChangeState(attackState);
                        lastAttackTime = Time.time;
                    }
                    


                }
                

            }
            else
            {
                if (CanJump())
                {
                    if (stateMachine.currentState != jumpState)
                    {
                        stateMachine.ChangeState(jumpState);
                        Debug.Log("Archer Jump");
                    }
                }
                    
            }
        }
        else
        {
            if (currentSpeed > beginSpeed)
            {
                currentSpeed -= Time.deltaTime ;
               
            }
            else
            {
                currentSpeed = beginSpeed;
            }

        }
    }

    public RaycastHit2D GroundBehind()=>Physics2D.BoxCast(groundBehindCheck.position, new Vector2(1, 1), 0, Vector2.down, 0f, groundLayer);
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawCube(groundBehindCheck.position, new Vector3(1, 1, 0f));
        
    }

    public bool CanJump()
    {
        if (GroundBehind().collider == null)
        {
            return false;
        }
        if (Time.time>lastJumpTime+jumpcoolDown)
        {
            lastJumpTime = Time.time;
            return true;
        }
        else
        {
            return false;
        }
    }
   
    public void AttackFinish()
    {
        stateMachine.currentState.Trigger();
    }
    //public override bool CanStun()
    //{
    //    if (base.CanStun())
    //    {

    //        stateMachine.ChangeState(stundState);
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
    public override void OnDie()
    {
        stateMachine.ChangeState(dieState);
        base.OnDie();

       

    }
}

