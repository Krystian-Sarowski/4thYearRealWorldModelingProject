using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour
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

    public void PlayAgain()
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
