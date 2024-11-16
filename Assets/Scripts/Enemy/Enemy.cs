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
    public float idleTimer;
    [Header("状态")]
    public bool isMove;
    public bool canAttack=true;
    
    
   
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
   
}
