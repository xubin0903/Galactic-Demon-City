using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BlackHole_Attack_Controller : MonoBehaviour
{
   public float[] damages;
   public float cooldown;
   public float cooldownTimer;


    public void SetUp(float[] damages, float cooldown,Vector3 _target)
    {
        transform.position = _target;
        this.damages = damages;
        this.cooldown = cooldown;
       
    }
    public void Update()
    {
        cooldownTimer -= Time.deltaTime;
       
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("BlackHole Attacked");
        if(other.GetComponent<Enemy>()!= null)
        {
            Debug.Log("BlackHole Attacked Enemy");
            if(cooldownTimer <= 0)
            {
                other.GetComponent<Enemy>().OtherDamage(Vector2.zero);
                var random = Random.Range(0, damages.Length);
                other.GetComponent<CharacterStats>().TakeDamage(damages[random]);
                Debug.Log("BlackHole Attacked Enemy"+damages[random]);
                cooldownTimer = cooldown;


            }
            
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("BlackHole Attacked");
        if (other.GetComponent<Enemy>() != null)
        {
            Debug.Log("BlackHole Attacked Enemy");
            if (cooldownTimer <= 0)
            {
                other.GetComponent<Enemy>().OtherDamage(Vector2.zero);
                var random = Random.Range(0, damages.Length);
                other.GetComponent<CharacterStats>().TakeDamage(damages[random]);
                Debug.Log("BlackHole Attacked Enemy" + damages[random]);
                cooldownTimer = cooldown;


            }

        }
    }
    public void OnDestroy()
    {
        Destroy(gameObject);
    }
}
