using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AdjustVolume : MonoBehaviour
{
    public AudioMixer m_mixer;
    public string m_volName;
    public Slider m_slider;

    void Start()
    {
        Initialise();
    }

    public void Initialise()
    {
        if (m_volName == "MusicVol")
        {
            m_slider.value = SoundManager.s_musicVolume;
        }
        else
        {
            m_slider.value = SoundManager.s_sfxVolume;
        }

        SetLevel();
    }

    public void SetLevel()
    {
        float sliderValue = m_slider.value;
        m_mixer.SetFloat(m_volName, Mathf.Log10(sliderValue) * 20);

        if (m_volName == "MusicVol")
        {
            SoundManager.s_musicVolume = sliderValue;
        }
        else
        {
            SoundManager.s_sfxVolume = sliderValue;
        }
    }
}
