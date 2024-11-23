using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Crystal_Skill : Skill
{
    public GameObject crystalPrefab;
    private GameObject currentCrystal;
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
                
                coolTimer = MulCrystalCoolDomn;
            }

        }
        
        if (canMulCrystal)
        {
            return;
        }
        
        if(currentCrystal == null)
        {
            currentCrystal = Instantiate(crystalPrefab, PlayerManager.instance.player.transform.position, transform.rotation);
            currentCrystal.GetComponent<Crystal_Skill_Controller>().SetupCrystal(growSpeed, canGrow, crystalDuration, attackForce, moveSpeed,FindClosestEnemy(currentCrystal.transform), canMove,damage);
            currentCrystal.GetComponent<Crystal_Skill_Controller>().Set_Idle();
        }
        else
        {

            if (canMove)
            {
                return;
            }
            coolTimer = coolDown;
            var position=player.transform.position;
            player.transform.position = currentCrystal.transform.position+returnOffset;
            currentCrystal.transform.position = position;
            //延迟一小段时间
            StartCoroutine(FinishTRansform());
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
}
