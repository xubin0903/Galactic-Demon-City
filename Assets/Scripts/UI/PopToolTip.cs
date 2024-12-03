using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopToolTip : MonoBehaviour
{
    private TextMeshPro textMesh;
    public float lifeTime;
    private float time;
    public float colorChangeSpeed;
    public float decreaseSpeed;
    public float speed;
    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        time = 0;
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time >= lifeTime)
        {
           float alpha=textMesh.color.a-colorChangeSpeed*Time.deltaTime;
            textMesh.color=new Color(textMesh.color.r,textMesh.color.g,textMesh.color.b,alpha);
            if (alpha <= 50)
            {
                speed = decreaseSpeed;
            }
            if (alpha <= 0)
                Destroy(gameObject);
        }
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y+1, transform.position.z ),speed*Time.deltaTime);
        
    }
}
