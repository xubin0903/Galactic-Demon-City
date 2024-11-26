using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : CharacterStats
{
    [Header("Health Bar")]
    public Image greenHealBar;
    public Image redHealBar;
    public float bufferEffect;

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
        currentHealth = maxHealth.GetValue();
        criticalPower.SetDefaultValue(150);
        aliveEffectSprite = aliveEffectUI.GetComponent<Image>().sprite;
        greenHealBar.fillAmount = 1;
        redHealBar.fillAmount = 1;
        fx=GetComponent<EntityFX>();
        
    }
    protected override void Update()
    {

        base.Update();
        if (currentHealth <= 0)
        {
            Die();
        }
        StartCoroutine(UpdataHealBar());
       
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
    private IEnumerator UpdataHealBar()
    {
        greenHealBar.fillAmount = currentHealth / maxHealth.GetValue();
        while (greenHealBar.fillAmount < redHealBar.fillAmount)
        {
            redHealBar.fillAmount -=  bufferEffect;
            yield return new WaitForSeconds(0.5f);
        }
        if (redHealBar.fillAmount <= greenHealBar.fillAmount)
        {
            redHealBar.fillAmount = greenHealBar.fillAmount;
        }
    }
    public override void ApplyAliveEffect()
    {
        if (aliveEffectSprite != null)
        {
            aliveEffectUI.GetComponent<Image>().sprite = aliveEffectSprite;
            aliveEffectUI.SetActive(true);
        }
    }




}
