using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrike_Controller : MonoBehaviour
{
    private CharacterStats stats;
    private void Start()
    {
        stats = PlayerManager.instance.player.stats;
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log("ThunderStrike_Controller");
        if(coll.GetComponent<Enemy>()!= null)
        {
            Debug.Log("ThunderStrike_Controller Enemy");
            coll.GetComponent<Enemy>().OtherDamage(new Vector2(0,8));
            CharacterStats _targetStats = coll.GetComponent<Enemy>().stats;
            stats.DoMagicDamage(_targetStats);
            AudioManager.instance.PlaySFX(29, null);
            Invoke("OnDestroy", 1f);
        }
    }
    private void OnDestroy()
    {
        Destroy(this.gameObject);
    }
}
