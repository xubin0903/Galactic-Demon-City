using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player :PublicCharacter
{
    [Header("基本属性")]
    [SerializeField] private float offsety;
    [SerializeField] private float beginspeed;
    [SerializeField] private float maxspeed;
    private float defaultmaxspeed;
    [SerializeField] private float acceleration = 1;
    private float defaultAcceleration;
    [SerializeField] private float jumpForce;
    private float defaultJumpForce;
    [SerializeField] private float currentJumpForce;
    [SerializeField] private float xInput;
    [SerializeField] private bool isDead;
    public EntityFX fx { get; private set; }
    public Animator animator { get; private set; }
    
    
    [Header("冲刺")]
    [SerializeField] private float dashSpeed;
    private float defaultdashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashDuration;
    [SerializeField] private bool isDash;
    
    
    [SerializeField] private float wallDashFairDir;
    [Header("攻击")]
    [SerializeField] private float attackTimer;
    [SerializeField] private float attackDuration;
    [SerializeField] private bool isAttack;
    [SerializeField] public int comobatCount { get; private set; }
    [Header("攻击细节")]
    [SerializeField] private float[] attackMovex;
    [SerializeField] private float[] attackMovey;
    [SerializeField] private float attackTime;
    [SerializeField] private float attackDir;

    [Header("滑铲")]
    [SerializeField] private float slidingSpeed;
    private float defaultslidingSpeed;
    [SerializeField] private float slideDuration;
    [SerializeField] private bool isSlide;
    [SerializeField] private float slidingCooldown;
    [SerializeField] private float slidingCooldownTimer;
    [Header("滑墙")]
    [SerializeField] private bool isSlideWall;
    [SerializeField] private bool isWall;
    [SerializeField] private float wallCheckDistance;
    [Header("受击反馈")]
    [SerializeField] private Vector2 HurtBackDir;
    [SerializeField] private float backDuration;
    [SerializeField] private bool isKoncked;
    [Header("反击")]
    [SerializeField] private bool isCounterAttack;
    [SerializeField] public bool isSuccessfulCounterAttack;
    [SerializeField] private float counterAttackDuration;



    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerBaseState baseState { get; private set; }
    public PlayerAimSwordState aimSword { get; private set; }
    public PlayerCatchSwordState catchSword { get; private set; }
    public PlayerThrowSwordState throwSword { get; private set; }
    public PlayerBlackHoleState blackhole { get; private set; }
    public CharacterStats stats { get; private set; }
    public PlayerDieState die { get; private set; }
    [Header("人物基本属性")]
   
    


    [Header("Sword")]
    public GameObject sword;
    [Header("Black Hole")]
    public bool isBlackHole;
    public bool canBlackHole;


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
        blackhole = new PlayerBlackHoleState(this, stateMachine,"isGrounded");
        stats = GetComponent<CharacterStats>();
        die = new PlayerDieState(this, stateMachine,"Die");

    }
    protected override void Start()
    {
       stateMachine.Initialize(baseState);
        //初始化基本属性
        defaultmaxspeed = maxspeed;
        defaultAcceleration = acceleration;
        defaultJumpForce = jumpForce;
        defaultdashSpeed = dashSpeed;
        defaultslidingSpeed = slidingSpeed;

    }
    protected override void Update()
    {
        //人物死亡不进行任何输入
        if (isDead)
        {
            return;
        }
      
        
        stateMachine.currentState.Update();
        #region 基本状态检测
        faceDir = transform.localScale.x>0?1:-1;

        
        //检测输入
        CheckInput();
        //速度限制
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
        //冲刺
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
        ////冲刺冷却
        //if (!isDashable)
        //{
        //    dashCooldownTimer -= Time.deltaTime;
        //    if (dashCooldownTimer<=0)
        //    {
        //        isDashable = true;
        //    }
        //}
        //攻击间隔
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
        //滑铲冷却
        if (slidingCooldownTimer > 0)
        {
            slidingCooldownTimer -= Time.deltaTime;
               
        }
        //滑墙
        if (!isGrounded && isWall)
        {
            isSlideWall = true;
            //降低掉落速度
            if (rb.velocity.y < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                if (Input.GetKey(KeyCode.S))
                {
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 2);
                }
            //蹬墙跳
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
        //攻击移动
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
        #region 技能剑的相关控制
        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1)&&(sword!=null||!isAttack&&!isDash&&!isSlideWall&&!isSlide&&!isMove))
            {
                //Debug.Log("鼠标左键按下");
                if(!HaveSword())//剑没回来不能继续攻击
                
                stateMachine.ChangeState(aimSword);
            }
        }


        #endregion
        #region 黑洞技能相关控制
        if (  isGrounded&&!isKoncked && !isDash && !isAttack && !isSlideWall && !isSlide && !isMove)
        {
            if (Input.GetKeyDown(KeyCode.R)&&canBlackHole)
            {
                isBlackHole = true;
               stateMachine.ChangeState(blackhole);
            }
        }
        #endregion
        #region 水晶技能相关控制
        if ( !isKoncked &&  !isAttack && !isSlideWall && !isSlide&&!isBlackHole )
        {
            if (Input.GetKeyDown(KeyCode.F) && SkillManager.instance.crystal.CanUseSkill())
            {
                Debug.Log("水晶技能");
            }
        }
        #endregion
       
    }

    private void CheckInput()
    {
        //移动
        isMove = Input.GetButtonDown("Horizontal") || Input.GetButton("Horizontal");
        if (isMove == true && !isDash && !isAttack&&!isKoncked&&stateMachine.currentState!=aimSword&&!isBlackHole)
        {
            Move();
            CounterAttackOver();
        }
        //跳跃
        if (Input.GetButtonDown("Jump")&&isGrounded&&!isKoncked&&!isDash&&!isBlackHole)
        {
            currentJumpForce = jumpForce;
            Jump();
            CounterAttackOver();

        }
        //冲刺
        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash_skill.CanUseSkill()&&!isSlideWall&&!isKoncked&&!isBlackHole)
        {
            Dash();
            CounterAttackOver();
        }
        //攻击
        if (Input.GetKeyDown(KeyCode.Mouse0)&&isGrounded&&!isKoncked&&!isDash&&!isBlackHole)
        {
            Attack();
        }
        //滑铲
        if (Input.GetKeyDown(KeyCode.C)&&isGrounded&&isMove&&!isKoncked&&!isDash&&!isBlackHole)
        {
            Slide();
            CounterAttackOver();

        }
        //反击
        if (Input.GetKeyDown(KeyCode.E)&&isGrounded&&!isKoncked&&!isAttack&&!isDash&&!isBlackHole)
        {
            CounterAttack();
        }
        //使用恢复药品
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            if (Inventory.instance.CanUseFlask())
            {
                Debug.Log("使用恢复药品");
            }
            else
            {
                Debug.Log("药品不足");
            }
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
        //启动协程
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
        
        //移动动画
        if (xInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);//反转
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
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(faceDir * wallCheckDistance, 0, 0)); // 画出射线
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
        SkillManager.instance.clone.CreateCloneOnDashSatrt(transform, new Vector3(0, 0, 0));
        
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
        if (comobatCount == 0)
        {
            attackRadius = 0.85f;
        }
        else if (comobatCount == 1)
        {
            attackRadius = 0.5f;
        }
        else if (comobatCount == 2)
        {
            attackRadius = 1.35f;

        }
        else
        {
            attackRadius = 1f;
           
        }
      
    }
    private void AttackMove()
    {
        //攻击移动
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
        //进行滑铲冷却
        slidingCooldownTimer = slidingCooldown; 
    }

    public void AttackOver()
    {

        isAttack = false;
        comobatCount++;
     
        if (comobatCount >= 3)
        {
            comobatCount = 0;
        }
        
        //Debug.Log("攻击段数："+comobatCount);
        
        
    }
    public void Damage(Enemy enemy)
    {
        //Debug.Log(gameObject.name + "受到伤害");
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
    #region 检查并储存剑
    public void CheckSword(GameObject _sword)
    {
        if (sword == null)
        {
            sword=_sword;
        }
        else
        {
            return;
        }
    }
    private bool HaveSword()
    {
        if (sword == null)
        {
            return false;
        }
        else
        {
            if (sword.GetComponent<Sword_Skill_Controller>().canReturn)
            {
                sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
                

            }
            return true;
        }
    }
    public void CatchSword()
    {
        if(!isAttack&&!isDash&&!isSlideWall&&!isSlide&&!isMove)
        stateMachine.ChangeState(catchSword);
        Destroy(sword);
    }
    #endregion
    public void ExitBlackHole()
    {
        stateMachine.ChangeState(baseState);
        fx.TransParent(false);
        isBlackHole = false;
    }
    public void OnDie()
    {
        stateMachine.ChangeState(die);
       
        
    }
    public void GameOver()
    {
        //Time.timeScale = 0;
        isDead = true;
        ////所有状态设置为死亡状态
        //isAttack = false;
        //isDash = false;
        //isSlide = false;
        //isSlideWall = false;
        //isCounterAttack = false;
        //isSuccessfulCounterAttack = false;
        //isKoncked = false;
        Debug.Log("Game Over");
    }
    public void IcedSlowEffect(float duration,float slowPercent)
    {
        currentspeed*=(1- slowPercent);
        maxspeed*= (1- slowPercent);
        currentJumpForce*= (1- slowPercent);
        jumpForce*= (1- slowPercent);
        animator.speed = 1- slowPercent;
        slidingSpeed*= (1- slowPercent);
        dashSpeed*= (1- slowPercent);
        
        acceleration*= (1- slowPercent);
       
        //Debug.Log("冰冻效果");
        StartCoroutine(IcedSlowEffcetEnd(duration));
    
    }
    public IEnumerator IcedSlowEffcetEnd(float duration)
    {
        yield return new WaitForSeconds(duration);
        currentspeed = beginspeed;
        currentJumpForce = defaultJumpForce;
        jumpForce = defaultJumpForce;
        slidingSpeed = defaultslidingSpeed;
        dashSpeed = defaultdashSpeed;
     
        acceleration = defaultAcceleration;
        animator.speed = 1;
        maxspeed = defaultmaxspeed;
      
        //Debug.Log("冰冻效果结束");
    }

}
