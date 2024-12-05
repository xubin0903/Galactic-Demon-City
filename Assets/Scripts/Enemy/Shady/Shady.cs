using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shady : Enemy
{
    #region ×´Ì¬»ú
    public ShadyIdleState idleState;
    public ShadyMoveState moveState;
    public ShadyMoveFastState moveFastState;
    public ShadyStunnedState stunnedState;
    public EnemyStateMachine stateMachine;
    public ShadyDieState dieState;
    public ShadyExplodeState explodeState;
   
    #endregion
    public GameObject explosionPrefab;
    [SerializeField] private float growSpeed;
    [SerializeField] private float maxSize;
   
    public override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        idleState = new ShadyIdleState(this,stateMachine,"Idle",this);
        moveState = new ShadyMoveState(this,stateMachine,"Move",this);
        moveFastState = new ShadyMoveFastState(this,stateMachine,"MoveFast",this);
        stunnedState = new ShadyStunnedState(this,stateMachine,"Stunned",this);
        dieState = new ShadyDieState(this,stateMachine,"Die",this);
        explodeState = new ShadyExplodeState(this,stateMachine,"Explode",this);
        
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
           
            return;
        }
        CanAttack();
        base.Update();
        stateMachine.currentState.Update();
        if (stateMachine.currentState == moveState|| stateMachine.currentState == moveFastState)
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
                currentSpeed=maxSpeed;
                stateMachine.ChangeState(moveFastState);
                
            }
            else if (playerCheck.distance < 0.8f)
            {
               
                stateMachine.ChangeState(explodeState);
                isDead = true;
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
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        else
        {
            return false;
        }
    }
    public override void OnDie()
    {
        stateMachine.ChangeState(dieState);
        Destroy(gameObject, 7f);
        CloseCounterWindow();
        base.OnDie();

    }
    public void Explode()
    {
        GameObject explosion = Instantiate(explosionPrefab, attackCheck.position, Quaternion.identity);
        explosion.GetComponent<ShadyExplodeController>().SetExplode(stats, growSpeed, maxSize, attackRadius);
    }

}
