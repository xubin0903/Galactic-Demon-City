using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : MonoBehaviour
{
    [Header("碰撞检测")]
    [SerializeField] protected Transform groundCheckPosition;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheckPosition;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected float offsetX;
    [SerializeField] protected float playerCheckDistance;
    [SerializeField] protected LayerMask playerLayer;
    public Transform attackCheck;
    [SerializeField] public float attackRadius;
    private EntityFX fx;

    [Header("受击反馈")]
    [SerializeField] protected Vector2[] HurtBackDir;
    [SerializeField] protected float backDuration;
    [SerializeField] protected bool isKoncked;

    [SerializeField] public bool isWall { private set; get; }
    [SerializeField] public bool isGrounded {private set; get; }

    [SerializeField] public RaycastHit2D playerCheck { get; private set; }
    [Header("组件")]
    public Rigidbody2D rb;
    public Animator anim;
    [SerializeField] public float faceDir;
    [Header("Animation状态")]
    public EnemyState currentState;

    //[SerializeField] protected bool facingRight  = true;
    public virtual void Awake()
    {
        fx = GetComponent<EntityFX>();
        
    }
    public virtual void Start()
    {

    }
    public virtual void Update()
    {
        faceDir = transform.localScale.x > 0 ? 1 : -1;
        CollisionCheck();
       
        
    }
    #region CollisionCheck
   
    
    public virtual void CollisionCheck()
    {
        playerCheck = Physics2D.Raycast(transform.position,Vector2.right*faceDir,playerCheckDistance,playerLayer);
        if(playerCheck.collider != null)
        {
            Debug.Log("Player is detected");
        }
        isWall = Physics2D.Raycast(wallCheckPosition.position, Vector2.right * faceDir, wallCheckDistance, groundLayer).collider != null;
        isGrounded = Physics2D.Raycast(groundCheckPosition.position + faceDir * offsetX * Vector3.right, Vector2.down, groundCheckDistance, groundLayer).collider != null;
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(groundCheckPosition.position +faceDir * offsetX * Vector3.right, Vector2.down*groundCheckDistance);
        Gizmos.DrawRay(wallCheckPosition.position,Vector2.right*wallCheckDistance*faceDir);
        Gizmos.DrawRay(transform.position,Vector2.right*playerCheckDistance*faceDir);
        Gizmos.DrawWireSphere(attackCheck.position,attackRadius);
    }
    #endregion
    #region Flip
    public virtual void Flip()
    {
        //翻转
        new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
    //public virtual void FlipControllerr(float _x)
    //{
    //    if (_x > 0 && !facingRight)
    //    {
    //        Flip();
    //    }else if(_x < 0 && facingRight)
    //    {
    //        Flip();
    //    }
    //}
    #endregion
    #region Velocity
    public virtual void SetZeroVelocity()
    {
        if (isKoncked)
        {
            return;
        }
       
        rb.velocity = new Vector2(0, 0);
    }
    public virtual void SetVelocity(Vector2 _velocity)
    {
        if (isKoncked)
        {
            return;
        }
        rb.velocity = _velocity;

    }
    #endregion
    #region Damage
    public void Damage(Player player)
    {
        Debug.Log(gameObject.name+"受到伤害");
        if(currentState.animName!="Attack")
        fx.Hurt();
        StartCoroutine(HurtBack(backDuration, player));
    }
    private IEnumerator HurtBack(float duration, Player player)
    {
        isKoncked = true;
        if (player.faceDir == faceDir)
        {
            
            rb.velocity = new Vector2(HurtBackDir[player.comobatCount].x * (faceDir), HurtBackDir[player.comobatCount].y);
            Debug.Log("受击反馈");
        }
        else
        {
            rb.velocity = new Vector2(HurtBackDir[player.comobatCount].x * (-faceDir), HurtBackDir[player.comobatCount].y);
            Debug.Log("受击反馈");

        }
        yield return new WaitForSeconds(duration);
        isKoncked = false;
    }
    #endregion
}
