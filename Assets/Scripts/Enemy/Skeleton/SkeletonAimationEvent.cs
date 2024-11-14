using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimationEvent : MonoBehaviour
{
    private Skeleton enemy;
    private void Awake()
    {
        enemy = GetComponentInParent<Skeleton>();
    }
    public void AttackFinish()
    {
        enemy.AttackFinish();
    }
    public void AttackEvent()
    {
        Collider2D[] colls = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackRadius);
        foreach (var coll in colls)
        {
            if (coll.GetComponent<Player>() != null)
            {
                coll.GetComponent<Player>().Damage();
            }
        }
    }
}
