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
        StartCoroutine(HurtFX(hurtTime));
    }
    private IEnumerator HurtFX(float time)
    {
        sr.material = hurtMaterial;
        yield return new WaitForSeconds(time);
        sr.material = originalMaterial;
    }
}
