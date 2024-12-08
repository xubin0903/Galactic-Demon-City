using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerSpellCastState : EnemyState
{
    public Bringer enemy;
  
    private float amountofSpell;
    private float spellcoolTimer;
    public BringerSpellCastState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName,Bringer _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        enemy = _enemy;

    }

    public override void Enter()
    {
        base.Enter();
        amountofSpell = enemy.spellAmout;
        spellcoolTimer = 0.5f;
        stateTimer = 5f;
        AudioManager.instance.PlaySFX(21, enemy.transform);
    }

    public override void Exit()
    {
        base.Exit();
    }

    

    public override void Update()
    {
        base.Update();
        spellcoolTimer -= Time.deltaTime;
        if (CanCastSpell())
        {
            enemy.CastSpell();
        }
        if(stateTimer <= 0)
        {
            enemy.stateMachine.ChangeState(enemy.teleportState);
        }
    }
    private bool CanCastSpell()
    {
        if (amountofSpell > 0 && spellcoolTimer <= 0)
        {
            Debug.Log("Can Cast Spell");
            spellcoolTimer = enemy.spellCoolDown;
            amountofSpell--;
            return true;
        }
        else
        {
            return false;
        }
    }
}
