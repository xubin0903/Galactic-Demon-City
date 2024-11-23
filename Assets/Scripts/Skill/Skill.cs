using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float coolDown;
    protected float coolTimer;
    protected Player player;
    public float damage;
    protected  virtual void Start()
    {
        player=PlayerManager.instance.player;
    }
    public virtual void Update()
    {
        coolTimer -= Time.deltaTime;
    }
    public virtual bool CanUseSkill()
    {
        if(coolTimer <= 0)
        {
            UseSkill();
            return true;
        }
        else
        {
            return false;
        }
    }
    public virtual void UseSkill()
    {
        coolTimer = coolDown;
    }
    public virtual Transform FindClosestEnemy(Transform origin)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(origin.position, 25);
        GameObject closestEnemy = null;
        float closestDistance = float.MaxValue;
        foreach (var collider in colliders)
        {
            var enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                float distance = Vector2.Distance(origin.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy.gameObject;
                }
            }
        }

        return closestEnemy!= null? closestEnemy.transform : null;
    }
}
