using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUI : MonoBehaviour
{
    private Enemy enemy;
    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }
    private void OnEnable()
    {
      enemy.OnHeathUIUpdate += OnDontFlip;
    }
   private void OnDisable()
    {
       enemy.OnHeathUIUpdate -=OnDontFlip;
    }

    private void OnDontFlip()
    {
        transform.Rotate(0, 0, 180);
    }
}
