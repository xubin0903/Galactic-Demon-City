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
                coll.GetComponent<Player>().Damage(enemy);
                PlayerStats targetStats = coll.GetComponent<PlayerStats>();
                enemy.stats.DoDamage(targetStats);
            }
        }
    }
    public void OpenCounterWindow()=> enemy.OpenCounterWindow();
    public void CloseCounterWindow()=> enemy.CloseCounterWindow();
    public void EnemyAfterDead()
    {
        StartCoroutine(EnemyDieAndDrop());
    }
    private IEnumerator EnemyDieAndDrop()
    {
       yield return new WaitForSeconds(1f);
       enemy.cd.enabled = false;
        enemy.rb.gravityScale = 10;//�ӿ������ٶ�
       //����һ����ȹ������ٵ���
       yield return new WaitForSeconds(10f);
       Destroy(enemy.gameObject);
    }
}
