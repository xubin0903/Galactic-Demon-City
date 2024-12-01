using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour,ISaveManager
{
    public static GameManager instance;
    public CheckPoint[] checkPoints;
    public string cloestCheckPointID;
    public float lostCurrentcyX;
    public float lostCurrentcyY;
    public int lostCurrentcyAmount;
    public GameObject lostCurrentcy;
    public GameObject currentLostCurrentcy;
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
        checkPoints = FindObjectsOfType<CheckPoint>(true);

    }
    private void Start()
    {
       
    }
    public void RestartGame()
    {
        SaveManager.instance.SaveGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void ISaveManager.LoadData(GameData _gameData)
    {

        foreach (KeyValuePair<string, bool> pair in _gameData.checkPoints)
        {
            foreach (CheckPoint _checkPoint in checkPoints)
            {
                if (_checkPoint.checkPointID == pair.Key && pair.Value == true)
                {
                    _checkPoint.Activate();
                }
            }
        }


        cloestCheckPointID = _gameData.cloestCheckPointID;

        Invoke("PlacePlayerCloseCheckPoint", .1f);
        CreateLostCurrentcy(_gameData);

    }

    private void CreateLostCurrentcy(GameData _gameData)
    {
        if (_gameData.lostCurrentcyAmount <= 0)//no lost currentcy
        {
            return;
        }
        this.lostCurrentcyAmount = _gameData.lostCurrentcyAmount;
        this.lostCurrentcyX = _gameData.lostCurrentcyX;
        this.lostCurrentcyY = _gameData.lostCurrentcyY;
        GameObject newLostCurrentcy = Instantiate(lostCurrentcy, new Vector3(this.lostCurrentcyX, this.lostCurrentcyY, 0), Quaternion.identity);
        newLostCurrentcy.GetComponent<LostCurrentcy>().currentcy = this.lostCurrentcyAmount;
        this.currentLostCurrentcy = newLostCurrentcy;
    }

    private void PlacePlayerCloseCheckPoint()
    {
        Debug.Log("PlacePlayerCloseCheckPoint");
        foreach (CheckPoint checkPoint in checkPoints)
        {
            if(cloestCheckPointID!= null&& checkPoint.checkPointID==cloestCheckPointID){
                Debug.Log("Player Close to CheckPoint");
                PlayerManager.instance.player.transform.position = checkPoint.transform.position+Vector3.up*3;
            }
        }
    }

    void ISaveManager.SaveData(ref GameData _gameData)
    {
        Debug.Log("GameManager SaveData");
        _gameData.checkPoints.Clear();
       
        _gameData.cloestCheckPointID = FindClosetCheckPoint();
        Debug.Log(_gameData.cloestCheckPointID);
        foreach(CheckPoint checkPoint in checkPoints)
        {
            _gameData.checkPoints.Add(checkPoint.checkPointID,checkPoint.active);
        }
        if (currentLostCurrentcy == null&&PlayerManager.instance.player.stats.currentHealth<=0)
        {
            _gameData.lostCurrentcyAmount = PlayerManager.instance.currency;
            PlayerManager.instance.currency = 0;
            if (_gameData.currentcy != 0)
            {
                _gameData.currentcy = 0;//reset currentcy
            }
            _gameData.lostCurrentcyX = PlayerManager.instance.player.transform.position.x;
            _gameData.lostCurrentcyY = PlayerManager.instance.player.transform.position.y;
        }
        else
        {
            _gameData.lostCurrentcyAmount = 0;
            _gameData.lostCurrentcyX = 0;
            _gameData.lostCurrentcyY = 0;
        }

    }
    public string FindClosetCheckPoint()
    {
        float minDistance = float.MaxValue;
        CheckPoint closestCheckPoint = null;
        foreach (CheckPoint checkPoint in checkPoints)
        {
            if (checkPoint.active)
            {
                float distance = Vector3.Distance(PlayerManager.instance.player.transform.position, checkPoint.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestCheckPoint = checkPoint;
                }
            }
        }
        if (closestCheckPoint == null)
        {
            Debug.LogError("No Active CheckPoint");
        }
        return closestCheckPoint.checkPointID;
    }
}
