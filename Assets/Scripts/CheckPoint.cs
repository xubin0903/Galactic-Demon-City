using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{   public bool active = false;
    public Animator animator;
    public string checkPointID;
   
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.GetComponent<Player>()!= null)
        {
            Activate();
        }
    }
    [ContextMenu("Generate ID")]
    private void GenerateID()
    {
        checkPointID=System.Guid.NewGuid().ToString();
    }
    public void Activate()
    {
        if (!active)
        {
            AudioManager.instance.StopSFX(5);
            AudioManager.instance.PlaySFX(5, transform);

        }
        active = true;
       
        animator.SetBool("Active", true);
    }

   
}
