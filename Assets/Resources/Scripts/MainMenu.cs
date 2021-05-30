using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public AudioMixer m_mixer;

    void Start()
    {
        GameController.SetUpdateTimer(false);
        Transition.StartFadeIn();
        SetBaseVolume();

        if (FindObjectOfType<SoundManager>() != null)
        {
            FindObjectOfType<SoundManager>().PlayMusic("Main Menu");
        }
    }

    void SetBaseVolume()
    {
        m_mixer.SetFloat("MusicVol", Mathf.Log10(SoundManager.s_musicVolume) * 20);
        m_mixer.SetFloat("SFXVol", Mathf.Log10(SoundManager.s_sfxVolume) * 20);
    }

    private void Update()
    {
        if (Transition.s_overlayBlack == true)
        {
            GameController.LoadFirstLevel();
        }
    }

    public void GameStart()
    {
        Transition.s_levelCounter = 0;
        Transition.StartFadeOut();
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
