using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler 
{
    public static event Action EnemyHealthUIEvent;
    public static void OnEnemyHealthUI()
    {
         EnemyHealthUIEvent?.Invoke();
    }
}
