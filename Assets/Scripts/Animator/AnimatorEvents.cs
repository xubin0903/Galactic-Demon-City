using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEvents : MonoBehaviour
{
   private Player player;
    private void Awake()
    {
        player=GetComponentInParent<Player>();
    }
    private void AnimatorEventnTrigger()
    {
        player.AttackOver();
    }
    public void AnimationAttackEvent()
    {
        Debug.Log("����");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRadius);
        foreach (var collider in colliders)
        {
            //Debug.Log(collider.name);
            var enemy = collider.GetComponent<Enemy>();
            if (enemy!= null)
            {
                //Debug.Log("Enemy Type: " + enemy.GetType().Name); // ��ӡʵ������

                enemy.Damage(player);
            }
        }
            
    }
    public void CounterAttackEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRadius);
        foreach (var collider in colliders)
        {
            //Debug.Log(collider.name);
            var enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (enemy.CanStun())
                {
                    
                player.CounterAttackOver();
                player.isSuccessfulCounterAttack = true;
                }
            }
        }
    }
    public void SuccessfulCounterAttackOverEvent()
    {
        player.SuccessfulCounterAttackOver();
    }
    public void ThrowSwordEvent()
    {
        player.stateMachine.ChangeState(player.baseState);
        SkillManager.instance.sword.CreateSword();
    }
    public void ChangeBaseSatetEvent()
    {
        player.stateMachine.ChangeState(player.baseState);
    }
}
