using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player :PublicCharacter
{
    [Header("��������")]
    [SerializeField] private float offsety;
    [SerializeField] private float beginspeed;
    [SerializeField] private float maxspeed;
    [SerializeField] private float acceleration = 1;
    [SerializeField] private float jumpForce;
    [SerializeField] private float currentJumpForce;
    [SerializeField] private float xInput;
    private EntityFX fx;
    public Animator animator { get; private set; }
    
    
    [Header("���")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashDuration;
    [SerializeField] private bool isDash;
    
    
    [SerializeField] private float wallDashFairDir;
    [Header("����")]
    [SerializeField] private float attackTimer;
    [SerializeField] private float attackDuration;
    [SerializeField] private bool isAttack;
    [SerializeField] public int comobatCount { get; private set; }
    [Header("����ϸ��")]
    [SerializeField] private float[] attackMovex;
    [SerializeField] private float[] attackMovey;
    [SerializeField] private float attackTime;
    [SerializeField] private float attackDir;

    [Header("����")]
    [SerializeField] private float slidingSpeed;
    [SerializeField] private float slideDuration;
    [SerializeField] private bool isSlide;
    [SerializeField] private float slidingCooldown;
    [SerializeField] private float slidingCooldownTimer;
    [Header("��ǽ")]
    [SerializeField] private bool isSlideWall;
    [SerializeField] private bool isWall;
    [SerializeField] private float wallCheckDistance;
    [Header("�ܻ�����")]
    [SerializeField] private Vector2 HurtBackDir;
    [SerializeField] private float backDuration;
    [SerializeField] private bool isKoncked;
    [Header("����")]
    [SerializeField] private bool isCounterAttack;
    [SerializeField] public bool isSuccessfulCounterAttack;
    [SerializeField] private float counterAttackDuration;



    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerBaseState baseState { get; private set; }
    public PlayerAimSwordState aimSword { get; private set; }
    public PlayerCatchSwordState catchSword { get; private set; }
    public PlayerThrowSwordState throwSword { get; private set; }
    
    public PlayerState currentPlayerState;



    protected override void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        fx = GetComponent<EntityFX>();
        stateMachine = new PlayerStateMachine();
        baseState = new PlayerBaseState(this, stateMachine,"BaseState");
        aimSword = new PlayerAimSwordState(this, stateMachine,"AimSword");
        catchSword = new PlayerCatchSwordState(this, stateMachine,"CatchSword");
        throwSword = new PlayerThrowSwordState(this, stateMachine,"ThrowSword");


    }
    protected override void Start()
    {
       stateMachine.Initialize(baseState);
    }
    protected override void Update()
    {
        stateMachine.currentState.Update();
        #region ����״̬���
        faceDir = transform.localScale.x>0?1:-1;

        
        //�������
        CheckInput();
        //�ٶ�����
        isMove = Input.GetButtonDown("Horizontal") || Input.GetButton("Horizontal");
        
        if (isMove == true && currentspeed < maxspeed)
        {
            currentspeed += Time.deltaTime * acceleration;
            acceleration += Time.deltaTime * 1.5f;


        }
        if (isMove == false&&!isKoncked)
        {
            currentspeed = beginspeed;
            rb.velocity = new Vector2(0, rb.velocity.y);
            acceleration = 1;
            animator.SetBool("isMove", false);
        }
        CollisionCheck();
        AnimationControl();
        //���
        if (dashTime>0)
        {
            dashTime -= Time.deltaTime;
            currentspeed = dashSpeed;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            if (dashTime<=0)
            {
                currentspeed = beginspeed;
                isDash = false;
            }
        }
        ////�����ȴ
        //if (!isDashable)
        //{
        //    dashCooldownTimer -= Time.deltaTime;
        //    if (dashCooldownTimer<=0)
        //    {
        //        isDashable = true;
        //    }
        //}
        //�������
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {

                comobatCount = 0;
            }
        }
        if (isDash)
        {
            DashMent();
        }
        //������ȴ
        if (slidingCooldownTimer > 0)
        {
            slidingCooldownTimer -= Time.deltaTime;
               
        }
        //��ǽ
        if (!isGrounded && isWall)
        {
            isSlideWall = true;
            //���͵����ٶ�
            if (rb.velocity.y < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                if (Input.GetKey(KeyCode.S))
                {
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 2);
                }
            //��ǽ��
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    currentJumpForce = jumpForce*1.2f ;
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    Jump();
                }
               
            }

        }
        else
        {
            isSlideWall = false;
        }
        //�����ƶ�
        if (isAttack)
        {
            if (attackTime > 0)
            {
                attackTime -= Time.deltaTime;
                AttackMove();
            }

        }
        //if (!isAttack && !isDash && !isSlideWall && !isSlide&&!isMove)
        //{
        //    if (rb.velocity.y == 0)
        //    {
        //        isGrounded = true;
        //        animator.SetBool("isGrounded", true);

        //    }
        //}
        #endregion
        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Debug.Log("����������");
                stateMachine.ChangeState(aimSword);
            }
        }
    }
    private void CheckInput()
    {
        //�ƶ�
        isMove = Input.GetButtonDown("Horizontal") || Input.GetButton("Horizontal");
        if (isMove == true && !isDash && !isAttack&&!isKoncked)
        {
            Move();
            CounterAttackOver();
        }
        //��Ծ
        if (Input.GetButtonDown("Jump")&&isGrounded&&!isKoncked&&!isDash)
        {
            currentJumpForce = jumpForce;
            Jump();
            CounterAttackOver();

        }
        //���
        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash_skill.CanUseSkill()&&!isSlideWall&&!isKoncked)
        {
            Dash();
            CounterAttackOver();
        }
        //����
        if (Input.GetKeyDown(KeyCode.Mouse0)&&isGrounded&&!isKoncked&&!isDash)
        {
            Attack();
        }
        //����
        if (Input.GetKeyDown(KeyCode.C)&&isGrounded&&isMove&&!isKoncked&&!isDash)
        {
            Slide();
            CounterAttackOver();

        }
        //����
        if (Input.GetKeyDown(KeyCode.E)&&isGrounded&&!isKoncked&&!isAttack&&!isDash)
        {
            CounterAttack();
        }
    }
    private void CounterAttack()
    {
        if (isCounterAttack)
        {
            return;
        }
        isCounterAttack = true;
        isSuccessfulCounterAttack = false;
        StartCoroutine(CounterAttackStart(counterAttackDuration));
    }
    public void CounterAttackOver()
    {
        isCounterAttack = false;
        
    }
    public void SuccessfulCounterAttackOver()
    {
        isSuccessfulCounterAttack = false;
    }
    private IEnumerator CounterAttackStart(float counterAttackDuration)
    {


        yield return new WaitForSeconds(counterAttackDuration);
        CounterAttackOver();
    }
    private void Slide()
    {
        if (isSlide)
        {
            return;
        }
        if (slidingCooldownTimer > 0)
        {
            return;
        }
        isSlide = true;
        currentspeed=slidingSpeed;
        //����Э��
        StartCoroutine(SlideStart(slideDuration));
    }

    private IEnumerator SlideStart(float slideDuration)
    {
        yield return new WaitForSeconds(slideDuration);
        SlideOver();
    }

    private void Move()
    {
        
        xInput = Input.GetAxis("Horizontal");
        
        rb.velocity = new Vector2( xInput * currentspeed, rb.velocity.y);
        
        //�ƶ�����
        if (xInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);//��ת
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        
        
    }
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, currentJumpForce);
        isAttack = false;
    }
    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(faceDir * wallCheckDistance, 0, 0)); // ��������
        Gizmos.DrawLine(new Vector3(transform.position.x+groundCheckRadius*faceDir,transform.position.y-offsety,transform.position.z), transform.position + Vector3.down * groundCheckDistance+Vector3.right*groundCheckRadius*faceDir);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(attackCheck.position, attackRadius);
    }
    protected override void CollisionCheck()
    {
      isGrounded = Physics2D.Raycast(new Vector2(transform.position.x+groundCheckRadius*faceDir,transform.position.y), Vector2.down, groundCheckDistance, groundLayer);
      isWall = Physics2D.Raycast(transform.position, Vector2.right * faceDir, wallCheckDistance, groundLayer);
    }
   private void AnimationControl()
    {
        animator.SetBool("isMove", isMove);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetBool("isDash", isDash);
        animator.SetBool("isAttack", isAttack);
        animator.SetInteger("comobatCount", comobatCount);
        animator.SetBool("isSlide", isSlide);
        animator.SetBool("isSlideWall", isSlideWall);
        animator.SetBool("isCounterAttack", isCounterAttack);
        animator.SetBool("isSuccessfulCounterAttack", isSuccessfulCounterAttack);
    }   
    private void Dash()
    {
        dashTime=dashDuration;
        isDash = true;
        SkillManager.instance.clone.CreateClone(transform);
        
        if (isSlideWall)
        {
            wallDashFairDir = faceDir*-1;
        }
        
        
    }
    private void Attack()
    {
        isAttack = true;
        attackTimer = attackDuration;
        rb.velocity = new Vector2(0, rb.velocity.y);
        attackTime = 0.1f;
    }
    private void AttackMove()
    {
        //�����ƶ�
        attackDir = faceDir;
        if (xInput != 0)
        {
            attackDir = xInput;
        }
        rb.velocity = new Vector2(attackMovex[comobatCount]*attackDir, attackMovey[comobatCount]*0.8f);
    }
    private void DashMent()
    {
        
        rb.velocity = new Vector2(faceDir * currentspeed, 0);
        

    }
    public void SlideOver()
    {
        isSlide = false;
        currentspeed = beginspeed;
        //���л�����ȴ
        slidingCooldownTimer = slidingCooldown; 
    }

    public void AttackOver()
    {
        isAttack = false;
        comobatCount++;
        
        if (comobatCount == 0)
        {
            attackRadius = 0.85f;
        }else if (comobatCount == 1)
        {
            attackRadius = 0.5f;
        }else if (comobatCount == 2)
        {
            attackRadius = 1.35f;
        }
        else
        {
            attackRadius = 1f;
            comobatCount = 0;
        }
        
        
    }
    public void Damage(Enemy enemy)
    {
        Debug.Log(gameObject.name + "�ܵ��˺�");
        fx.Hurt();
        StartCoroutine(HurtBack(backDuration,enemy));
    }
    private IEnumerator HurtBack(float duration,Enemy enemy)
    {
        isKoncked = true;
        if (enemy.faceDir == faceDir)
        {
            rb.velocity = new Vector2(HurtBackDir.x * (faceDir), HurtBackDir.y);
        }
        else
        {
        rb.velocity = new Vector2(HurtBackDir.x*(-faceDir), HurtBackDir.y);

        }
        yield return new WaitForSeconds(duration);
        isKoncked = false;
    }

}
