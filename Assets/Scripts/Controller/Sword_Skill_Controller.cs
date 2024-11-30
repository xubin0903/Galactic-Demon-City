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
    [SerializeField] private float beginReturnSpeed;
    [SerializeField] private float returnSpeed;
    public bool canReturn;
    public bool isReturn;
    //[SerializeField] private float existduration;
    [SerializeField] private float returnDistance;
    [SerializeField] private Vector2 attackForce;
    [SerializeField] private float maxDistance;
    private float damage;
    [Header("弹跳")]
    [SerializeField] private bool isBouncing;
     private int maxBounceAmount;
    [SerializeField] private int bounceCount = 0;
    [SerializeField] private List<Transform> bounceList;
    private float bounceSpeed;
    private int targetIndex = 0;
    [Header("Pierce Info")]
    private bool isPiercing;
    private int pierceCount;
    private int pieceAmount;
    private float pierceSpeed;
    

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CircleCollider2D>();
        sr=GetComponentInChildren<SpriteRenderer>();
    }
    public void SetupSword(Vector2 launchDir,float gravityScale,float _damage)
    {
        //Debug.Log("Setting up sword");
        rb.velocity = launchDir;
        rb.gravityScale = gravityScale;
        damage=_damage;
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
            returnSpeed = beginReturnSpeed+Vector2.Distance(transform.position, PlayerManager.instance.player.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, PlayerManager.instance.player.transform.position, returnSpeed*Time.deltaTime);
            if(Vector2.Distance(transform.position, PlayerManager.instance.player.transform.position) < returnDistance)
            {
                PlayerManager.instance.player.CatchSword();
                returnSpeed = 0;
            }
            transform.right = -rb.velocity;
        }
        //距离过远自动销毁
        if(Vector2.Distance(transform.position, PlayerManager.instance.player.transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
#region 弹跳
        if (isBouncing && bounceList.Count > 1)
        {
            
            transform.position = Vector3.MoveTowards(transform.position, bounceList[targetIndex].position, bounceSpeed*Time.deltaTime);
            if (Vector2.Distance(transform.position, bounceList[targetIndex].position) < 0.1f)
            {
                targetIndex++;
                bounceCount++;
                //实现来回弹跳
                if(targetIndex >= bounceList.Count)
                {
                    targetIndex = 0;
                }
                
                
            }
        }
#endregion
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (bounceList.Count <= 0)
        {
            AddBounceList(collision);

        }
        if (bounceCount >= maxBounceAmount)
        {
            isBouncing = false;
            bounceCount = 0;
        }
        StuckInto(collision);
        //如果是敌人进行攻击
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().OtherDamage(attackForce);
            collision.GetComponent<CharacterStats>().TakeDamage(damage);
            ItemData_Equipment targetEquipment = Inventory.instance.GetEquippedment(EquipmentType.Amulet);
            if (targetEquipment != null)
            {
                targetEquipment.ExecuteEffects(coll.transform);
            }


        }
        //StartCoroutine(ReturnAfterTime(existduration));
    }

    private void AddBounceList(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 10);
            foreach (Collider2D col in cols)
            {
                if (col.GetComponent<Enemy>() != null)
                {
                    bounceList.Add(col.transform);
                }
            }
        }
    }

    private void StuckInto(Collider2D collision)
    {
        if (isPiercing&&collision.GetComponent<Enemy>() != null)
        {
            
             collision.GetComponent<Enemy>().OtherDamage(attackForce);
            collision.GetComponent<CharacterStats>().TakeDamage(damage);
            ItemData_Equipment targetEquipment = Inventory.instance.GetEquippedment(EquipmentType.Amulet);
            if (targetEquipment != null)
            {
                targetEquipment.ExecuteEffects(coll.transform);
            }

            pierceCount++;
            if (pierceCount >= pieceAmount)
            {
                isPiercing = false;
                pierceCount = 0;
            }
            return;
        }
        canRotate = false;
        //Debug.Log("Sword collided with " + collision.transform.name);
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        rb.isKinematic = true;
        if (isBouncing && bounceList.Count > 1)
        {
            return;
            
        }
        canReturn = true;
        AnimationSword(false);
        coll.enabled = false;
        //挂载到物体
        transform.parent = collision.transform;
    }

   



    //剑的返回
    public void ReturnSword()
    {
        if (SkillManager.instance.sword.doublePierce&&isPiercing)
        {
            coll.enabled = true;
        }
        canReturn = false;
        canRotate = true;
        isReturn=true;
        rb.isKinematic = false;
       
        transform.parent = null;
        
       if (isPiercing)
        {
            AnimationSword(false);
        }
       else 
        {
            AnimationSword(true);

        }
    }
    //弹跳模式剑
    public void SetBounce(bool _isBouncing,int _maxBounceAmount,float _bounceSpeed)
    {
        isBouncing=_isBouncing;
        maxBounceAmount=_maxBounceAmount;
        bounceSpeed=_bounceSpeed;
    }
    public void SetPierce(bool _isPiercing,int _pieceAmount,float _pierceSpeed)
    {   
        isPiercing = _isPiercing;
        pieceAmount = _pieceAmount;
        pierceSpeed = _pierceSpeed;
    

    }
}
