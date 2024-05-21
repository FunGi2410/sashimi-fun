using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSetting : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;

        //audioMixer.SetFloat("music", Mathf.Log10(volume) * 20);
    }


}
