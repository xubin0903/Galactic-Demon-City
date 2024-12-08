using System.Collections;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private bool Idle;
    private bool Explode;
    private Rigidbody2D rb;
    private float growSpeed;
    private bool canGrow;
    private float crystalDuration;
    private Vector2 attackForce;
    private CircleCollider2D cd;
    private bool canMove;
    private float moveSpeed;
    private Transform closestEnemy;
    private float damage;
   
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }
    private void Update()
    {
        if (canGrow)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector2(2, 2), growSpeed * Time.deltaTime);
            if (transform.localScale.x >= 2)
            {
                canGrow = false;
            }
        }
        if (canMove)
        {
            if (closestEnemy == null)
            {
                canMove=false;
                Invoke("Set_Explode", 0.5f);
                return;
            }
            transform.position = Vector2.MoveTowards(transform.position, closestEnemy.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, closestEnemy.position) <= 0.2f)
            {
                canMove = false;
                Set_Explode();

            }
        }
    }
    private void Start()
    {
        rb.gravityScale = 0;
    }
    
    public void Set_Idle()
    {
        
        Idle = true;
        Explode = false;
        anim.SetBool("Idle", Idle);
        anim.SetBool("Explode", Explode);
      
    }
    public void Set_Explode()
    {
        Idle = false;
        Explode = true;
        anim.SetBool("Idle", Idle);
        anim.SetBool("Explode", Explode);
    }
    public  void OnDestroy()
    {
        Destroy(gameObject);
    }
    public void SetupCrystal(float _growSpeed,bool _canGrow,float _crystalDuration,Vector2 _attackForce,float _moveSpeed,Transform _closestEnemy,bool _canMove,float _damage)
    {
        growSpeed = _growSpeed;
        canGrow = _canGrow;
        crystalDuration = _crystalDuration;
        attackForce = _attackForce;
        moveSpeed = _moveSpeed;
        closestEnemy = _closestEnemy;
        canMove = _canMove;
        damage = _damage;
        StartCoroutine(CrystalDuration());
    }
    private IEnumerator CrystalDuration()
    {
        yield return new WaitForSeconds(crystalDuration);
        if (SkillManager.instance.crystal.explodedUnlocked)
        {
            Set_Explode();

        }
        else
        {
            OnDestroy();
        }
    }
    public void ExplodeCollision()
    {
        var colls = Physics2D.OverlapCircleAll(transform.position,cd.radius);
        foreach (var coll in colls)
        {
            if (coll.GetComponent<Enemy>()!= null)
            {
                if (coll.transform.position.x < transform.position.x)
                {
                    attackForce.x = -attackForce.x;
                }
                coll.gameObject.GetComponent<Enemy>().OtherDamage(attackForce);
                PlayerManager.instance.player.stats.DoMagicDamage(coll.gameObject.GetComponent<Enemy>().stats);
                coll.GetComponent<CharacterStats>().TakeDamage(damage);
                ItemData_Equipment targetEquipment = Inventory.instance.GetEquippedment(EquipmentType.Amulet );
                if (targetEquipment != null)
                {
                    targetEquipment.ExecuteEffects(coll.transform);
                }
            }
        }
       
    }
}
