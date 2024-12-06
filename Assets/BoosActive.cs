using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosActive : MonoBehaviour
{
    public GameObject Boos;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>()!= null&&Boos.activeSelf == false)
        {
            Boos.SetActive(true);
        }
    }
    
}
