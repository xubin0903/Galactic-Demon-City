
using UnityEngine;

public class Chest : MonoBehaviour
{
    public bool canOpen;
    public bool open;
    private Animator animator;
    private bool possiblyOpen;
    public string chestID;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (open == true)
        {
            this.gameObject.SetActive(false);
        }
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
            AudioManager.instance.PlaySFX(22,transform);
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

        transform.GetComponent<ChestDropItem>().GenerateDropItems();
        this.gameObject.SetActive(false);
    }

    [ContextMenu("Generate ID")]
    private void GenerateID()
    {
        chestID = System.Guid.NewGuid().ToString();
    }
}
