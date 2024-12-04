using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : CharacterStats
{
    private EnemyDropItem dropitemSystem;
    private Enemy enemy;
    private Slider healthBar;
    [Header("Level")]
    public int level;
    [Range(0f, 1f)]
    [SerializeField] private float perecentage;
    public Stat dropCurrency;
    
 


    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        healthBar=GetComponentInChildren<Slider>();
        dropitemSystem = GetComponent<EnemyDropItem>();
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
        dropCurrency.SetDefaultValue(100);
     
        Modify(maxHealth);
        Modify(damage);
        Modify(FireDamage);
        Modify(IceDamage);
        Modify(LightningDamage);
        Modify(criticalChance);
        Modify(evasion);
        Modify(armor);
        Modify(dropCurrency);
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
        healthBar.value = currentHealth / maxHealth.GetValue();
    }
    public override void Die()
    {
        base.Die();
        enemy.OnDie();
        healthBar.gameObject.SetActive(false);
        dropitemSystem.GenerateDropItems();
        PlayerManager.instance.currency+=(int)dropCurrency.GetValue();

    }
    private void Modify(Stat _stat)
    {
        if (_stat.GetValue() == 0)
        {
            return;
        }
        for (int i = 1; i < level; i++)
        {
            
            float modifier = _stat.GetValue() * perecentage;
            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }
}
