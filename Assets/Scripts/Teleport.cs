using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Teleport : MonoBehaviour
{
   public Vector3 teleportPosition;
    public bool Opne;
    public bool isOpen;
    public bool possibleOpen;
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<Player>()!= null&& Opne)
        {
            PlayerManager.instance.player.fx.OpenInteractionButton();
            possibleOpen = true;
        }
        else if (!isOpen && other.GetComponent<Player>() != null)
        {
            other.GetComponent<Player>().fx.GeneratePopToolTip("¥´ÀÕ√≈Œ¥Ω‚À¯");
        }
    }
    public void TeleportTo()
    {
        PlayerManager.instance.player.transform.position=teleportPosition;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerManager.instance.player.fx.CloseInteractionButton();
        possibleOpen = false;
    }
    public void Update()
    {
        if(possibleOpen && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = true;
          
            possibleOpen = false;
            PlayerManager.instance.player.fx.CloseInteractionButton();
        }
        if (isOpen)
        {
            TeleportTo();
            PlayerManager.instance.player.fx.CloseInteractionButton();
            isOpen = false;
     
            possibleOpen = false;
        }
    }
}
