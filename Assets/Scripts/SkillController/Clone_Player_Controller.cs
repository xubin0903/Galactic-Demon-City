using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Player_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private float losingAlphaSpeed;
    [SerializeField] private float CloneTimer;
    [SerializeField] private Vector2 beatBack;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackRadius;
     private Animator animator;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator= GetComponent<Animator>();
    }
    private void Update()
    {
        CloneTimer -= Time.deltaTime;
        if (CloneTimer <= 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a- Time.deltaTime *losingAlphaSpeed);
        }
        if(sr.color.a <= 0)
        {
            Destroy(gameObject);

        }
    }
    public void SetupClone( float cloneDuraion,Vector3 clonePosition,bool _canAttack)
    {

        Debug.Log("CloneTransform: " +clonePosition);
        transform.position = clonePosition;
        CloneTimer = cloneDuraion;
        if (_canAttack)
        {
            animator.SetInteger("AttackNumber", Random.Range(1, 4));
        }
        FaceClostestEnemy();
    }
    private void AnimatorEventnTrigger()
    {
        CloneTimer = .1f;
    }
    public void AnimationAttackEvent()
    {
        Debug.Log("攻击");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius);
        foreach (var collider in colliders)
        {
            //Debug.Log(collider.name);
            var enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                //Debug.Log("Enemy Type: " + enemy.GetType().Name); // 打印实际类型

                enemy.OtherDamage(beatBack);
            }
        }

    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(attackCheck.position, attackRadius);
    //}
    private void FaceClostestEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        GameObject closestEnemy = null;
        float closestDistance = float.MaxValue;
        foreach (var collider in colliders)
        {
            var enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy.gameObject;
                }
            }
            if (closestEnemy != null)
            {
               if(closestEnemy.transform.position.x > transform.position.x)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
            }
        }
    }
}
