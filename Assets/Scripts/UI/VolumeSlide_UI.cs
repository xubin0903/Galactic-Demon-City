using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlide_UI : MonoBehaviour
{
   public Slider slider;
    public string parameterName;
    [SerializeField] private AudioMixer audioMixer;
    public void SetVolume(float value)
    {
        float volume = value*100-80;
      
        audioMixer.SetFloat(parameterName, volume);
    }
    public void LoadVolume(float sliderValue)
    {
        float volume = sliderValue * 100 - 80;
       
        audioMixer.SetFloat(parameterName, volume);
    }
}
