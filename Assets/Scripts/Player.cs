using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("基本属性")]
    [SerializeField] private float beginspeed;
    [SerializeField] private float maxspeed;
    [SerializeField] private float acceleration=1;
    [SerializeField] private float currentspeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float xInput;
    [SerializeField] private bool isMove;
    [SerializeField] private bool isJump;
    [Header("组件")]
    [SerializeField] private Animator animator;
    private Rigidbody2D rb;
     private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {

        //移动
        isMove = Input.GetButtonDown("Horizontal")||Input.GetButton("Horizontal");
        if (isMove == true)
        {
            Move();
            //移动动画
            if (xInput < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);//反转
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            animator.SetBool("isMove", true);
        }
        if(isMove == true&&currentspeed<maxspeed)
        {
            currentspeed += Time.deltaTime*acceleration;
            acceleration += Time.deltaTime*1.5f;
            

        }
        if (isMove==false)
        {
            currentspeed = beginspeed;
            rb.velocity = new Vector2(0, rb.velocity.y);
            acceleration = 1;
            animator.SetBool("isMove", false);
        }
        //跳跃
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }
    private void Move()
    {
        xInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(xInput * currentspeed, rb.velocity.y);
    }
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isJump = true;
        animator.SetBool("isJump", true);
    }
    
}
