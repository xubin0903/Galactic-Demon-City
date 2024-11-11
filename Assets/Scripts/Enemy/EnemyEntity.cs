using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : MonoBehaviour
{
    [Header("Åö×²¼ì²â")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask groundLayer;
    [Header("×é¼þ")]
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] protected float faceDir { get; private set; } = 1;
    [SerializeField] protected bool facingRight  = true;
    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    public virtual void Start()
    {

    }
    public virtual void Update()
    {

    }
    #region CollisionCheck
    public virtual bool IsGrounded() => Physics.Raycast(groundCheck.position,Vector2.down,groundCheckDistance ,groundLayer);
    public virtual bool IsWall() => Physics.Raycast(wallCheck.position,Vector2.right,wallCheckDistance,groundLayer);
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(groundCheck.position,Vector2.down*groundCheckDistance);
        Gizmos.DrawRay(wallCheck.position,Vector2.right*wallCheckDistance);
    }
    #endregion
    #region Flip
    public virtual void Flip()
    {
        faceDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
    public virtual void FlipControllerr(float _x)
    {
        if (_x > 0 && !facingRight)
        {
            Flip();
        }else if(_x < 0 && facingRight)
        {
            Flip();
        }
    }
    #endregion
    #region Velocity
    public virtual void SetZeroVelocity() =>new Vector2(0,0);
    public virtual void SetVelocity(Vector2 _velocity)
    {
        rb.velocity = _velocity;
        FlipControllerr(_velocity.x);
    }
    #endregion  
}
