using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private bool Idle;
    private bool Expode;
    private Rigidbody2D rb;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

    }
    private void Start()
    {
        rb.gravityScale = 0;
    }
    public void Set_Idle()
    {
        anim.SetBool("Idle", true);
        Idle = true;
        Expode = false;
    }
    public void Set_Explode()
    {
        Idle = false;
        anim.SetBool("Idle", false);
        anim.SetBool("Explode", true);
        Expode = true;
    }
    public  void OnDestroy()
    {
        Destroy(gameObject);
    }
}
