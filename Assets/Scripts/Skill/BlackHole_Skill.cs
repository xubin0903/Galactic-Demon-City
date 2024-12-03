using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackHole_Skill : Skill
{
    public GameObject blackHolePrefab;
    private GameObject blackHole;
    public SkillSlot_UI blackHoleUnlockedSlot;
    public bool blackHoleUnlocked;
    [Header("Skill Settings")]
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private bool canGrow;

    [SerializeField] private List<KeyCode> keyCodes;
    [SerializeField] private GameObject blackHoleKeyCodePrefab;
    [SerializeField] private bool canAttack;
    [SerializeField] private int maxAttackAmount;

    [SerializeField] private float attackCooldown;

    [SerializeField] private Vector3 CloneOffset;

    [SerializeField] private bool isShrinking;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private float[]  damages;
    [SerializeField] private GameObject attackPrefab;

    protected override void Start()
    {
        base.Start();
        blackHoleUnlockedSlot.GetComponent<Button>().onClick.AddListener(() => UnlockBlackHole());
        
    }
    public override void Update()
    {
        base.Update();
        if(coolTimer <=0)
        {
            PlayerManager.instance.player.canBlackHole = true;
        }
        else
        {
            PlayerManager.instance.player.canBlackHole = false;
        }
    }
    public override bool CanUseSkill()
    {
        //if(balckHoleUnlocked == false)
        //{
        //    return false;
        //}
        if (coolTimer <= 0)
        {
            UseSkill();
            return true;
        }
        else
        {
           
            return false;
        }

    }
    public override void UseSkill()
    {
        base.UseSkill();
        CreateBlackHole();
    }
    public void CreateBlackHole()
    {
        blackHole = Instantiate(blackHolePrefab, PlayerManager.instance.player.transform.position , Quaternion.identity);
        blackHole.GetComponent<BlackHole_Skill_Controller>().SetupBlackHole(maxSize, growSpeed, canGrow, keyCodes, blackHoleKeyCodePrefab, canAttack, maxAttackAmount, attackCooldown, CloneOffset, isShrinking, shrinkSpeed, damages, attackPrefab);
    }
    public void UnlockBlackHole()
    {
        if (blackHoleUnlockedSlot.unlocked)
        {
            blackHoleUnlocked = true;  
        }
    }
    protected override void CheckUnlocked()
    {
        UnlockBlackHole();
        
    }
}
  
