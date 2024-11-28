using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
}
[CreateAssetMenu(fileName = "Buff_Effect", menuName = "Data/ItemEffect/Buff_Effect")]
public class Buff_Effect : ItemEffect
{
    private CharacterStats playerStats;
    public StatType statType;
    public int value;
    public float duration;
    public override void ExecuteEffect(Transform target)
    {
        playerStats = PlayerManager.instance.player.stats;
        playerStats.IncreaseStatBy( value,duration,GetStat(statType));
    }
    private Stat GetStat(StatType type)
    {
        switch (type)
        {
            case StatType.strength:
                return playerStats.strength;
            case StatType.agility:
                return playerStats.agility;
            case StatType.intelligence:
                return playerStats.intelligence;
            case StatType.vitality:
                return playerStats.vitality;
            case StatType.armor:
                return playerStats.armor;
            case StatType.magicResist:
                return playerStats.magicResist;
            case StatType.damage:
                return playerStats.damage;
            case StatType.criticalChance:
                return playerStats.criticalChance;
            case StatType.criticalPower:
                return playerStats.criticalPower;
            case StatType.fireDamage:
                return playerStats.FireDamage;
            case StatType.iceDamage:
                return playerStats.IceDamage;
            case StatType.lightningDamage:
                return playerStats.LightningDamage;
            case StatType.evasion:
                return playerStats.evasion;
            default:
                return null;
        }
    }

}
