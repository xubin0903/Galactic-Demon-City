using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostCurrentcy : MonoBehaviour
{
   public int currentcy;
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.GetComponent<Player>() != null)
        {
            PlayerManager.instance.currency+=this.currentcy;
            Destroy(this.gameObject);
        }
    }
}
