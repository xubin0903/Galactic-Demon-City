using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : CharacterStats
{
    private Skeleton enemy;
    private Slider healthBar;
 


    private void Awake()
    {
        enemy = GetComponent<Skeleton>();
        healthBar=GetComponentInChildren<Slider>();
    }
    public override void DoDamage(CharacterStats _TargetStats)
    {
        base.DoDamage(_TargetStats);
    }

    public override void TakeDamage(float _damage)
    {
        base.TakeDamage( _damage);
    }

    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
        healthBar.value = currentHealth / maxHealth.GetValue();
    }
    public override void Die()
    {
        base.Die();
        enemy.OnDie();
        healthBar.gameObject.SetActive(false);

    }
}
