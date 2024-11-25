using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Skeleton enemy;
    private void Awake()
    {
        enemy = GetComponent<Skeleton>();
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
    public override void Die()
    {
        base.Die();
        enemy.OnDie();

    }
}
