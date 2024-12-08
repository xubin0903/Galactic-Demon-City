using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bringer : Enemy
{
    #region 状态机
    public EnemyStateMachine stateMachine;
    public BringerIdleState idleState;
    public BringerMoveState moveState;
    public BringerAttackState attackState;
    public BringerTeleportState teleportState;
    public BringerSpellCastState spellCastState;
    public BringerDieState dieState;
    #endregion
    public bool isBattle;
    private CapsuleCollider2D coll;
    public int teleportChance;
    private int defaultTeleportChance;
    public float spellCastCooldown;
    private float spellLastTime;
    [Header("Teleport Details")]
    [SerializeField] private BoxCollider2D arena;
    [SerializeField] private Vector2 surroundingCheckSize;
    [Header("Spell Details")]
    public GameObject spellPrefab;
    [SerializeField] public int spellAmout;
    [SerializeField] public float spellCoolDown;
    public GameObject healthUI;
    

    public override void Awake()
    {
        base.Awake();
        coll = GetComponent<CapsuleCollider2D>();
        stateMachine = new EnemyStateMachine();
        idleState = new BringerIdleState(this,stateMachine,"Idle",this);
        moveState = new BringerMoveState(this,stateMachine,"Move",this);
        attackState = new BringerAttackState(this,stateMachine,"Attack",this);
        teleportState = new BringerTeleportState(this, stateMachine, "Teleport", this);
        spellCastState = new BringerSpellCastState(this, stateMachine, "SpellCast", this);
        dieState = new BringerDieState(this, stateMachine, "Die", this);
       
    }
    public override void Start()
    {
        base.Start();
        currentSpeed = beginSpeed;
        stateMachine.Initialize(idleState);
        defaultTeleportChance = teleportChance;
        spellLastTime = -spellLastTime;
        healthUI.SetActive(true);

    }
    public void SetDefalutTeleportChance()
    {
        teleportChance = defaultTeleportChance;
    }
    public override void Update()
    {
        if (isDead)
        {
            
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
       

        FindPlayerAccelerate();
        if (isVulnerable && coll.enabled)
        {
            coll.enabled = false;
            //固定在地面上
            rb.gravityScale = 0;

        }
        else if (!isVulnerable && !coll.enabled)
        {
            rb.gravityScale = 1;
            coll.enabled = true;
        }


        if (isBattle)
        {
            //获取一定范围
            Collider2D coll = Physics2D.OverlapCircle(transform.position, 6f, playerLayer);
            if (coll != null)
            {
                //如果玩家走到敌人身后
                if (PlayerManager.instance.player.transform.position.x - transform.position.x > 0 && faceDir == -1 && PlayerManager.instance.player.isGrounded || PlayerManager.instance.player.transform.position.x - transform.position.x < 0 && faceDir == 1 && PlayerManager.instance.player.isGrounded)
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
    public void CastSpell()
    {
        Debug.Log("Cast Spell");
        Player player = PlayerManager.instance.player;
        Vector3 spellPosition;
        if (player.rb.velocity.x == 0)
        {
            spellPosition=new Vector3(player.transform.position.x , player.transform.position.y + 1.5f);
        }else if(player.rb.velocity.x < 5)
        {
            spellPosition = new Vector3(player.transform.position.x +player.faceDir* 1.5f, player.transform.position.y+1.5f);
        }
        else
        {
           spellPosition = new Vector3(player.transform.position.x + player.faceDir * 3f, player.transform.position.y + 1.5f);
        }
        GameObject newSpell = Instantiate(spellPrefab, spellPosition, Quaternion.identity);
        newSpell.GetComponent<Bringer_Spell_Controller>().SetUp(stats);
    }
    public bool CanSpellCast()
    {
        if (Time.time >= spellLastTime + spellCastCooldown)
        {
            stateMachine.ChangeState(spellCastState);
            spellLastTime = Time.time;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanTeleport()
    {
        if (Random.Range(0, 100) >teleportChance)
        {
            return false;
        }
        else
        {
           
            SetDefalutTeleportChance();
            return true;
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
            if (playerCheck.distance > 4f)
            {
                if (currentSpeed <= maxSpeed)
                {
                    currentSpeed = maxSpeed;
                    if(stateMachine.currentState != spellCastState && stateMachine.currentState != moveState && stateMachine.currentState != attackState && stateMachine.currentState != teleportState)
                    {

                        stateMachine.ChangeState(moveState);
                        Debug.Log("Accelerate");
                    }

                   

                }
               
            }
            else if (playerCheck.distance < 2f)
            {
                SetZeroVelocity();
            

                if (stateMachine.currentState != attackState&&stateMachine.currentState!=spellCastState&&stateMachine.currentState!=teleportState)
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
   
    public override void OnDie()
    {
        base.OnDie();

        stateMachine.ChangeState(dieState);
        healthUI.SetActive(false);
        UI.instance.ShowPop("恭喜你通关了游戏");
       

    }
    public void FindPosition()
    {
        float x=Random.Range(arena.bounds.min.x+3,arena.bounds.max.x-3);
        float y=Random.Range(arena.bounds.min.y+3,arena.bounds.max.y-3);
        transform.position = new Vector3(x, y, transform.position.z);
        transform.position=new Vector3(transform.position.x,transform.position.y-GroundBelow().distance+cd.size.y/2,transform.position.z);
        if (SomrthingIsAround() || !GroundBelow())
        {
            Debug.Log("Can't find position");
            FindPosition();
        }

    }
    private RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 100, groundLayer);
    private bool SomrthingIsAround() => Physics2D.Raycast(transform.position, surroundingCheckSize, 0, groundLayer);
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - GroundBelow().distance, transform.position.z));
        Gizmos.DrawWireCube(transform.position, surroundingCheckSize);
    }
    public override void Damage(Player player)
    {
        base.Damage(player);
        if(!isBattle)
        isBattle=true;
    }
}
