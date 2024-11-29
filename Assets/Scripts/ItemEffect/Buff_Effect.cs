using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        playerStats.IncreaseStatBy( value,duration,playerStats.GetStat(statType));
    }
   

}
