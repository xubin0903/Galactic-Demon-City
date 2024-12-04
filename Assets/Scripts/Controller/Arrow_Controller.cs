using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Controller : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private string targetLayerName = "Player";
    [SerializeField] private float xVelocity;
    private Rigidbody2D rb;
    [SerializeField] private bool canMove;
    [SerializeField] private bool flipped;

    private void Awake()
    {
       
        rb = GetComponent<Rigidbody2D>();

    }
    private void Update()
    {
        if(canMove)
        rb.velocity = new Vector2(xVelocity*transform.localScale.x, rb.velocity.y);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            collision.GetComponent<CharacterStats>().TakeDamage(damage);
            if (targetLayerName == "Enemy")
            {
                collision.GetComponent<Enemy>().OtherDamage(new Vector2(transform.localScale.x * 3, 0));
            }
            else
            {
                collision.GetComponent<Player>().Damage(null);
            }
            StuckInto(collision);
        }else if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            StuckInto(collision);
        }

    }

    private void StuckInto(Collider2D collision)
    {
        rb.isKinematic = true;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponentInChildren<ParticleSystem>().Stop();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        canMove = false;
        transform.parent = collision.transform;
        Destroy(gameObject, Random.Range(5, 7));
    }

    public void FlipArrow()
    {
        if (flipped)
        {
            return;
        }
        xVelocity *= -1;
        flipped = true;
        transform.Rotate(0, 180, 0);
        targetLayerName = "Enemy";
    }
}
