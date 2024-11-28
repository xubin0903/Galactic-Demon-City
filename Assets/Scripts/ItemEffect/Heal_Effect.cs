using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Heal_Effect", menuName = "Data/ItemEffect/Heal_Effect")]

public class Heal_Effect : ItemEffect
{
    [Range(0, 1)]
    public float healPercent;
    public override void ExecuteEffect(Transform target)
    {
        CharacterStats playerStats = PlayerManager.instance.player.stats;
        var amount = playerStats.maxHealth.GetValue() * healPercent;
        playerStats.OncreatHealth(amount);
    }
   
}
