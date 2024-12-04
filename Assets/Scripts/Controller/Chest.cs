using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public bool canOpen;
    private bool open;
    private Animator animator;
    private bool possiblyOpen;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.GetComponent <Player>()!= null && canOpen)
        {
            var player = coll.GetComponent<Player>();
            player.fx.OpenInteractionButton();
            possiblyOpen = true;
            
        }
    }
    private void Update()
    {
        if (possiblyOpen && Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("Open");
            open = true;
            possiblyOpen = false;
            canOpen = false;
            PlayerManager.instance.player.fx.CloseInteractionButton();
        }
    }
    
    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.GetComponent <Player>()!= null && canOpen&&!open)
        {
            var player = coll.GetComponent<Player>();
            player.fx.CloseInteractionButton();
            possiblyOpen = false;
        }
    }
    public void Destroy()
    {
        
        Destroy(this.gameObject);
        transform.GetComponent<ChestDropItem>().GenerateDropItems();
    }
}
