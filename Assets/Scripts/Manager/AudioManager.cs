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
    private bool canPlaySFX;
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
    private void Start()
    {
        Invoke("CanPlaySFX", 2f);
    }
    public void Update()
    {
        if (!isPlay)
        {
            RandomPlayBGM();
        }
        if (bgm[bgmIndex].isPlaying == false)
        {
            isPlay = false;
        }
    }

    public void PlaySFX(int index,Transform _source)
    {
        if (!canPlaySFX)
            return;
        float volume = sfx[index].volume;
        if(_source != null)
        {
        //根据距离远近调整音效大小
            float distance = Vector3.Distance(_source.position, PlayerManager.instance.player.transform.position);
            if(distance > 20f)
            {
                volume = 0f;
            }
            else if (distance > 15f)
            {
                volume = 0.2f;
            }
            else if (distance > 10f)
            {
                volume = 0.5f;
            }
            else if (distance > 5f)
            {
                volume = 0.7f;
            }
            
            else
            {
                volume = 1f;
            }

        }
        
        sfx[index].volume = volume;
        if(index < sfx.Length)
        {
            if(sfx[index].isPlaying == true)
                return;
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
            isPlay = true;
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
    private void CanPlaySFX()=> canPlaySFX = true;
}
