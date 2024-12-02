using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighten_Controller : MonoBehaviour
{
    private CharacterStats targetStats;
    [SerializeField] private float speed;
    private Animator animator;
    private bool triggered;
    private int damage;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
     }
    public void SetUp(CharacterStats _targetStats, int _damage)
    {
        targetStats = _targetStats;
        damage = _damage;
    }
    private void Start()
    {
        
        transform.position=new Vector2(targetStats.transform.position.x,targetStats.transform.position.y+3f);
        if (Vector2.Distance(transform.position, targetStats.transform.position) < 1f)
        {
            AudioManager.instance.PlaySFX(6, null);
        }
    }
        
    
    private void Update()
    {
        if (targetStats == null)
        {
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetStats.transform.position.x, targetStats.transform.position.y), speed * Time.deltaTime);
        if(transform.position.y-targetStats.transform.position.y<0.1f && !triggered)
        {
            triggered = true;
            animator.SetTrigger("Lightening");
            Destroy(gameObject, 5f);
        }
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.GetComponent<Enemy>() != null)
        {
            
            var enemy = coll.GetComponent<Enemy>();
            
            enemy.OtherDamage(new Vector2(5, 5));
            
            PlayerManager.instance.player.stats.DoMagicDamage(enemy.stats);
            return;
           
        }
        if (coll.GetComponent<Player>() != null)
        {
            var player = coll.GetComponent<Player>();
           
        }
        
    }

}
