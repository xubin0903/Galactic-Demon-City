using System.Collections;
using TMPro;
using UnityEngine;

public class UI_Pop : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public float lifeTime;
    private float time;
   
    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
       
    }
    private void Update()
    {
        
      

           

        
    }

public void FadeOutText() => StartCoroutine(CorountineFadeOutText(textMesh));

    public IEnumerator CorountineFadeOutText(TextMeshProUGUI textMeshPro)
    {
        float duration = lifeTime; // ������ʧ��ʱ��
        float startAlpha = textMeshPro.color.a;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            Color newColor = textMeshPro.color;
            newColor.a = Mathf.Lerp(startAlpha, 0, time / duration);
            textMeshPro.color = newColor;
            yield return null;
        }

        // ȷ������ֵΪ 0
        Color finalColor = textMeshPro.color;
        finalColor.a = 0;
        textMeshPro.color = finalColor;

    }
}
