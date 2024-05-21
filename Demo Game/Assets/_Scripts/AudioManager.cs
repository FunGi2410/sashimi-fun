using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    public AudioClip bgMusic;
    public AudioClip cashRegister;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetMusicVolume();
        this.musicSource.clip = bgMusic;
        this.musicSource.Play();
    }

    public void PlayerSFX(AudioClip clip)
    {
        this.sfxSource.PlayOneShot(clip);
    }

    public void SetMusicVolume()
    {
        musicSource.volume = musicSlider.value;
    }
    public void SetSFxVolume()
    {
        sfxSource.volume = sfxSlider.value;
    }

}
