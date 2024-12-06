using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    armor,
    magicResist,
    damage,
    criticalChance,
    criticalPower,
    fireDamage,
    iceDamage,
    lightningDamage,
    evasion,
    maxHealth,
}
public class CharacterStats : MonoBehaviour
{

    [Header("Major Stats")]

    public Stat strength;//����������һ���˺���1%�ı�����
    public Stat agility;//���ݣ�����һ���ƶ��ٶȣ�ֻ�ܼ��ٺ����ļ���Ч������1%��������
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
    public bool isIced;//�����Ա�������20% and ����20%
    public bool isLightned;//����������
    public Stat magicResist;
    public Stat FireDamage;
    public Stat IceDamage;
    public Stat LightningDamage;
    public float iceDuration;
    public float lightningDuration;
    public float fireDuration;
    public float fireCooldown;
    protected float fireTimer;
    protected float fireCooldownTimer;
    protected float iceTimer; 
    protected float lightningTimer;
    [Header("Alive Status and Effect")]
    public Sprite FiredEffectSprite;
    public Sprite IcedEffectSprite;
    public Sprite LightningEffectSprite;
    protected Sprite aliveEffectSprite;
    public GameObject aliveEffectUI;
    protected EntityFX fx;
    public GameObject lighteningEffect;

    [Header("Current Health")]
    [SerializeField] public float currentHealth;
    [SerializeField] public bool isDead;
   

    protected virtual void Start()
    {
        currentHealth = GetMaxHealth();
        criticalPower.SetDefaultValue(150);
        aliveEffectSprite = aliveEffectUI.GetComponent<SpriteRenderer>().sprite;
        fx = GetComponent<EntityFX>();
        
        
    }
    protected virtual void Update()
    {
        AliveEffectUpdate();
       

    }
    public virtual float GetMaxHealth()
    {
        return maxHealth.GetValue()+vitality.GetValue()*Random.Range(3,5);
    }
    public virtual float GetSlowPercent()
    {
        float slowPercent = 0;
        slowPercent = agility.GetValue() / 100;
        if (isIced)
        {
            slowPercent+=0.2f;
        }
        return slowPercent;
    }
    public virtual void OncreatHealth(float _health)
    {
        currentHealth += _health;
        if (currentHealth > GetMaxHealth())
        {
            currentHealth = GetMaxHealth();
        }
    }
    private void AliveEffectUpdate()
    {
        if (fireTimer > 0)
        {
            fireTimer -= Time.deltaTime;
            fireCooldownTimer -= Time.deltaTime;
            if (fireTimer <= 0)
            {
                isFired = false;
                TurnoffAliveEffect();
            }
        }
        if (fireCooldownTimer < 0 && isFired)
        {
            TakeDamage(maxHealth.GetValue()*0.01f);
            Debug.Log(gameObject.name + "is do firedamage");
            fireCooldownTimer = fireCooldown;
        }
        if (iceTimer > 0)
        {
            iceTimer -= Time.deltaTime;
            if (iceTimer <= 0)
            {
                isIced = false;
                TurnoffAliveEffect();
            }
        }
        if (lightningTimer > 0)
        {
            lightningTimer -= Time.deltaTime;
            if (lightningTimer <= 0)
            {
                isLightned = false;
                TurnoffAliveEffect();
            }
        }
    }

    public virtual void ApplyAliveEffect()
    {
        if (aliveEffectSprite != null)
        {
            aliveEffectUI.GetComponent<SpriteRenderer>().sprite = aliveEffectSprite;
            aliveEffectUI.SetActive(true);
        }
    }
    public virtual void TurnoffAliveEffect()
    {
        this.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        aliveEffectUI.SetActive(false);
    }
    public virtual void TakeDamage(float _damage)
    {
        DecreaseHealthBy(_damage);

    }

    protected virtual void DecreaseHealthBy(float _damage)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= _damage;
        fx.GeneratePopToolTip(_damage.ToString());
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
        //Debug.Log(_TargetStats.gameObject.name + "is taking Physicedamage:" + finalDamage);
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
            //������������㲢����ȣ�����һ�������㣬�����ѡ��һ��
            if (fireDamage == iceDamage && lightningDamage == 0)
            {
                if(Random.value > 0.5f)
                {
                    useFire = true;
                }
                else
                {
                    useIce = true;
                }
            }   
            else if (fireDamage == lightningDamage && iceDamage == 0)
            {
                if (Random.value > 0.5f)
                {
                    useFire = true;
                }
                else
                {
                    useLightning = true;
                }
            }
            else if (iceDamage == lightningDamage && fireDamage == 0)
            {
                if(Random.value > 0.5f)
                {
                    useIce = true;
                }
                else
                {
                    useLightning = true;
                }
            }
            else
            {
            //ʹ��α����㷨ѡ��һ���˺�����,�����������㣬�����ѡ��һ��
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
        }
        //Debug.Log("useFire:" + useFire + " useIce:" + useIce + " useLightning:" + useLightning);
        _TargetStats.ApplyMagicStaus(useFire, useIce, useLightning);

        finalMagicDamage += intelligence.GetValue() * 1;
        finalMagicDamage = CheckTargetMagicResist(_TargetStats, finalMagicDamage);
     
        Debug.Log("finalMagicDamage:" + finalMagicDamage);
        _TargetStats.TakeDamage(finalMagicDamage);
        //Debug.Log(_TargetStats.gameObject.name + "is taking magicdamage:" + finalMagicDamage);

    }
    public virtual float CheckTargetArmor(CharacterStats _TargetStats, float finalDamage)
    {
        
        float defennce=_TargetStats.armor.GetValue();
        if (_TargetStats.isIced)
        {
            defennce *= 0.8f;
            //Debug.Log(defennce);
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
        //Debug.Log(_TargetStats.gameObject.name + "evasion:" + toalEvasion);
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
        isDead = true;
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
          
            aliveEffectSprite = FiredEffectSprite;
            fx.FiredColorFor(fireDuration);
           // Debug.Log(gameObject.name + "is using fire");

        }else if (_isIced)
        {

            isIced = _isIced;
            iceTimer = iceDuration;
            
           aliveEffectSprite = IcedEffectSprite;
            if (GetComponent<Player>() != null)
            {
                GetComponent<Player>().IcedSlowEffect(iceDuration, GetSlowPercent());
                Debug.Log(gameObject.name + "is using ice");
            }
            else if(GetComponent<Enemy>() != null)
            {
                GetComponent<Enemy>().IcedSlowEffect(iceDuration, GetSlowPercent());
            }
            fx.IcedColorFor(iceDuration);
           // Debug.Log(gameObject.name + "is using ice");
        }
        else if (_isLightned)
        {

            isLightned = _isLightned;
            lightningTimer = lightningDuration;
            
            aliveEffectSprite = LightningEffectSprite;
            fx.LightnedColorFor(lightningDuration);
            GameObject effect = Instantiate(lighteningEffect);

            effect.GetComponent<Lighten_Controller>().SetUp(this, 10);
            //Debug.Log(gameObject.name + "is using lightning");
        }
        ApplyAliveEffect();
    }

    public virtual float CheckTargetMagicResist(CharacterStats _TargetStats, float _damage)
    {
        var magicResist = _TargetStats.magicResist.GetValue() + _TargetStats.intelligence.GetValue() * 3;
        magicResist = Mathf.Clamp(magicResist, 0, 100);
        return _damage * (100 - magicResist) / 100;

    }
    public void IncreaseStatBy(int modifer, float duration, Stat targetStat)
    {
        StartCoroutine(IncreaseStatByCoroutine(modifer, duration, targetStat));
    }

    private IEnumerator IncreaseStatByCoroutine(int modifer, float duration, Stat targetStat)
    {
        targetStat.AddModifier(modifer);
        if (targetStat == maxHealth)
        {
            currentHealth +=modifer ;
        }
        
        yield return new WaitForSeconds(duration);
        targetStat.RemoveModifier(modifer);
    }

    public Stat GetStat(StatType type)
    {
        switch (type)
        {
            case StatType.strength:
                return strength;
            case StatType.agility:
                return agility;
            case StatType.intelligence:
                return intelligence;
            case StatType.vitality:
                return vitality;
            case StatType.armor:
                return armor;
            case StatType.magicResist:
                return magicResist;
            case StatType.damage:
                return damage;
            case StatType.criticalChance:
                return criticalChance;
            case StatType.criticalPower:
                return criticalPower;
            case StatType.fireDamage:
                return FireDamage;
            case StatType.iceDamage:
                return IceDamage;
            case StatType.lightningDamage:
                return LightningDamage;
            case StatType.evasion:
                return evasion;
            case StatType.maxHealth:
                return maxHealth;
            default:
                return null;
        }
    }

}