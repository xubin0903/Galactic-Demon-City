using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUI : MonoBehaviour
{
    private void OnEnable()
    {
        EventHandler.EnemyHealthUIEvent+=OnDontFlip;
    }
   private void OnDisable()
    {
        EventHandler.EnemyHealthUIEvent-=OnDontFlip;
    }

    private void OnDontFlip()
    {
        transform.Rotate(0, 0, 180);
    }
}
