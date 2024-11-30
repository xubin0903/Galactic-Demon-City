using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class Crystal_Skill : Skill
{
    public SkillSlot_UI crystalUnlockSlot;
    public SkillSlot_UI explodeUnlockSlot;
    public SkillSlot_UI moveUnlockSlot;
    public SkillSlot_UI mulCrystalUnlockSlot;
    public SkillSlot_UI mirageUnlockSlot;
    public GameObject crystalPrefab;
    private GameObject currentCrystal;
    public bool explodedUnlocked;
    public bool crystalUnlocked;
    public bool mirageUnlocked;
    [SerializeField] private Vector3 returnOffset;
    [SerializeField] private float growSpeed;
    [SerializeField] private bool canGrow;
    [SerializeField] private float crystalDuration;
    [SerializeField] private Vector2 attackForce;
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool canMove;
    [SerializeField] private bool canMulCrystal;
    [SerializeField] private List<GameObject> mulCrystalList=new List<GameObject>();
    [SerializeField] private float amountOfMulCrystal;
    [SerializeField] private float MulCrystalCoolDomn;
    protected override void Start()
    {
        base.Start();
        crystalUnlockSlot.GetComponent<Button>().onClick.AddListener(() => CrystalUnlocked());
        explodeUnlockSlot.GetComponent<Button>().onClick.AddListener(() => ExplodeUnlocked());
        moveUnlockSlot.GetComponent<Button>().onClick.AddListener(() => MoveUnlocked());
        mulCrystalUnlockSlot.GetComponent<Button>().onClick.AddListener(() => MulCrystalUnlocked());
        mirageUnlockSlot.GetComponent<Button>().onClick.AddListener(() => MirageUnlocked());
        ResetMulCrystal();
    }


    public override bool CanUseSkill()
    {
        if (!crystalUnlocked)
        {
            return false;
        }
        return base.CanUseSkill();
    }
    public override void UseSkill()
    {
        if (canMulCrystal&&mulCrystalList.Count>0)
        {
            var newCrystal = Instantiate(mulCrystalList[0], PlayerManager.instance.player.transform.position, transform.rotation);
            newCrystal.GetComponent<Crystal_Skill_Controller>().SetupCrystal(growSpeed, canGrow, crystalDuration, attackForce, moveSpeed, FindClosestEnemy(newCrystal.transform), canMove,damage);
            newCrystal.GetComponent<Crystal_Skill_Controller>().Set_Idle();
            mulCrystalList.Remove(mulCrystalList[0]);
            if (mulCrystalList.Count <= 0)
            {
                coolDown=MulCrystalCoolDomn;
                coolTimer = MulCrystalCoolDomn;
            }

        }
        
        if (canMulCrystal)
        {
            return;
        }

        if (currentCrystal == null)
        {
            currentCrystal = Instantiate(crystalPrefab, PlayerManager.instance.player.transform.position, transform.rotation);
            currentCrystal.GetComponent<Crystal_Skill_Controller>().SetupCrystal(growSpeed, canGrow, crystalDuration, attackForce, moveSpeed, FindClosestEnemy(currentCrystal.transform), canMove, damage);
            currentCrystal.GetComponent<Crystal_Skill_Controller>().Set_Idle();
        }
        else
        {

            if (canMove)
            {
                return;
            }
            coolTimer = coolDown;
            var position = player.transform.position;
            player.transform.position = currentCrystal.transform.position + returnOffset;
            currentCrystal.transform.position = position;
            //延迟一小段时间
            if (explodedUnlocked)
            {
                StartCoroutine(FinishTRansform());

            }
            else if (mirageUnlocked)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero, 20);
                
            
            }
            
        }

    }
    public override void  Update()
    {
        base.Update();
        if (coolTimer < 0&&canMulCrystal)
        {
            ResetMulCrystal();

        }
    }
    private IEnumerator FinishTRansform()
    {
        yield return new WaitForSeconds(0.5f);
        if(currentCrystal!= null)
        currentCrystal.GetComponent<Crystal_Skill_Controller>().Set_Explode();
        currentCrystal = null;
    }
    private void ResetMulCrystal()
    {
        if(mulCrystalList.Count > 0)
        {
            return;
        }
        for (int i = 0; i < amountOfMulCrystal; i++)
        {
            mulCrystalList.Add(crystalPrefab);
        }
    }
    public void CrystalUnlocked()
    {
        if (crystalUnlockSlot.unlocked)
        {
            crystalUnlocked = true;
        }
    }
    public void ExplodeUnlocked()
    {
        if (explodeUnlockSlot.unlocked)
        {
            explodedUnlocked = true;
        }
    }
    public void MoveUnlocked()
    {
        if (moveUnlockSlot.unlocked)
        {
            canMove = true;
        }
    }
    public void MulCrystalUnlocked()
    {
        if (mulCrystalUnlockSlot.unlocked)
        {
            canMulCrystal = true;
        }
    }
    private void MirageUnlocked()
    {
       if (mirageUnlockSlot.unlocked)
        {
            mirageUnlocked = true;
        }
    }
}
