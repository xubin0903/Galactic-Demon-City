using Cinemachine;
using System.Collections;
using TMPro;
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
    public ParticleSystem fireParticle;
    public ParticleSystem lightnedParticle; 
    public ParticleSystem iceParticle;
    public ParticleSystem DustFX;
    public GameObject interactionButton;
    [Header("Cinemachine")]
    public CinemachineImpulseSource screenShake;
    [SerializeField] private float multiplier;
    public Vector3 swordShakePower;
    public Vector3 highDamageShakePower;
    [Header("PopToolTip")]
    public GameObject popToolTipPrefab;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        screenShake = GetComponent<CinemachineImpulseSource>();
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
    public void ScreenShake(Vector3 shakePower)
    {
        screenShake.m_DefaultVelocity =new  Vector3(shakePower.x*PlayerManager.instance.player.faceDir,shakePower.y)*multiplier;
        screenShake.GenerateImpulse();
    }
    public void GeneratePopToolTip(string text)
    {
        int randomX = Random.Range(-1, 2);
        int randomY = Random.Range(1, 3);
        Vector3 position = transform.position + new Vector3(randomX, randomY, 0);
        GameObject newPop=Instantiate(popToolTipPrefab, position, Quaternion.identity);
        newPop.GetComponent<TextMeshPro>().text = text;
    
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
        StopFire();
        StopLightned();
        StopIce();
    
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
        Fire();
        
    }
    public void LightnedColorFor(float time)
    {
        //Debug.Log("LightnedColorFor");
        InvokeRepeating("LightnedColorBlink", 0, 0.2f);
        Invoke("CancelColorBlink", time);
        Lightned();
    }
    public void IcedColorFor(float time)
    {
        //Debug.Log("IcedColorFor");
        InvokeRepeating("IcedColorBlink", 0,0.1f);
        Invoke("CancelColorBlink", time);
        Ice();
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
    public void Fire()=> fireParticle.Play();
    public void Lightned()=> lightnedParticle.Play();
    public void Ice()=> iceParticle.Play();
    public void StopFire()=> fireParticle.Stop();
    public void StopLightned()=> lightnedParticle.Stop();
    public void StopIce()=> iceParticle.Stop();
    public void Dust()=> DustFX.Play();
    public void OpenInteractionButton()=> interactionButton.SetActive(true);
    public void CloseInteractionButton()=> interactionButton.SetActive(false);
}
