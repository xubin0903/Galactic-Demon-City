using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
[CreateAssetMenu(fileName = "GameSceneSo", menuName = "GameScene/GameSceneSo")]
public class GameSceneSo : ScriptableObject
{
    public AssetReference sceneRef;
    public string sceneName;
}
