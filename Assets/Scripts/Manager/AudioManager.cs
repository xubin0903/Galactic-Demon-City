using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource[] sfx;
    public AudioSource[] bgm;
    public bool isPlay;
    private int bgmIndex;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Update()
    {
        if (!isPlay)
        {
            StopAllBGM();
        }
        else
        {
            if(bgm[bgmIndex].isPlaying == false)
            {
                PlayBGM(bgmIndex);
            }
        }
    }

    public void PlaySFX(int index)
    {
        if(index < sfx.Length)
        {
            sfx[index].pitch=Random.Range(0.8f,1.2f);//随机播放音效减少音效爆炸
            sfx[index].Play();

        }
    }

    public void PlayBGM(int index)
    {
        StopAllBGM();
        if (index < bgm.Length)
        {
            bgmIndex = index;
            bgm[index].Play();

        }
    }
    private void RandomPlayBGM()
    {
        int index = Random.Range(0, bgm.Length);
        PlayBGM(index);
    }
    public void StopBGM(int index)
    {
        if (index < bgm.Length)
            bgm[index].Stop();
    }
    public void StopAllBGM()
    {
        foreach (AudioSource bgm in bgm)
        {
            bgm.Stop();
        }
    }
    public void StopSFX(int index)
    {
        if (index < sfx.Length)
        {
            sfx[index].Stop();
        }
    }
}
