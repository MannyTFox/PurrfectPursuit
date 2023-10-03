using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] AudioMixer mixerMaster;
    [SerializeField] Slider masterVolSlider;
    [SerializeField] Slider musicVolSlider;
    [SerializeField] Slider sfxVolSlider;

    private void Start()
    {
        // Choose whether to load volume if prefs were created or set normally
        if (PlayerPrefs.HasKey(Prefs.MasterVolume))
        {
            LoadMasterVolume();
        }
        else
        {
            SetMasterVolumeValue();
        }

        if (PlayerPrefs.HasKey(Prefs.MusicVolume))
        {
            LoadMusicVolume();
        }
        else
        {
            SetMusicVolumeValue();
        }

        if (PlayerPrefs.HasKey(Prefs.SFXVolume))
        {
            LoadSFXVolume();
        }
        else
        {
            SetSFXVolumeValue();
        }
    }

    public void SetMasterVolumeValue()
    {
        float volume = masterVolSlider.value;

        mixerMaster.SetFloat("masterVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(Prefs.MasterVolume, volume);
    }

    public void LoadMasterVolume()
    {
        masterVolSlider.value = PlayerPrefs.GetFloat(Prefs.MasterVolume);

        SetMasterVolumeValue();
    }

    public void SetMusicVolumeValue()
    {
        float volume = musicVolSlider.value;

        mixerMaster.SetFloat("musicVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(Prefs.MusicVolume, volume);
    }

    public void LoadMusicVolume()
    {
        musicVolSlider.value = PlayerPrefs.GetFloat(Prefs.MusicVolume);

        SetMusicVolumeValue();
    }

    public void SetSFXVolumeValue()
    {
        float volume = sfxVolSlider.value;

        mixerMaster.SetFloat("sfxVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(Prefs.SFXVolume, volume);
    }

    public void LoadSFXVolume()
    {
        sfxVolSlider.value = PlayerPrefs.GetFloat(Prefs.SFXVolume);

        SetSFXVolumeValue();
    }
}
