using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackFinish : MonoBehaviour
{
    private Enemy enemy;
    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }
    public void AttackFinish()
    {
        enemy.AttackFinish();
    }
}
