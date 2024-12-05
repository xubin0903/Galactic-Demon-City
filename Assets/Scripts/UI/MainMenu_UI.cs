using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_UI : MonoBehaviour
{
    [SerializeField] private string sceneName;
    public GameObject continueButton;
    public GameSceneSo gameSceneToLoadSo;
    
  
    private void Start()
    {
        if (SaveManager.instance.HasSaveData() == false)
        {
            continueButton.SetActive(false);
        }
        //fadeScreen.FadeIn();
    }
    public void ContinueGame()
    {
        LoadManager.instance.Transition(gameSceneToLoadSo);
    }
    public void NewGame()
    {

        SaveManager.instance.DeleteData();
        LoadManager.instance.Transition(gameSceneToLoadSo);
    }
    public void QuitGame()
    {
        Application.Quit(); 
    }
    public void OpenText(GameObject obj)=> obj.SetActive(true);
    public void CloseText(GameObject obj)=> obj.SetActive(false);
}
