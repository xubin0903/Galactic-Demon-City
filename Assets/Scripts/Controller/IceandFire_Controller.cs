using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceandFire_Controller : MonoBehaviour
{
    private CharacterStats stats;
    private void Start()
    {
        stats = PlayerManager.instance.player.stats;
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
      
        if (coll.GetComponent<Enemy>() != null)
        {
           
            coll.GetComponent<Enemy>().OtherDamage(new Vector2(0, 0));
            CharacterStats _targetStats = coll.GetComponent<Enemy>().stats;
            stats.DoMagicDamage(_targetStats);
            
        }
    }
    
   
}
