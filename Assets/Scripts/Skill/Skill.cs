using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float coolDown;
    protected float coolTimer;
    protected Player player;
    protected  virtual void Start()
    {
        player=PlayerManager.instance.player;
    }
    public virtual void Update()
    {
        coolTimer -= Time.deltaTime;
    }
    public virtual bool CanUseSkill()
    {
        if(coolTimer <= 0)
        {
            UseSkill();
            return true;
        }
        else
        {
            return false;
        }
    }
    public virtual void UseSkill()
    {
        coolTimer = coolDown;
    }
}
