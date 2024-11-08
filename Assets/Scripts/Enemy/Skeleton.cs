using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Skeleton : PublicCharacter
{
    [SerializeField] private bool isWall;
    [SerializeField] private float TimeToWall;
    [SerializeField] private float TimeToWallTimer;
    [SerializeField] private float checkPlayerDistance;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private bool isAttack;
    private RaycastHit2D isPlayerDetected;
    protected override void Start()
    {
        faceDir = transform.localScale.x > 0 ? 1 : -1;
        currentspeed=speed;
    }
    protected override void Update()
    {

        base.Update();
        faceDir = transform.localScale.x > 0 ? 1 : -1;
        if (!isWall&&!isAttack)
        MoveMent();
        AnimatorControl();
        CollisionCheck();
        if (isWall || !isGrounded)
        {
            Flip();

        }
        if(isPlayerDetected.collider!= null)
        {
            if (isPlayerDetected.distance > 0.5f)
            {
                
                isAttack = false;
                currentspeed =speed*2;
            }
            else
            {
                rb.velocity = Vector2.zero;
                isAttack = true;
                isMove = false;
            }
        }
        else
        {
            isAttack = false;
            currentspeed = speed;
            
        }
    }



    private void MoveMent()
    {
        isMove = true;
        rb.velocity = new Vector2(faceDir * currentspeed, rb.velocity.y);

    }
    private void AnimatorControl()
    {
        animator.SetBool("isMove", isMove);
        animator.SetBool("isAttack", isAttack);
    }
    protected override void CollisionCheck()
    {
        isGrounded = Physics2D.Raycast(new Vector2(transform.position.x + groundCheckRadius * faceDir, transform.position.y), Vector2.down, groundCheckDistance, groundLayer);
        isWall = Physics2D.Raycast(transform.position, Vector2.right * faceDir, 0.8f, groundLayer);
        isPlayerDetected = Physics2D.Raycast(transform.position, Vector2.right , checkPlayerDistance*faceDir, whatIsPlayer);
    }
    private void Flip()
    {
        Vector3 localScale = transform.localScale; // 获取当前缩放
        localScale.x *= -1; // 翻转X轴缩放
        transform.localScale = localScale; // 
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(faceDir * 0.8f, 0, 0)); // 画出射线
        Gizmos.DrawLine(transform.position, new(transform.position.x + checkPlayerDistance * faceDir, transform.position.y, transform.position.z));// 画出检测玩家射线

    }
}
