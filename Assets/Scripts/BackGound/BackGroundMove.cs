using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMove : MonoBehaviour
{
    private GameObject cam;
    [SerializeField] private float speed;
    private float xPosition;
    private float length;
    void Start()
    {
        cam=GameObject.Find("Virtual Camera");
        xPosition=transform.position.x;
        length=GetComponent<Renderer>().bounds.size.x;
    }

    
    void Update()
    {
        float distanceMoved = cam.transform.position.x * (1 - speed);
        float distance = cam.transform.position.x * speed;
        transform.position=new Vector3(xPosition+distance,transform.position.y);
        if (distanceMoved > length + xPosition)
        {
            xPosition += length;
        }
        else if(distanceMoved < xPosition - length)
        {
            xPosition -= length;
        }
    }
}
