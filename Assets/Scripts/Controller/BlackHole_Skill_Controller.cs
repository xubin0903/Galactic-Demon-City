using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackHole_Skill_Controller : MonoBehaviour
{
    [Header("Skill Settings")]
    private float maxSize;
    private float growSpeed;
    private bool canGrow;
    private List<Transform> targets=new List<Transform>();
    private List<KeyCode> keyCodes=new List<KeyCode>();
    private GameObject blackHoleKeyCodePrefab;
    private bool canAttack;
    private int maxAttackAmount;
    private int amountAttacked;
    private float  attackCooldown;
    private float attackCooldownTimer;
    private Vector3 CloneOffset;
    private List<GameObject> blackHoleKeyCodes=new List<GameObject>();
    private bool isShrinking;
    private float shrinkSpeed;
    private GameObject attackPrefab;
    private float[] damage;
    
    private void Update()
    {
        if (canGrow && !isShrinking)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(maxSize, maxSize, maxSize), growSpeed * Time.deltaTime);
        }
        if (isShrinking)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(-1, -1, -1), shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x <= 0)
            {
                Destroy(gameObject);
                

            }
            if(transform.localScale.x <= 5f)
            {
                //player�˳��ڶ�״̬
                PlayerManager.instance.player.ExitBlackHole();
            }
        }
       attackCooldownTimer -= Time.deltaTime;
        if (Input.GetKeyUp(KeyCode.R))
        {
            canAttack = true;
            if (amountAttacked >= maxAttackAmount)
            {
                canAttack = false;
            }
            //player���͸��
            PlayerManager.instance.player.fx.TransParent(true);
            if(targets.Count <= 0)
            {
                Invoke("AttackBlackHoleFinish", 0);
                
            }

        }
        CloneAttackLogic();
    }

    private void CloneAttackLogic()
    {
        if (canAttack && attackCooldownTimer <= 0&&amountAttacked <=maxAttackAmount)
        {
            attackCooldownTimer = attackCooldown;
            if (targets.Count <= 0)
            {
                Debug.Log("No Targets left");
                AttackBlackHoleFinish();
                return;
            }
            Vector3 offset;
            if (Random.Range(0, 100) < 50)
            {
                offset = -CloneOffset;
            }
            else
            {
                offset= CloneOffset;
            }
            int randomTargetIndex = Random.Range(0, targets.Count);
            Debug.Log("enemyTransform:" + targets[randomTargetIndex].position);
            var randomDamage = damage[Random.Range(0, damage.Length)];
            SkillManager.instance.clone.CreateClone(targets[randomTargetIndex], offset, randomDamage);
            //��������Ѿ�����ɾ��Ŀ��
            if (targets[randomTargetIndex].GetComponent<CharacterStats>().currentHealth <= 0)
            {
                targets.Remove(targets[randomTargetIndex]);
            }
            //Debug.Log("enemyTransform:" + targets[randomTargetIndex].position);
            //GameObject newclone=Instantiate(attackPrefab);
            //var position = targets[randomTargetIndex].position ;
            //targets.Remove(targets[randomTargetIndex]);
            //newclone.GetComponent<BlackHole_Attack_Controller>().SetUp(damage,0.3f,position);
            amountAttacked++;
            if(amountAttacked >= maxAttackAmount)
            AttackBlackHoleFinish();
        }
    }

    private void AttackBlackHoleFinish()
    {
        
            
            canAttack = false;
            isShrinking = true;
            OnDestroyKeyCodes();
        
    }
    private void PLayerStateChange()
    {
        //player�˳��ڶ�״̬
        PlayerManager.instance.player.ExitBlackHole();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CreateHoleKeyCode(collision);
    }

    private void CreateHoleKeyCode(Collider2D collision)
    {
        //keycode���˾Ͳ�������
        if(keyCodes.Count <= 0)
        {
            Debug.Log("No KeyCodes left");
            return;
        }
        //����Ѿ���ʼ�����˻��ߺڶ��Ѿ���ȫ���ɣ��Ͳ�������KeyCode
        if(canAttack&&transform.localScale.x>= maxSize-1)
        {
            return;
        }
        if (collision.GetComponentInChildren<BlackHole_KeyCode_Controller>() == null&&collision.GetComponent<Enemy>()!= null)
        {
           
            collision.GetComponent<Enemy>().FreezeTime(true);
            var newBlackHole_KeyCode = Instantiate(blackHoleKeyCodePrefab, collision.transform.position + Vector3.up * 2f, Quaternion.identity,collision.transform);
            //׷��ÿһ������Ӧһ������
            
            blackHoleKeyCodes.Add(newBlackHole_KeyCode);
            var choosenKeyCode = keyCodes[Random.Range(0, keyCodes.Count)];
            keyCodes.Remove(choosenKeyCode);
            newBlackHole_KeyCode.GetComponent<BlackHole_KeyCode_Controller>().SetKeyCode(choosenKeyCode, collision.transform,this);

        }
        if(collision.GetComponent<Enemy>()!= null)
        {
            if(collision.GetComponentInChildren<BlackHole_KeyCode_Controller>() != null)
            {
                if(!collision.GetComponent<Enemy>().isFrozen)
                collision.GetComponent<Enemy>().FreezeTime(true);
            }
        }
    }
    public void AddEnemyToTargets(Transform _enemyTransform)=> targets.Add(_enemyTransform);
    private void OnDestroyKeyCodes()
    {
        if(blackHoleKeyCodes.Count <= 0)
        {
            Debug.Log("No KeyCodes left");
            return;
        }
        foreach(var blackHoleKeyCode in blackHoleKeyCodes)
        {
            Destroy(blackHoleKeyCode);
        }
        blackHoleKeyCodes.Clear();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(false);
        }
    }
    public void SetupBlackHole(float _maxSize, float _growSpeed, bool _canGrow, List<KeyCode> _keyCodes, GameObject _blackHoleKeyCodePrefab, bool _canAttack, int _maxAttackAmount, float _attackCooldown, Vector3 _cloneOffset, bool _isShrinking, float _shrinkSpeed, float[] _damage, GameObject _attackPrefab)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        canGrow = _canGrow;
     
        keyCodes = new List<KeyCode>(_keyCodes);
        blackHoleKeyCodePrefab = _blackHoleKeyCodePrefab;
        canAttack = _canAttack;
        maxAttackAmount = _maxAttackAmount;
        
        attackCooldown = _attackCooldown;
        CloneOffset = _cloneOffset;
     
        isShrinking = _isShrinking;
        shrinkSpeed = _shrinkSpeed;
        damage = _damage;
        attackPrefab = _attackPrefab;
    }
}
