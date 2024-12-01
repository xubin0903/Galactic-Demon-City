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
       // ���ڲ��ҳ���������ʵ��ISaveManager�ӿڵ�MonoBehaviour���󣬲�����洢��IEnumerable<ISaveManager>���͵ļ����С�
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveManager>();//����true��Ϊ�˲���δ�����

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
