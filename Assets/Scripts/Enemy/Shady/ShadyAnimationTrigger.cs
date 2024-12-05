using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyAnimationTrigger : MonoBehaviour
{
    private Shady enemy;
    private void Awake()
    {
        enemy = GetComponentInParent<Shady>();
    }
    public void AttackFinish()
    {
        enemy.AttackFinish();
    }
    public void AttackEvent()
    {
        enemy.Explode();

    }
    public void OpenCounterWindow() => enemy.OpenCounterWindow();
    public void CloseCounterWindow() => enemy.CloseCounterWindow();
    public void SetDestory() => Destroy(enemy.gameObject);
}
