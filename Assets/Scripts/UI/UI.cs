using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance;
    public UI_Item_ToolTip itemToolTip;
    public GameObject Character;
    public GameObject SkillTree;
    public GameObject Craft;
    public GameObject Options;
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
    }
    // Start is called before the first frame update
    void Start()
    {
        SwitchTo(null);
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
            return;
        }
        SwitchTo(_menu);
       
    }
  
}
