using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : CharacterStats
{
    private EnemyDropItem dropitemSystem;
    private Skeleton enemy;
    private Slider healthBar;
    [Header("Level")]
    public int level;
    [Range(0f, 1f)]
    [SerializeField] private float perecentage;
    
 


    private void Awake()
    {
        enemy = GetComponent<Skeleton>();
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

     
        Modify(maxHealth);
        Modify(damage);
        Modify(FireDamage);
        Modify(IceDamage);
        Modify(LightningDamage);
        Modify(criticalChance);
        Modify(evasion);
        Modify(armor);
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

    }
    private void Modify(Stat _stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = _stat.GetValue() * perecentage;
            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }
}
