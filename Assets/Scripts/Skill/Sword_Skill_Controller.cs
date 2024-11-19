using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private CircleCollider2D coll;
    private SpriteRenderer sr;
    private bool canRotate = true;
    [SerializeField] private float returnSpeed;
    public bool canReturn;
    public bool isReturn;
    //[SerializeField] private float existduration;
    [SerializeField] private float returnDistance;
    

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CircleCollider2D>();
        sr=GetComponentInChildren<SpriteRenderer>();
    }
    public void SetupSword(Vector2 launchDir,float gravityScale)
    {
        Debug.Log("Setting up sword");
        rb.velocity = launchDir;
        rb.gravityScale = gravityScale;
    }
    public void AnimationSword(bool _Flip)
    {
        animator.SetBool("Flip", _Flip);
    }
    private void Update()
    {
        if(canRotate)
        transform.right = rb.velocity;
        if (isReturn)
        {
            transform.position = Vector3.MoveTowards(transform.position, PlayerManager.instance.player.transform.position, returnSpeed*Time.deltaTime);
            if(Vector2.Distance(transform.position, PlayerManager.instance.player.transform.position) < returnDistance)
            {
                PlayerManager.instance.player.ClearSword();
            }
        }
        
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        canReturn = true;
        canRotate = false;
        Debug.Log("Sword collided with " + collision.transform.name);
        //挂载到物体
        rb.velocity = Vector2.zero;
        coll.enabled = false;
        AnimationSword(false);
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;
        //StartCoroutine(ReturnAfterTime(existduration));
    }
    //private IEnumerator ReturnAfterTime(float duration)
    //{
    //    yield return new WaitForSeconds(duration);
    //    ReturnSword();
    //}



    //剑的返回
    public void ReturnSword()
    {
        canReturn = false;
        canRotate = true;
        isReturn=true;
        rb.isKinematic = false;
       
        transform.parent = null;
        
       
        AnimationSword(true);
    }
}
