using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    [Header("Major Stats")]

    public Stat strength;//����������һ���˺���1%�ı�����
    public Stat agility;//���ݣ�����һ���ƶ��ٶȺ�1%��������
    public Stat intelligence;//����������3��ħ�����Ժ�1���ħ��������
    public Stat vitality;//����������3��5���������ֵ
    [Header("Defensive Stats")]
    public Stat maxHealth;
    public Stat armor;//����
    public Stat evasion;//����
    [Header("Attack Stats")]
    [SerializeField] public Stat damage;
    public Stat criticalChance;
    public Stat criticalPower;//Ĭ�ϡ�150
    [Header("Magic Stats")]
    public bool isFired;//�����Ի����˺�
    public bool isIced;//�����Ա�������20%
    public bool isLightned;//����������
    public Stat magicResist;
    public Stat FireDamage;
    public Stat IceDamage;
    public Stat LightningDamage;
    public float iceDuration;
    public float lightningDuration;
    public float fireDuration;
    public float fireCooldown;
    private float fireTimer;
    private float fireCooldownTimer;
    private float iceTimer; 
    private float lightningTimer;


    [SerializeField] public float currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();
        criticalPower.SetDefaultValue(150);
    }
    protected virtual void Update()
    {
        if(fireTimer > 0)
        {
            fireTimer -= Time.deltaTime;
            fireCooldownTimer -= Time.deltaTime;
            if (fireTimer <= 0)
            {
                isFired = false;
                
            }
        }
        if (fireCooldownTimer <0&&isFired)
        {
            TakeDamage(2);
            Debug.Log(gameObject.name + "is do firedamage");
            fireCooldownTimer = fireCooldown;
        }
        if(iceTimer > 0)
        {
            iceTimer -= Time.deltaTime;
            if (iceTimer <= 0)
            {
                isIced = false;
            }
        }
        if(lightningTimer > 0)
        {
            lightningTimer -= Time.deltaTime;
            if(lightningTimer <= 0)
            {
                isLightned = false;
            }
        }
        
    }
    public virtual void TakeDamage(float _damage)
    {
        currentHealth -= _damage;
        if (currentHealth <= 0)
        {
            Die();

        }

    }
    public virtual void DoDamage(CharacterStats _TargetStats)
    {
       
        DoPhysicsDamage(_TargetStats);
        DoMagicDamage(_TargetStats);
    }

    public  virtual void DoPhysicsDamage(CharacterStats _TargetStats)
    {
        if (TargetCanAvoidAttack(_TargetStats))
        {
            Debug.Log(_TargetStats.gameObject.name + "is avoiding the attack");
            return;

        }
        float finalDamage = damage.GetValue() + strength.GetValue();
        finalDamage = CheckTargetArmor(_TargetStats, finalDamage);
        if (CanCrticalAttack())
        {
            finalDamage = GetCriticalDamage(finalDamage);
            Debug.Log(gameObject.name + "is doing a critical attack");
        }
        _TargetStats.TakeDamage(finalDamage);
        Debug.Log(_TargetStats.gameObject.name + "is taking Physicedamage:" + finalDamage);
    }

    public virtual void DoMagicDamage(CharacterStats _TargetStats)
    {
        float finalMagicDamage;
        
        var fireDamage = FireDamage.GetValue();
        var iceDamage = IceDamage.GetValue();
        var lightningDamage = LightningDamage.GetValue();
        finalMagicDamage = fireDamage + iceDamage + lightningDamage;
        if (finalMagicDamage <= 0)
        {
            Debug.Log("no magic damage");
            return;

        }
     
        //ѡ�����ֵһ��ħ���˺�����
        bool useFire = false;
        bool useIce = false;
        bool useLightning = false;
        useFire = fireDamage>iceDamage&&fireDamage>lightningDamage;
        useIce = iceDamage>fireDamage&&iceDamage>lightningDamage;
        useLightning = lightningDamage>fireDamage&&lightningDamage>iceDamage;
        if(!useFire && !useIce && !useLightning)
        {
            //ʹ��α����㷨ѡ��һ���˺�����
            int randomNum = Random.Range(0, 3);
            switch (randomNum)
            {
                case 0:
                    useFire = true;
                    break;  
                case 1:
                    useIce = true;  
                break;  
                case 2:
                    useLightning = true;  
                break;
            }
        }
        //Debug.Log("useFire:" + useFire + " useIce:" + useIce + " useLightning:" + useLightning);
        _TargetStats.ApplyMagicStaus(useFire, useIce, useLightning);

        finalMagicDamage += intelligence.GetValue() * 1;
        finalMagicDamage = CheckTargetMagicResist(_TargetStats, finalMagicDamage);
        finalMagicDamage=Mathf.Clamp(finalMagicDamage,0,float.MaxValue);
        _TargetStats.TakeDamage(finalMagicDamage);
        Debug.Log(_TargetStats.gameObject.name + "is taking magicdamage:" + finalMagicDamage);

    }
    public virtual float CheckTargetArmor(CharacterStats _TargetStats, float finalDamage)
    {
        
        float defennce=_TargetStats.armor.GetValue();
        if (_TargetStats.isIced)
        {
            defennce *= 0.8f;
            Debug.Log(defennce);
        }

       
        
        finalDamage -= defennce;
        finalDamage = Mathf.Max(finalDamage, 0);
        return finalDamage;
    }

    public virtual bool TargetCanAvoidAttack(CharacterStats _TargetStats)
    {
        float toalEvasion = _TargetStats.evasion.GetValue() + _TargetStats.agility.GetValue();
        if (_TargetStats.isLightned)
        {
            toalEvasion -= 20;
        }
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
    public virtual bool CanCrticalAttack()
    {
        float critical = criticalChance.GetValue() + strength.GetValue();
        if (Random.value < critical / 100)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    public virtual float GetCriticalDamage(float _damage)
    {
        var criticalPower = this.criticalPower.GetValue();

        return _damage * criticalPower / 100;
    }

    public virtual void Die()
    {

    }
    public virtual void ApplyMagicStaus(bool _isFired, bool _isIced, bool _isLightned)
    {
        if (isFired || isIced || isLightned)
        {
            return;//����Ѿ���ħ��Ч���������ٴη���
        }
        if (_isFired)
        {
            isFired = _isFired;
            fireTimer = fireDuration;
            fireCooldownTimer = fireCooldown;
           // Debug.Log(gameObject.name + "is using fire");

        }else if (_isIced)
        {

            isIced = _isIced;
            iceTimer = iceDuration;
           // Debug.Log(gameObject.name + "is using ice");
        }
        else if (_isLightned)
        {

            isLightned = _isLightned;
            lightningTimer = lightningDuration;
            //Debug.Log(gameObject.name + "is using lightning");
        }
        
    }

    public virtual float CheckTargetMagicResist(CharacterStats _TargetStats, float _damage)
    {
        var magicResist = _TargetStats.magicResist.GetValue() + _TargetStats.intelligence.GetValue() * 3;
        magicResist = Mathf.Clamp(magicResist, 0, 100);
        return _damage * (100 - magicResist) / 100;

    }
}