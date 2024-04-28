using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioMixer masterMixer;

    public float MusicVolume
    {
        get
        {
            masterMixer.GetFloat("musicVol", out float musicVolume);
            return musicVolume;
        }
        set
        {
            float volume = value;
            if (volume == -40f)
                volume = -80f;
            masterMixer.SetFloat("musicVol", volume);
        }
    }

    public float EffectsVolume
    {
        get
        {
            masterMixer.GetFloat("sfxVol", out float effectsVolume);
            return effectsVolume;
        }
        set
        {
            float volume = value;
            if (volume == -40f)
                volume = -80f;
            masterMixer.SetFloat("sfxVol", volume);
        }
    }

    public bool IsSoundOn
    {
        get
        {
            masterMixer.GetFloat("masterVol", out float volume);
            return volume == 0f;
        }
        set
        {
            masterMixer.SetFloat("masterVol", value ? 0f : -80f);
        }
    }
}
