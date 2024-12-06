using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SlimeType
{
    Small,
    Medium,
    Large
}
public class Slime : Enemy
{
    public EnemyStateMachine stateMachine;
    public SlimeStunnedState stundState;
    public SlimeIdleState idleState;
    public SlimeAttackState attackState;
    public SlimeMoveState moveState;
    public SlimeDied dieState;
    [Header("Create Slime")]
    public SlimeType slimeType;
    public GameObject smallSlime;
  
    public bool isBattle;
    private CapsuleCollider2D coll;
    public override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        stundState = new SlimeStunnedState(this, stateMachine, "Stunned",this);
        idleState = new SlimeIdleState(this, stateMachine, "Idle",this);
        attackState = new SlimeAttackState(this, stateMachine, "Attack",this);
        moveState = new SlimeMoveState(this, stateMachine, "Move",this);
        dieState = new SlimeDied(this, stateMachine, "Die",this);
        
        coll = GetComponent<CapsuleCollider2D>();
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
        //调试用
        //if (Input.GetKeyUp(KeyCode.Mouse0))
        //{
        //    stateMachine.ChangeState(stundState);
        //}

        
            FindPlayerAccelerate();
        if (isVulnerable && coll.enabled)
        {
            coll.enabled = false;
            //固定在地面上
            rb.gravityScale = 0;
            Debug.Log("slime collider off");
        }
        else if (!isVulnerable &&!coll.enabled)
        {
            rb.gravityScale = 1;
            coll.enabled = true;
        }
        
        
        if (isBattle)
        {
            //获取一定范围
            Collider2D coll = Physics2D.OverlapCircle(transform.position, 3f, playerLayer);
            if (coll != null)
            {
                //如果玩家走到敌人身后
                if (PlayerManager.instance.player.transform.position.x - transform.position.x > 0 && faceDir == -1 || PlayerManager.instance.player.transform.position.x - transform.position.x < 0 && faceDir == 1)
                {
                    //翻转敌人
                    Flip();
                }
            }else
            {
                isBattle = false;
            }
           
        }
       

    }
    private void FindPlayerAccelerate()
    {
        if (isFrozen||Stunned)
        {
            return;
        }
        if (playerCheck.collider != null)
        {
            isBattle = true;
            if (playerCheck.distance > 1)
            {
                if (currentSpeed < maxSpeed)
                {
                    currentSpeed += Time.deltaTime * 0.8f;
                    //Debug.Log(currentSpeed);

                }
                //Debug.Log("skeleton accelerate");
            }
            else if (playerCheck.distance <0.8f)
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
            
            stateMachine.ChangeState(stundState);
            return true;
        }
        else
        {
            return false;
        }
    }
    public override void OnDie()
    {
        base.OnDie();

        stateMachine.ChangeState(dieState);
       if(slimeType == SlimeType.Small)
        {
            return;
        }
       CreateSlime(2,smallSlime);
        

    }
    public void CreateSlime(int amount,GameObject slime)
    {
        for (int i = 0; i < amount; i++)
        {
            var newSlime = Instantiate(slime, transform.position,transform.rotation);
            var faceDir = Random.Range(-1, 2);
            newSlime.GetComponent<Slime>().SetUp(faceDir);
           
           
           

        }
    }
    public void SetUp(int _faceDir)
    {
        float xVelocity = Random.Range(-3, 5);
        float yVelocity = Random.Range(3, 5);
        isKoncked=true;
        GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity, yVelocity);
        Invoke("CancelKonck", 0.5f);
    }
    private void CancelKonck()=>isKoncked = false;
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        //Gizmos.DrawWireSphere(transform.position, 5f);
    }



}
