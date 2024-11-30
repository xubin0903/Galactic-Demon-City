using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public GameData gameData;
    private void Start()
    {
        LoadGame();
    }
    public void SaveGame()
    {
        Debug.Log("Saving game data");
    }

    
    public void NewGame()
    {
        Debug.Log("Creating new game");
        gameData = new GameData();
    }
    public void LoadGame()
    {
        if(gameData!= null)
        {
            Debug.Log("Loading game data");
        }
        else
        {
            NewGame();
        }
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
