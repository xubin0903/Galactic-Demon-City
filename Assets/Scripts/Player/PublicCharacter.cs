using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicCharacter : MonoBehaviour
{
   [Header("»ù±¾ÊôÐÔ")]
   [SerializeField] protected float speed;
   [SerializeField] protected bool isMove;
   [SerializeField] public int faceDir;
   [SerializeField] protected float currentspeed;
   
   protected Rigidbody2D rb;
   protected Collider2D coll;
   [Header("Åö×²¼ì²â")]
   [SerializeField] protected LayerMask groundLayer;
   [SerializeField] protected float groundCheckDistance;
   [SerializeField] protected float groundCheckRadius;
   [SerializeField] protected bool isGrounded;
    public Transform attackCheck;
    public float attackRadius;


    protected virtual void Awake()
   {
     
      rb = GetComponent<Rigidbody2D>();
      coll = GetComponent<Collider2D>();
   }

   protected virtual void Start()
   {

   }

   protected virtual void Update()
   {
        CollisionCheck();
   }

   protected virtual void FixedUpdate()
   {

   }
   protected virtual void CollisionCheck()
   {
       isGrounded = Physics2D.Raycast(new Vector2(transform.position.x + groundCheckRadius, transform.position.y), Vector2.down, groundCheckDistance, groundLayer);

   }
   protected virtual void OnDrawGizmos()
   {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(transform.position.x + groundCheckRadius, transform.position.y, transform.position.z), transform.position + Vector3.down * groundCheckDistance + Vector3.right * groundCheckRadius);
       
    }
  
}
