using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("Clone Skill Info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;
     public void CreateClone(Transform target,Vector3 offset)
    {
        target.position += offset;
        var newclone = Instantiate(clonePrefab, target.position, Quaternion.identity);
        newclone.GetComponent<Clone_Player_Controller>().SetupClone(cloneDuration,target,canAttack);
    }
}
