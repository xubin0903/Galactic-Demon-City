using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private float decreasingSpeed;
    public void Setup(Sprite _sprite, float _decreasingSpeed)
    {
        sr=GetComponent<SpriteRenderer>();
        sr.sprite = _sprite;
        decreasingSpeed = _decreasingSpeed;
    }

    private void Update()
    {
        if (sr.color.a > 0)
        {
            float alpha = decreasingSpeed*Time.deltaTime;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a - alpha);
        }
        if(sr.color.a <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
