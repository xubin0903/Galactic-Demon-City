using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    public override void DoDamage(CharacterStats _TargetStats)
    {
        base.DoDamage(_TargetStats);
    }

    public override void TakeDamage(float _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Start()
    {
        base.Start();
    }
    public override void Die()
    {
        base.Die();
        PlayerManager.instance.player.OnDie();
    }

    public override float CheckTargetArmor(CharacterStats _TargetStats, float finalDamage)
    {
        return base.CheckTargetArmor(_TargetStats, finalDamage);
    }

    public override bool TargetCanAvoidAttack(CharacterStats _TargetStats)
    {
        return base.TargetCanAvoidAttack(_TargetStats);
    }
}
