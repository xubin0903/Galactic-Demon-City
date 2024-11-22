using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill : Skill
{
    public GameObject crystalPrefab;
    private GameObject currentCrystal;
    [SerializeField] private Vector3 returnOffset;
   public override void UseSkill()
    {
        
        if(currentCrystal == null)
        {
            currentCrystal = Instantiate(crystalPrefab, PlayerManager.instance.player.transform.position, transform.rotation);
            currentCrystal.GetComponent<Crystal_Skill_Controller>().Set_Idle();
        }
        else
        {
            coolTimer = coolDown;
            player.transform.position = currentCrystal.transform.position+returnOffset;

            currentCrystal.GetComponent<Crystal_Skill_Controller>().Set_Explode();
            currentCrystal = null;

        }

    }
}
