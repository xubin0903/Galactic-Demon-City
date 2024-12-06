using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
   public GameSceneSo firstScene;
    public FadeScreen_UI fadeScreen;
    public static LoadManager instance;
    public GameSceneSo currentScene;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
       currentScene = firstScene;
       currentScene.sceneRef.LoadSceneAsync(LoadSceneMode.Additive);

       
    }
    private void Start()
    {
    }
    public void Transition(GameSceneSo scene)
    {
        StartCoroutine(LoadScene(scene));
    }
    private IEnumerator LoadScene(GameSceneSo scene)
    {
        fadeScreen.FadeOut();
       
        if (currentScene!= null)
        {
            yield return currentScene.sceneRef.UnLoadScene();
        }
        yield return new WaitForSeconds(2);
        currentScene=scene;
        currentScene.sceneRef.LoadSceneAsync(LoadSceneMode.Additive);
        yield return new WaitForSeconds(2);
      
        Debug.Log("Scene Loaded" + currentScene.sceneName);
        for(int i = 0; i < SceneManager.sceneCount; i++)
        {
            Debug.Log("Sjab");
            Debug.Log(SceneManager.GetSceneAt(i).name);
            Scene loadedScene = SceneManager.GetSceneAt(i);
            if (scene.name == currentScene.sceneName)
            {
                SceneManager.SetActiveScene(loadedScene);
            }
        }
        Debug.Log("Scene Activated");
        fadeScreen.FadeIn();
      

    }
}
