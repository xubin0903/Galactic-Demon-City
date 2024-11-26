using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler
{
    public static event Action EnemyHealthUIEvent;//让敌人的血量UI不一起翻转
    public static void OnEnemyHealthUI()
    {
        EnemyHealthUIEvent?.Invoke();
    }
}  
