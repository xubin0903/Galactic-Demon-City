using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("»ù±¾ÊôÐÔ")]
    [SerializeField] private float beginspeed;
    [SerializeField] private float maxspeed;
    [SerializeField] private float acceleration = 1;
    [SerializeField] private float currentspeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float xInput;
    [SerializeField] private bool isMove;
    [SerializeField] private int faceDir;
    
    [Header("³å´Ì")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashDuration;
    [SerializeField] private bool isDash;
    [SerializeField] private bool isDashable;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashCooldownTimer;
    [Header("¹¥»÷")]
    [SerializeField] private float attackTimer;
    [SerializeField] private float attackDuration;
    private bool isAttack;
    [SerializeField] private int comobatCount;
    
    [Header("×é¼þ")]
    [SerializeField] private Animator animator;
    private Rigidbody2D rb;
    [Header("Åö×²¼ì²â")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {

        //¼ì²âÊäÈë
        CheckInput();
        //ËÙ¶ÈÏÞÖÆ
        isMove = Input.GetButtonDown("Horizontal") || Input.GetButton("Horizontal");
        if (isMove == true&&!isDash)
        {
            Move();
        }
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
        CheckGrounded();
        AnimationControl();
        //³å´Ì
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
        //³å´ÌÀäÈ´
        if (!isDashable)
        {
            dashCooldownTimer -= Time.deltaTime;
            if (dashCooldownTimer<=0)
            {
                isDashable = true;
            }
        }
        //¹¥»÷¼ä¸ô
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


    }
    private void Move()
    {
        
        xInput = Input.GetAxis("Horizontal");
        if (xInput > 0)
        {
            faceDir = 1;

        }
        else if (xInput < 0)
        {
            faceDir = -1;
        }
        
        rb.velocity = new Vector2( xInput * currentspeed, rb.velocity.y);
        
        //ÒÆ¶¯¶¯»­
        if (xInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);//·´×ª
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        
        
    }
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        AttackOver();
       
        
    }
    private void CheckInput()
    {
        //ÒÆ¶¯
        isMove = Input.GetButtonDown("Horizontal") || Input.GetButton("Horizontal");
        if (isMove == true)
        {
            Move();
        }
        //ÌøÔ¾
        if (Input.GetButtonDown("Jump")&&isGrounded)
        {
            Jump();

        }
        //³å´Ì
        if (Input.GetKeyDown(KeyCode.LeftShift)&&isDashable)
        {
            Dash();
        }
        //¹¥»÷
        if (Input.GetKeyDown(KeyCode.Mouse0)&&isGrounded)
        {
            Attack();
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(transform.position.x+groundCheckRadius,transform.position.y,transform.position.z), transform.position + Vector3.down * groundCheckDistance+Vector3.right*groundCheckRadius);
    }
    private void CheckGrounded()
    {
      isGrounded = Physics2D.Raycast(new Vector2(transform.position.x+groundCheckRadius,transform.position.y), Vector2.down, groundCheckDistance, groundLayer);
      
    }
   private void AnimationControl()
    {
        animator.SetBool("isMove", isMove);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetBool("isDash", isDash);
        animator.SetBool("isAttack", isAttack);
        animator.SetInteger("comobatCount", comobatCount);
    }   
    private void Dash()
    {
        dashTime=dashDuration;
        isDash = true;
        isDashable = false;
        dashCooldownTimer = dashCooldown;
        if (!isMove)
        {
            rb.velocity = new Vector2(faceDir * currentspeed, rb.velocity.y);
        }
    }
    private void Attack()
    {
        isAttack = true;
        attackTimer = attackDuration;
        rb.velocity = new Vector2(0, rb.velocity.y);
        

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
    private void DashMent()
    {
        rb.velocity = new Vector2(faceDir * currentspeed, rb.velocity.y);
    }

}
