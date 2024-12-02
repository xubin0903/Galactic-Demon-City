using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : EnemyEntity
{
    
    [SerializeField] public float currentSpeed;
    [SerializeField] public float maxSpeed;
 
    [SerializeField] public float beginSpeed;
    private float defaultMaxSpeed;
    private float defaultBeginSpeed;
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
    //public string enemyID;
   
    public override void Awake()
    {
        base.Awake();
        
        
    }
    public  override void Start()
    {
        base.Start();
        currentSpeed = beginSpeed;
        defaultMaxSpeed= maxSpeed;
        defaultBeginSpeed = beginSpeed;

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
            beginSpeed = 0;
            maxSpeed = 0;
            anim.speed = 0;
            isFrozen=true;
            //Debug.Log(gameObject.name + " is frozen");
        }
        else
        {
            beginSpeed = defaultBeginSpeed;
            maxSpeed = defaultMaxSpeed;
            currentSpeed = beginSpeed;
            anim.speed = 1;
            isFrozen = false;
            //Debug.Log(gameObject.name + " is unfrozen");
        }
    }
    public virtual void Freeze(float duration)
    {
        StartCoroutine(FreezeFor(duration));
    }
    private IEnumerator  FreezeFor(float duration)
    {
        //Debug.Log(gameObject.name + " is frozen for " + duration + " seconds");
        FreezeTime(true);
        yield return new WaitForSeconds(duration);
        FreezeTime(false);
    }
    public virtual void OnDie()
    {
        isDead = true;
        //Debug.Log(gameObject.name + "OnDie");

    }
    public void IcedSlowEffect(float duration,float slowPercentage)
    {
        currentSpeed*=(1-slowPercentage);
        maxSpeed*=(1-slowPercentage);
        beginSpeed*=(1-slowPercentage);
        anim.speed = 1 - slowPercentage;
        Invoke("ResetSpeed", duration);
    }
    public void ResetSpeed()
    {
        beginSpeed = defaultBeginSpeed;
        maxSpeed = defaultMaxSpeed;
        currentSpeed = beginSpeed;
        anim.speed = 1; 

    }
    //[ContextMenu("Generate ID")]
    //private void GenerateID()
    //{
    //    enemyID = System.Guid.NewGuid().ToString();
    //}
}
