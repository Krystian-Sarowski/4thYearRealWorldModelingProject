using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject m_pauseMenu;
    public GameObject m_helpMenu;
    public GameObject m_optionsMenu;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if(GameController.s_gameState != GameState.Paused)
            {
                Debug.Log("The game has paused");
                GameController.PauseGame();
                m_pauseMenu.SetActive(true);

                if (FindObjectOfType<SoundManager>() != null)
                {
                    FindObjectOfType<SoundManager>().PauseAllSoundEffects();
                }
            }

            else
            {
                if (!m_helpMenu.activeSelf && !m_optionsMenu.activeSelf)
                {
                    Unpause();
                }
            }
        }
    }

    public void Unpause()
    {
        GameController.UnpauseGame();
        m_pauseMenu.SetActive(false);
        m_optionsMenu.SetActive(false);
        FindObjectOfType<SoundManager>().UnpauseAllSoundEffects();
    }

    public void Menu()
    {
        Unpause();
        GameController.LoadScene("MainMenu");
    }
}
