using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameover : MonoBehaviour
{
    void Start()
    {
        Transition.ResetTransition();
        GameController.SetUpdateTimer(false);
        Transition.StartFadeIn();

        if (FindObjectOfType<SoundManager>() != null)
        {
            FindObjectOfType<SoundManager>().PlayMusic("Main Menu");
        }
    }

    private void Update()
    {
        if (Transition.s_overlayBlack == true)
        {
            GameController.LoadFirstLevel();
        }
    }

    public void Restart()
    {
        Transition.s_levelCounter = 0;
        Transition.StartFadeOut();
    }

    public void MainMenu()
    {
        Transition.StartFadeOut();
        GameController.LoadScene("MainMenu");
    }
}
