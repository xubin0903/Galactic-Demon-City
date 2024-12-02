using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private string fileName;
    public static SaveManager instance;
    public List<ISaveManager> saveManagers;
    private FileDataHandler fileDataHandler;
    public GameData gameData;
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    [ContextMenu("Delete Data")]
    public void DeleteData()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        fileDataHandler.DeleteData();
    }
    private void Start()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath,fileName);
        saveManagers = FindAllSaveManagers();
        LoadGame();
    }
    public void SaveGame()
    {
     
        foreach(ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }
        fileDataHandler.SaveData(gameData);
       
    }

    
    public void NewGame()
    {
       
        gameData = new GameData();
       
    }
    public void Update()
    {
       
    }
    public void LoadGame()
    {
        gameData=fileDataHandler.LoadData();
        if(this.gameData!= null)
        {
            
            foreach(ISaveManager saveManager in saveManagers)
            {
                saveManager.LoadData(gameData);
            }
           
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
    private List<ISaveManager> FindAllSaveManagers()
    {
       // 用于查找场景中所有实现ISaveManager接口的MonoBehaviour对象，并将其存储在IEnumerable<ISaveManager>类型的集合中。
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveManager>();//加上true是为了查找未激活的

        return new List<ISaveManager>(saveManagers);
    }
    public bool HasSaveData()
    {
        if (fileDataHandler.LoadData() != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
