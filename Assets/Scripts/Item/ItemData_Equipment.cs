using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EquipmentType
{
    Weapon,// 武器
    Armor,// 盔甲
    Amulet,// 项链
    Flask// 药水
}
[CreateAssetMenu(fileName = "New Equipment Item", menuName = "Data/Equipment Data")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;
    [Header("Major Stats")]

    public float strength;//力量：增加一点伤害和1%的暴击率
    public float agility;//敏捷：增加一点移动速度（只能减少寒冰的减速效果）和1%的闪避率
    public float intelligence;//智力：增加3点魔法抗性和1点的魔法攻击力
    public float vitality;//体力：增加3或5点最大生命值
    [Header("Attack Stats")]
 
    public float damage;
    public float criticalChance;
    public float criticalPower;//
    [Header("Defensive Stats")]
    public float maxHealth;
    public float armor;//防御
    public float evasion;//闪避
    [Header("Magic Stats")]
    public float fireDamage;
    public float iceDamage;
    public float lightningDamage;
    public void AddModifer()
    {
        Player player = PlayerManager.instance.player;
        if (player == null) return;
        player.stats.strength.AddModifier(strength);
        player.stats.agility.AddModifier(agility);
        player.stats.intelligence.AddModifier(intelligence);
        player.stats.vitality.AddModifier(vitality);
        player.stats.damage.AddModifier(damage);
        player.stats.criticalChance.AddModifier(criticalChance);
        player.stats.criticalPower.AddModifier(criticalPower);  
        player.stats.maxHealth.AddModifier(maxHealth);
        player.stats.armor.AddModifier(armor);
        player.stats.evasion.AddModifier(evasion);
        player.stats.FireDamage.AddModifier(fireDamage);
        player.stats.IceDamage.AddModifier(iceDamage);
        player.stats.LightningDamage.AddModifier(lightningDamage);
    }
    public void RemoveModifer()
    {
        Player player = PlayerManager.instance.player;
        if (player == null) return;
        player.stats.strength.RemoveModifier(strength);
        player.stats.agility.RemoveModifier(agility);
        player.stats.intelligence.RemoveModifier(intelligence);
        player.stats.vitality.RemoveModifier(vitality);
        player.stats.damage.RemoveModifier(damage);         
        player.stats.criticalChance.RemoveModifier(criticalChance);
        player.stats.criticalPower.RemoveModifier(criticalPower);
        player.stats.maxHealth.RemoveModifier(maxHealth);
        player.stats.armor.RemoveModifier(armor);
        player.stats.evasion.RemoveModifier(evasion);
        player.stats.FireDamage.RemoveModifier(fireDamage);
        player.stats.IceDamage.RemoveModifier(iceDamage);
        player.stats.LightningDamage.RemoveModifier(lightningDamage);               
    }
}
