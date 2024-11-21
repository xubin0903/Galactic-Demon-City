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
        }
        attackCooldownTimer -= Time.fixedDeltaTime;
        if (Input.GetKeyUp(KeyCode.R))
        {
            canAttack = true;
            if (amountAttacked >= maxAttackAmount)
            {
                canAttack = false;
            }
        }
        CloneAttackLogic();
    }

    private void CloneAttackLogic()
    {
        if (canAttack && attackCooldownTimer <= 0)
        {
            attackCooldownTimer = attackCooldown;
            if (targets.Count <= 0)
            {
                Debug.Log("No Targets left");
                return;
            }
            //随机选择偏移量
            if (Random.Range(0, 2) == 0)
            {
                CloneOffset = Vector3.left;
            }
            else
            {
                CloneOffset = Vector3.right;
            }
            int randomTargetIndex = Random.Range(0, targets.Count);
            SkillManager.instance.clone.CreateClone(targets[randomTargetIndex], CloneOffset);
            amountAttacked++;
            if (amountAttacked >= maxAttackAmount)
            {
                canAttack = false;
                isShrinking = true;
                OnDestroyKeyCodes();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CreateHoleKeyCode(collision);
    }

    private void CreateHoleKeyCode(Collider2D collision)
    {
        //keycode满了就不再生成
        if(keyCodes.Count <= 0)
        {
            Debug.Log("No KeyCodes left");
            return;
        }
        //如果已经开始攻击了或者黑洞已经完全生成，就不再生成KeyCode
        if(canAttack&&transform.localScale.x>= maxSize)
        {
            return;
        }
        if (collision.GetComponent<Enemy>() != null)
        {
            if (collision.GetComponent<Enemy>().isFrozen)
            {
                return;
            }
            collision.GetComponent<Enemy>().FreezeTime(true);
            var newBlackHole_KeyCode = Instantiate(blackHoleKeyCodePrefab, collision.transform.position + Vector3.up * 2f, Quaternion.identity);
            blackHoleKeyCodes.Add(newBlackHole_KeyCode);
            var choosenKeyCode = keyCodes[Random.Range(0, keyCodes.Count)];
            keyCodes.Remove(choosenKeyCode);
            newBlackHole_KeyCode.GetComponent<BlackHole_KeyCode_Controller>().SetKeyCode(choosenKeyCode, collision.transform,this);

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
    public void SetupBlackHole(float _maxSize, float _growSpeed, bool _canGrow, List<KeyCode> _keyCodes, GameObject _blackHoleKeyCodePrefab, bool _canAttack, int _maxAttackAmount, float _attackCooldown, Vector3 _cloneOffset, bool _isShrinking, float _shrinkSpeed)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        canGrow = _canGrow;
     
        keyCodes = _keyCodes;
        blackHoleKeyCodePrefab = _blackHoleKeyCodePrefab;
        canAttack = _canAttack;
        maxAttackAmount = _maxAttackAmount;
        
        attackCooldown = _attackCooldown;
        CloneOffset = _cloneOffset;
     
        isShrinking = _isShrinking;
        shrinkSpeed = _shrinkSpeed;
    }
}
