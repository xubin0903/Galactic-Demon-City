using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    [Header("Major Stats")]

    public Stat strength;//����������һ���˺���1%�ı�����
    public Stat agility;//���ݣ�����һ���ƶ��ٶȺ�1%��������
    public Stat intelligence;//����������һ��ħ�����Ժ�3���ħ��������
    public Stat vitality;//����������3��5���������ֵ
    [Header("Defensive Stats")]
    public Stat maxHealth;
    public Stat armor;//����
    public Stat evasion;//����

    [SerializeField] public float currentHealth;
    [SerializeField] public Stat damage;

    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();
    }
    public virtual void TakeDamage( float _damage)
    {
        currentHealth -= _damage;
        if (currentHealth <= 0)
        {
           Die();
         
        }
    
    }
    public virtual void DoDamage(CharacterStats _TargetStats)
    {
        if (TargetCanAvoidAttack(_TargetStats))
        {
            Debug.Log(_TargetStats.gameObject.name + "is avoiding the attack");
            return;
            
        }
        float finalDamage = damage.GetValue() + strength.GetValue();
        finalDamage = CheckTargetArmor(_TargetStats, finalDamage);
        _TargetStats.TakeDamage( finalDamage);
        Debug.Log(_TargetStats.gameObject.name + "is taking damage:" + finalDamage);
    }

    public virtual  float CheckTargetArmor(CharacterStats _TargetStats, float finalDamage)
    {
        finalDamage -= _TargetStats.armor.GetValue();
        finalDamage = Mathf.Max(finalDamage, 0);
        return finalDamage;
    }

    public virtual bool TargetCanAvoidAttack(CharacterStats _TargetStats)
    {
        float toalEvasion = _TargetStats.evasion.GetValue() + _TargetStats.agility.GetValue();
        toalEvasion = Mathf.Clamp(toalEvasion, 0, 100);
        Debug.Log(_TargetStats.gameObject.name + "evasion:" + toalEvasion);
        if (Random.value < toalEvasion / 100)
        {
            //Debug.Log(gameObject.name + "is evading the attack");
            return true;
        }
        else
        {
            return false;

        }
    }

    public virtual void Die()
    {

    }
}
