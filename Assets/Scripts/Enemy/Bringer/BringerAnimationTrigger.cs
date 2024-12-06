using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerAnimationTrigger : MonoBehaviour
{
    private Bringer enemy;
    private void Awake()
    {
        enemy = GetComponentInParent<Bringer>();
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
                coll.GetComponent<Player>().Damage(enemy);
                PlayerStats targetStats = coll.GetComponent<PlayerStats>();
                enemy.stats.DoDamage(targetStats);
            }
        }

    }
    public void OpenCounterWindow() => enemy.OpenCounterWindow();
    public void CloseCounterWindow() => enemy.CloseCounterWindow();
    public void SetDestory() => Destroy(enemy.gameObject);
    public void SetisVulnerable() => enemy.isVulnerable = true;
    public void SetisInvulnerable() => enemy.isVulnerable = false;
    public void FindPosition() => enemy.FindPosition();
    public void CanSpellCast()=> enemy.CanSpellCast();

}
