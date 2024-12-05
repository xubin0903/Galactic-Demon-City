using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour,ISaveManager
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
    public FadeScreen_UI fadeScreen;
    public GameObject youDiedText;
    public GameObject restartButton;
    public VolumeSlide_UI[] volumeSliders;
    public GameSceneSo mainMenuScene;
   
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

        if (Input.GetKeyDown(KeyCode.V))
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
           if(transform.GetChild(i).gameObject.GetComponent<FadeScreen_UI>()==null)
            transform.GetChild(i).gameObject.SetActive(false);
        }
        AudioManager.instance.PlaySFX(7, null);
        if(GameManager.instance!= null)
        {
            if(_menu==null||_menu==InGameUI)
            {
                GameManager.instance.PauseGame(false);
                
            }
            else
            {
                GameManager.instance.PauseGame(true);
            }
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
        AudioManager.instance.PlaySFX(7, null);
        CheckInGameUI();
    }
    private void CheckInGameUI()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf&&transform.GetChild(i).gameObject.GetComponent<FadeScreen_UI>() == null)
            {
                return;
            }
        }
        SwitchTo(InGameUI);
    }
    public void ShowYouDiedText()
    {
        fadeScreen.FadeOut();
        StartCoroutine(ShowYouDiedTextCoroutine(1.5f));
    }
    private IEnumerator ShowYouDiedTextCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        AudioManager.instance.PlaySFX(11, null);
        youDiedText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        restartButton.SetActive(true);
       
    }

    void ISaveManager.LoadData(GameData _gameData)
    {
       foreach(KeyValuePair<string, float> volumeSetting in _gameData.volumeSettings)
        {
            foreach(VolumeSlide_UI volumeSlider in volumeSliders)
            {
                if(volumeSlider.parameterName == volumeSetting.Key)
                {
                    volumeSlider.slider.value = volumeSetting.Value;
                    volumeSlider.LoadVolume(volumeSetting.Value);
                }
            }
        }
    }

    void ISaveManager.SaveData(ref GameData _gameData)
    {
        _gameData.volumeSettings.Clear();
        foreach(VolumeSlide_UI volumeSlider in volumeSliders)
        {
            _gameData.volumeSettings.Add(volumeSlider.parameterName, volumeSlider.slider.value);
        }
    }
    public void SaveAndQuit()
    {
        SaveManager.instance.SaveGame();
        Time.timeScale = 1;


        LoadManager.instance.Transition(mainMenuScene);
    }
    
}
