using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_UI : MonoBehaviour
{
    [SerializeField] private string sceneName;
    public GameObject continueButton;
    public FadeScreen_UI fadeScreen;
    private void Start()
    {
        if (SaveManager.instance.HasSaveData() == false)
        {
            continueButton.SetActive(false);
        }
    }
    public void ContinueGame()
    {
        StartCoroutine(FadeToScene(1.5f));
    }
    public void NewGame()
    {
        SaveManager.instance.DeleteData();
        StartCoroutine(FadeToScene(1.5f));
    }
    public void QuitGame()
    {
        Application.Quit(); 
    }
    private IEnumerator FadeToScene(float delay)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
    public void Open(GameObject _menu)
    {
        _menu.SetActive(true);
    }
    public void Close(GameObject _menu)
    {
        _menu.SetActive(false);
    }
}
