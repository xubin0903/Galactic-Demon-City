using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Clone_Skill : Skill
{
    [Header("Clone Skill Info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;
    [SerializeField] private bool canCreateOnCounterClone;
    [SerializeField] private bool canCreateOnDashStart;
     public void CreateClone(Transform target,Vector3 offset)
    {
        var position=target.position+offset;
        var newclone = Instantiate(clonePrefab, position, Quaternion.identity);
        newclone.GetComponent<Clone_Player_Controller>().SetupClone(cloneDuration,position,canAttack,damage);
    }
    public void CreateCloneOnCounterClone(Transform target, Vector3 offset)
    {
        if (canCreateOnCounterClone)
        {
           StartCoroutine(CreateOnCounterCloneDalay(target, offset));
        }
        
    }
    private IEnumerator CreateOnCounterCloneDalay(Transform target, Vector3 offset)
    {
        yield return new WaitForSeconds(.3f);
        CreateClone(target, offset);
    }
    public void CreateCloneOnDashSatrt(Transform target, Vector3 offset)
    {
        if (canCreateOnDashStart)
        {
            CreateClone(target, offset);
        }
    }
}
