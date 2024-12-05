using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyExplodeController : MonoBehaviour
{
    private CharacterStats myStats;
    public float growSpeed;
    public float maxSize;
    public float explodeRadius;
    private bool canGrow=true;
    private Animator animator;
    private  void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void SetExplode(CharacterStats stats,float growSpeed,float maxSize,float explodeRadius)
    {
        myStats = stats;
        this.growSpeed = growSpeed;
        this.maxSize = maxSize;
        this.explodeRadius = explodeRadius;
    }
    private void Update()
    {
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
        if (maxSize - transform.localScale.x <= 0.5f)
        {
            animator.SetTrigger("Explode");
            canGrow = false;
        }
    }
    private void AnimationExpldeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explodeRadius);
        foreach (Collider2D collider in colliders)
        {
            if(collider.GetComponent<Player>()!= null)
            {
                collider.GetComponent<Player>().Damage(null);
                myStats.DoDamage(collider.GetComponent<Player>().stats);
            }
        }
        
    }
    private void SetDestroy()=> Destroy(gameObject);
}
