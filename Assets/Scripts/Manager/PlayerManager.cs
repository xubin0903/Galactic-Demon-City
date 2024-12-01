using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour,ISaveManager
{
    public static PlayerManager instance;
    public int currency;
    public Player player;
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
    public void AddCurrency(int amount)
    {
        currency += amount;
    }
    public bool HvaeEnoughCurrency(int amount)
    {
        if (currency >= amount)
        {
            currency -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }

    void ISaveManager.LoadData(GameData _gameData)
    {
       this.currency = _gameData.currentcy;
    }

    void ISaveManager.SaveData(ref GameData _gameData)
    {
        
       _gameData.currentcy = this.currency;
    }
}
