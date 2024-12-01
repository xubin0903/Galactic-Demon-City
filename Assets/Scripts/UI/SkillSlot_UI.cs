using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot_UI : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,ISaveManager
{
    public string skillName;
   
    public string skillDescription;
    public bool unlocked;
    public int price;
    [SerializeField] private SkillSlot_UI[] shoundBeUnlocked;
    [SerializeField] private SkillSlot_UI[] shoundBeLocked;
    private Image skillImage;
    [SerializeField] private Color lockedColor;
    private void OnValidate()
    {
        gameObject.name = "SkillSlot_UI "+skillName;
        
    }
   private void Awake()
    {
        skillImage = GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(() => UnLockSkillSlot());
    }
    private void Start()
    {
       if(unlocked == true)
        {
            skillImage.color = Color.white;

        }else
        skillImage.color = lockedColor;
    }
    public void UnLockSkillSlot()
    {
        if (unlocked == true)
        {
            return;
        }
        for(int i = 0; i < shoundBeUnlocked.Length; i++)
        {
            if (shoundBeUnlocked[i].unlocked == true)
            {
                Debug.Log("Unlocked");
                return;
            }
        }
        for(int i = 0; i < shoundBeLocked.Length; i++)
        {
            if (shoundBeLocked[i].unlocked == false)
            {
                Debug.Log("UnLocked");
                return;
            }
        }
        if (PlayerManager.instance.HvaeEnoughCurrency(price)==false)
        {
            Debug.Log("Not Enough Currency");
            return;

        }
        unlocked = true;
        Debug.Log("locked");
        skillImage.color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       UI.instance.skillToolTip.HideToolTip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UI.instance.skillToolTip.ShowDescription( skillDescription,skillName,price.ToString());
        float x = Input.mousePosition.x;
        float offsetX = 0;
        if(x > Screen.width / 2)
        {
            offsetX = -300;
        }
        else
        {
            offsetX = 300;
        }
        UI.instance.skillToolTip.transform.position = new Vector3(x + offsetX, UI.instance.skillToolTip.transform.position.y, UI.instance.skillToolTip.transform.position.z);
    }

    void ISaveManager.LoadData(GameData _gameData)
    {
        if(_gameData.SkillTree.TryGetValue(skillName,out bool value))
        {
            unlocked = value;
            
        }
    }

    void ISaveManager.SaveData(ref GameData _gameData)
    {
        if(_gameData.SkillTree.TryGetValue(skillName,out bool value))
        {
            _gameData.SkillTree.Remove(skillName);
            _gameData.SkillTree.Add(skillName,unlocked);
        }
        else
        {
            _gameData.SkillTree.Add(skillName,unlocked);
        }
    }
}
