using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler
{
    public static event Action EnemyHealthUIEvent;//�õ��˵�Ѫ��UI��һ��ת
    public static void OnEnemyHealthUI()
    {
        EnemyHealthUIEvent?.Invoke();
    }
}  
