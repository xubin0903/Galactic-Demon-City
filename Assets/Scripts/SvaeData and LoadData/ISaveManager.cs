using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveManager
{
    void LoadData(GameData _gameData);
    void SaveData( ref GameData _gameData);
}
