using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightBorneAnimationEvent : MonoBehaviour
{
    private NightBorne enemy;
    private void Awake()
    {
        enemy = GetComponentInParent<NightBorne>();
    }
    public void AttackFinish()
    {
        enemy.AttackFinish();
    }
    public void AttackEvent()
    {
        AudioManager.instance.PlaySFX(21, enemy.transform);
        Collider2D[] colls = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackRadius);
        foreach (var coll in colls)
        {
            if (coll.GetComponent<Player>() != null)
            {
                coll.GetComponent<Player>().Damage(enemy);
                PlayerStats targetStats = coll.GetComponent<PlayerStats>();
                Debug.Log(targetStats.name + "受到" + enemy.name + "攻击");
                enemy.stats.DoDamage(targetStats);
               
            }
        }
    }
    public void OpenCounterWindow() => enemy.OpenCounterWindow();
    public void CloseCounterWindow() => enemy.CloseCounterWindow();
   public void Die()
    {
        Destroy(enemy.gameObject);
    }
    public void DieExPlosion()
    {
        Collider2D[] colls = Physics2D.OverlapCircleAll(enemy.transform.position, 3f);
        foreach (var coll in colls)
        {
            if (coll.GetComponent<Player>() != null)
            {
                coll.GetComponent<Player>().Damage(enemy);
                PlayerStats targetStats = coll.GetComponent<PlayerStats>();
                Debug.Log(targetStats.name + "受到" + enemy.name + "攻击");
                enemy.stats.DoMagicDamage(targetStats);
            }
        }
    }
}
