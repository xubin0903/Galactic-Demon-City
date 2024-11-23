using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private Material hurtMaterial;
    [SerializeField] private Material originalMaterial;
    [SerializeField] private float hurtTime;
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
        sr.material = hurtMaterial;
        yield return new WaitForSeconds(time);
        sr.material = originalMaterial;
    }
    public void RedColorBlink()
    {
        if (sr.color != Color.white)
        {
            sr.color = Color.white;
            Debug.Log("white");
        }
        else
        {
            Debug.Log("Red");
            sr.color = Color.red;
        }
    }
    public void CancelRedColorBlink()
    {
        //ȡ��
        CancelInvoke();
        Debug.Log("cancel");
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
}
