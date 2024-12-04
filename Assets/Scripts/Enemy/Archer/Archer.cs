using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Enemy
{

    #region ״̬��
    public ArcherAttackState attackState;
    public ArcherIdle idleState;
    public ArcherMoveState moveState;
    public ArcherDieState dieState;
    public ArcherJumpState jumpState;
    public EnemyStateMachine stateMachine;


    #endregion
    public bool isBattle;
    private CapsuleCollider2D coll;
    public override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
       
        idleState = new ArcherIdle(this, stateMachine, "Idle", this);
        attackState = new ArcherAttackState(this, stateMachine, "Attack", this);
        moveState = new ArcherMoveState(this, stateMachine, "Move", this);
        dieState = new ArcherDieState(this, stateMachine, "Die", this);

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
        //������
        //if (Input.GetKeyUp(KeyCode.Mouse0))
        //{
        //    stateMachine.ChangeState(stundState);
        //}


        FindPlayerAccelerate();
        if (isVulnerable && coll.enabled)
        {
            coll.enabled = false;
            //�̶��ڵ�����
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
            //��ȡһ����Χ
            Collider2D coll = Physics2D.OverlapCircle(transform.position, 7f, playerLayer);
            if (coll != null)
            {
                //�������ߵ��������
                if (PlayerManager.instance.player.transform.position.x - transform.position.x > 0 && faceDir == -1&&PlayerManager.instance.player.isGrounded|| PlayerManager.instance.player.transform.position.x - transform.position.x < 0 && faceDir == 1&&PlayerManager.instance.player.isGrounded)
                {
                    //��ת����
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
            if (playerCheck.distance > 5)
            {
                if (currentSpeed < maxSpeed)
                {
                    currentSpeed += Time.deltaTime * 0.8f;
                    //Debug.Log(currentSpeed);

                }
                //Debug.Log("skeleton accelerate");
            }
            else if (playerCheck.distance <= 5f&&playerCheck.distance>2f)
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
            else
            {
                //TODO:����̫����ֹͣ���������
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
        base.OnDie();

        stateMachine.ChangeState(dieState);
       

    }
}

