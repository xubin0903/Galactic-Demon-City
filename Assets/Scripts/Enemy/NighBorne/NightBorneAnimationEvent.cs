using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightBorneAnimationEvent : MonoBehaviour
{
    private NightBorne enemy;
    private void Awake()
    {
        enemy=GetComponentInParent<NightBorne>();
    }
    public void AttackFinish()
    {
        enemy.AttackFinish();
    }
    public void AttackEvent()
    {
        Collider2D[] colls=Physics2D.OverlapCircleAll(enemy.attackCheck.position,enemy.attackRadius);
        foreach(var coll in colls)
        {
            if (coll.GetComponent<Player> ()!= null)
            {
                coll.GetComponent<Player>().Damage(enemy);
                //coll.GetComponent<CharacterStats>().TakeDamage(enemy.stats.damage.GetValue());
            }
        }
    }
}
