using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance;
    public UI_Item_ToolTip itemToolTip;
    public UI_Skill_ToolTip skillToolTip;
    public GameObject Character;
    public GameObject SkillTree;
    public GameObject Craft;
    public GameObject Options;
    public GameObject InGameUI;
    public UI_CraftWindow craftWindow;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
       SkillTree.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        SwitchTo(InGameUI);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchWithKey(Character);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SwitchWithKey(SkillTree);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            SwitchWithKey(Craft);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SwitchWithKey(Options);
        }
    }
    public void SwitchTo(GameObject _menu)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        if (_menu == null)
        {
            return;
        }
        _menu.SetActive(true);
        Inventory.instance.UpdateSlotUI();
    }
    public void SwitchWithKey(GameObject _menu)

    {
        if(_menu!= null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            CheckInGameUI();
            return;
        }
        SwitchTo(_menu);
       
    }
   public void TurnOffAll()
    {
        SkillTree.SetActive(false);
        Character.SetActive(false);
        Craft.SetActive(false);
        Options.SetActive(false);
        CheckInGameUI();
    }
    private void CheckInGameUI()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                return;
            }
        }
        SwitchTo(InGameUI);
    }
  
}
