using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : EnemyEntity
{
    
    [SerializeField] public float currentSpeed;
    [SerializeField] public float maxSpeed;
    [SerializeField] public float beginSpeed;
    [SerializeField] public float lastAttackTime;
    [SerializeField] private float attckCoolDown;
    public bool isFrozen;
    public float idleTimer;
    [Header("状态")]
    public bool isMove;
    public bool canAttack=true;
    public bool isDead;
    [Header("counter 窗户")]
    [SerializeField] protected bool canbeStunned;
    [SerializeField] protected GameObject counterImage;
    [Header("Character Stats")]
    [SerializeField] public float damage;
   
    public override void Awake()
    {
        base.Awake();
        
        
    }
   
    public  void Move()
    {
        SetVelocity(new Vector2(currentSpeed * faceDir, rb.velocity.y));
    }
    public void CanAttack()
    {
        if (Time.time > lastAttackTime + attckCoolDown&&!isKoncked)
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }
    }
   public virtual void OpenCounterWindow()
    {
        canbeStunned = true;
        counterImage.SetActive(true);
    }
    public virtual void CloseCounterWindow()
    {
        canbeStunned = false;
        counterImage.SetActive(false);
    }
    public virtual bool CanStun()
    {
        if (canbeStunned)
        {
            CloseCounterWindow();
            return true;
        }else
        {
            return false;
        }
    }
    public virtual void FreezeTime(bool isFreeze)
    {
        if (isFreeze)
        {
            currentSpeed = 0;
            anim.speed = 0;
            isFrozen=true;
        }
        else
        {
            currentSpeed = beginSpeed;
            anim.speed = 1;
            isFrozen = false;
        }
    }
    public virtual void OnDie()
    {
        isDead = true;
        Debug.Log(gameObject.name + "OnDie");

    }
}
