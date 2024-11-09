using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player :PublicCharacter
{
    [Header("ª˘±æ Ù–‘")]
    [SerializeField] private float offsety;
    [SerializeField] private float beginspeed;
    [SerializeField] private float maxspeed;
    [SerializeField] private float acceleration = 1;
    [SerializeField] private float jumpForce;
    [SerializeField] private float currentJumpForce;
    [SerializeField] private float xInput;
    
    
    [Header("≥Â¥Ã")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashDuration;
    [SerializeField] private bool isDash;
    [SerializeField] private bool isDashable;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashCooldownTimer;
    [SerializeField] private float wallDashFairDir;
    [Header("π•ª˜")]
    [SerializeField] private float attackTimer;
    [SerializeField] private float attackDuration;
    [SerializeField] private bool isAttack;
    [SerializeField] private int comobatCount;
    [Header("ª¨≤˘")]
    [SerializeField] private float slidingSpeed;
    [SerializeField] private float slideDuration;
    [SerializeField] private bool isSlide;
    [SerializeField] private float slidingCooldown;
    [SerializeField] private float slidingCooldownTimer;
    [Header("ª¨«Ω")]
    [SerializeField] private bool isSlideWall;
    [SerializeField] private bool isWall;
    [SerializeField] private float wallCheckDistance;
    
   

    //public PlayerStateMachine stateMachine { get; private set; }
    //public PlayerIdleState idleState{ get; private set;}
    //public PlayerMoveState moveState { get; private set; }
    //public PlayerState currentPlayerState;



    protected override void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //stateMachine = new PlayerStateMachine();
        //idleState = new PlayerIdleState(this,stateMachine,"Idle");
        //moveState = new PlayerMoveState(this,stateMachine,"Move");
        
    }
    protected override void Start()
    {
       //stateMachine.Initialize(idleState);
    }
    protected override void Update()
    {
        //stateMachine.currentState.Update();
        
         faceDir=transform.localScale.x>0?1:-1;

        
        //ºÏ≤‚ ‰»Î
        CheckInput();
        //ÀŸ∂»œﬁ÷∆
        isMove = Input.GetButtonDown("Horizontal") || Input.GetButton("Horizontal");
        
        if (isMove == true && currentspeed < maxspeed)
        {
            currentspeed += Time.deltaTime * acceleration;
            acceleration += Time.deltaTime * 1.5f;


        }
        if (isMove == false)
        {
            currentspeed = beginspeed;
            rb.velocity = new Vector2(0, rb.velocity.y);
            acceleration = 1;
            animator.SetBool("isMove", false);
        }
        CollisionCheck();
        AnimationControl();
        //≥Â¥Ã
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
        //≥Â¥Ã¿‰»¥
        if (!isDashable)
        {
            dashCooldownTimer -= Time.deltaTime;
            if (dashCooldownTimer<=0)
            {
                isDashable = true;
            }
        }
        //π•ª˜º‰∏Ù
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
        //ª¨≤˘¿‰»¥
        if (slidingCooldownTimer > 0)
        {
            slidingCooldownTimer -= Time.deltaTime;
               
        }
        //ª¨«Ω
        if (!isGrounded && isWall)
        {
            isSlideWall = true;
            //ΩµµÕµÙ¬‰ÀŸ∂»
            if (rb.velocity.y < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                if (Input.GetKey(KeyCode.S))
                {
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 2);
                }
            //µ≈«ΩÃ¯
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

    }
    private void CheckInput()
    {
        //“∆∂Ø
        isMove = Input.GetButtonDown("Horizontal") || Input.GetButton("Horizontal");
        if (isMove == true && !isDash && !isAttack)
        {
            Move();
        }
        //Ã¯‘æ
        if (Input.GetButtonDown("Jump")&&isGrounded)
        {
            currentJumpForce = jumpForce;
            Jump();

        }
        //≥Â¥Ã
        if (Input.GetKeyDown(KeyCode.LeftShift)&&isDashable&&!isSlideWall)
        {
            Dash();
        }
        //π•ª˜
        if (Input.GetKeyDown(KeyCode.Mouse0)&&isGrounded)
        {
            Attack();
        }
        //ª¨≤˘
        if (Input.GetKeyDown(KeyCode.C)&&isGrounded&&isMove)
        {
            Slide();
            

        }
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
        //∆Ù∂Ø–≠≥Ã
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
        
        //“∆∂Ø∂Øª≠
        if (xInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);//∑¥◊™
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
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(faceDir * wallCheckDistance, 0, 0)); // ª≠≥ˆ…‰œﬂ
        Gizmos.DrawLine(new Vector3(transform.position.x+groundCheckRadius*faceDir,transform.position.y-offsety,transform.position.z), transform.position + Vector3.down * groundCheckDistance+Vector3.right*groundCheckRadius*faceDir);
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
    }   
    private void Dash()
    {
        dashTime=dashDuration;
        isDash = true;
        isDashable = false;
        dashCooldownTimer = dashCooldown;
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
       
        

    }
    private void DashMent()
    {
        
        rb.velocity = new Vector2(faceDir * currentspeed, 0);
        

    }
    public void SlideOver()
    {
        isSlide = false;
        currentspeed = beginspeed;
        //Ω¯––ª¨≤˘¿‰»¥
        slidingCooldownTimer = slidingCooldown; 
    }

    public void AttackOver()
    {
        isAttack = false;
        comobatCount++;
        if (comobatCount > 2)
        {
            comobatCount = 0;
        }
        
    }
}
