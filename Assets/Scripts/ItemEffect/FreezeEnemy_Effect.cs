using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FreezeEnemy_Effect", menuName = "Data/ItemEffect/FreezeEnemy_Effect")]

public class FreezeEnemy_Effect : ItemEffect
{
    public override void ExecuteEffect(Transform _transform)
    {
        //Debug.Log("Freeze Enemy Effect");
        var colls = Physics2D.OverlapCircleAll(_transform.position, 6f);
        foreach (var coll in colls)
        {
            if (coll.gameObject.GetComponent<Enemy>() != null)
            {
                //Debug.Log("Enemy is frozen");
                coll.gameObject.GetComponent<Enemy>().Freeze(10f);
            }
            
        }
    }
   
}
