using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private Material hurtMaterial;
    private Material originalMaterial;
    [SerializeField] private float hurtTime;
    [SerializeField] private Color icedColor;
    [SerializeField] private Color[] FiredColor;
    [SerializeField] private Color[] LightnedColor;
    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }
    private void Start()
    {
        originalMaterial = sr.material;
    }
    public void Hurt()
    {
        //Debug.Log("HurtFX");
        StartCoroutine(HurtFX(hurtTime));
    }
    private IEnumerator HurtFX(float time)
    {
        Color color=sr.color;
        sr.material = hurtMaterial;
        sr.color = Color.white;
        yield return new WaitForSeconds(time);
        sr.color = color;
        sr.material = originalMaterial;
    }
    public void RedColorBlink()
    {
        if (sr.color != Color.white)
        {
            sr.color = Color.white;
            //Debug.Log("white");
        }
        else
        {
            //Debug.Log("Red");
            sr.color = Color.red;
        }
    }
    public void CancelColorBlink()
    {
        //È¡Ïû
        CancelInvoke();
        //Debug.Log("cancel");
        sr.color = Color.white;
    }
    public void TransParent(bool isParent)
    {
        if (isParent)
        {
           sr.color = Color.clear;
        }
        else
        {
            sr.color = Color.white;
        }
    }
    public void FiredColorFor(float time)
    {
        //Debug.Log("FiredColorFor");
        InvokeRepeating("FiredColorBlink", 0, 0.2f);
        Invoke("CancelColorBlink", time);
    }
    public void LightnedColorFor(float time)
    {
        //Debug.Log("LightnedColorFor");
        InvokeRepeating("LightnedColorBlink", 0, 0.2f);
        Invoke("CancelColorBlink", time);
    }
    public void IcedColorFor(float time)
    {
        //Debug.Log("IcedColorFor");
        InvokeRepeating("IcedColorBlink", 0,0.1f);
        Invoke("CancelColorBlink", time);
    }
    private void LightnedColorBlink()
    {
        //Debug.Log("LightnedColorBlink");
        if (sr.color!= LightnedColor[0])
        {
            sr.color = LightnedColor[0];
           //Debug.Log("LightnedColor[0]");
        }
        else
        {
            //Debug.Log("LightnedColor[1]");
            sr.color = LightnedColor[1];
        }
    }
    private void FiredColorBlink()
    {
        if(sr.color!= FiredColor[0])
        {
            sr.color = FiredColor[0];
           
        }
        else
        {
            sr.color = FiredColor[1];
        }
    }
    private void IcedColorBlink()
    {
        sr.color = icedColor;
    }
}
