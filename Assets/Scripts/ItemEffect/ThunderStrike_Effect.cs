using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ThunderStrike_Effect", menuName = "Data/ItemEffect/ThunderStrike_Effect", order = 1)]
public class ThunderStrike_Effect : ItemEffect
{
    public GameObject thunderStrikePregab;
    public override void ExecuteEffect(Transform target)
    {
        GameObject thunderStrike = Instantiate(thunderStrikePregab, target.position, Quaternion.identity);
    }
}
