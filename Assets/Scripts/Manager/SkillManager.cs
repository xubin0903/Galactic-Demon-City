using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public Dash_Skill dash_skill { get; private set; }
    public Clone_Skill clone { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log("SkillManager instance created");
        }
        else
        {
            Destroy(this.gameObject);
        }
        dash_skill = GetComponent<Dash_Skill>();
        clone = GetComponent<Clone_Skill>();
    }
}
