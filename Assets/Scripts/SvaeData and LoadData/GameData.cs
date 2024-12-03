using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameData 
{
    public int currentcy;
    public SerializableDictionary<string, int> Inventory;
    public SerializableDictionary<string, bool> SkillTree;
    public SerializableDictionary<string, bool> checkPoints;
    public string cloestCheckPointID;
    public float lostCurrentcyX;
    public float lostCurrentcyY;
    public int lostCurrentcyAmount;
    public float currentHealth;
    public  SerializableDictionary<string, float> volumeSettings;
    public  GameData()
    {
        currentcy = 0;
        Inventory = new SerializableDictionary<string, int>();
        SkillTree = new SerializableDictionary<string, bool>();
        checkPoints = new SerializableDictionary<string, bool>();
        cloestCheckPointID = null;
        lostCurrentcyX = 0;
        lostCurrentcyY = 0;
        lostCurrentcyAmount = 0;
        volumeSettings = new SerializableDictionary<string, float>();
        

    }

}
