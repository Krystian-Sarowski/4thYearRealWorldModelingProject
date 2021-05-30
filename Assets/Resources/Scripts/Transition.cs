using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    public static Transition s_instance;

    public static Image s_image;
    public static Text s_levelText;
    public static int s_levelCounter = 0;
    public static bool s_fadeInStarted = false;
    public static bool s_fadeOutStarted = false;
    public static bool s_overlayClear = false;
    public static bool s_overlayBlack = false;
    public static bool s_textDone = false;
    public static bool s_showText = true;
    public static bool s_playerDied = false;

    private static float s_timer;
    private static float s_duration = 1.0f;


    public void Awake()
    {
        s_instance = this;

        GameObject go = s_instance.transform.Find("Overlay").gameObject;
        s_image = go.GetComponentInChildren<Image>();
        s_image.color = Color.black;

        go = s_instance.transform.Find("Level Text").gameObject;
        s_levelText = go.GetComponentInChildren<Text>();
    }

    private void Update()
    {
        if (GameController.s_currentLevel != 0)
        {
            s_levelText.text = "Level " + s_levelCounter;
        }

        if (s_fadeOutStarted)
        {
            s_overlayClear = false;

            if (!s_playerDied)
            {
                if (!s_showText)
                {
                    if (s_levelCounter < 4)
                    {
                        s_levelCounter++;
                        s_showText = true;
                    }
                }
            }

            FadeOut();
        }
        if (s_fadeInStarted)
        {
            s_overlayBlack = false;
            FadeIn();
        }
    }

    private static void FadeIn()
    {
        if (s_image.gameObject.activeSelf == false)
        {
            s_image.gameObject.SetActive(true);
        }

        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            if (s_showText)
            {
                if (s_levelText.gameObject.activeSelf == false)
                {
                    s_levelText.gameObject.SetActive(true);
                }
            }
        }

        s_image.color = Color.Lerp(Color.black, Color.clear, s_timer);
        s_levelText.color = Color.Lerp(Color.white, Color.clear, s_timer);

        s_timer += (Time.deltaTime / s_duration);

        if (s_image.color.a <= 0)
        {
            s_fadeInStarted = false;
            s_overlayBlack = false;
            s_overlayClear = true;
            s_showText = false;
            s_levelText.gameObject.SetActive(false);
            s_image.gameObject.SetActive(false);
        }
    }

    private static void FadeOut()
    {
        if (s_image.gameObject.activeSelf == false)
        {
            s_image.gameObject.SetActive(true);
        }

        if (s_showText)
        {
            if (s_levelText.gameObject.activeSelf == false)
            {
                s_levelText.gameObject.SetActive(true);
            }
        }

        s_image.color = Color.Lerp(Color.clear, Color.black, s_timer);
        s_levelText.color = Color.Lerp(Color.clear, Color.white, s_timer);

        s_timer += (Time.deltaTime / s_duration);

        if (s_image.color.a >= 1)
        {
            s_fadeOutStarted = false;
            s_overlayBlack = true;
        }
    }

    public static void StartFadeIn()
    {
        if (!s_fadeInStarted)
        {
            s_timer = 0;
        }
        s_fadeOutStarted = false;
        s_fadeInStarted = true;
    }

    public static void StartFadeOut()
    {
        if (!s_fadeOutStarted)
        {
            s_timer = 0;
        }
        s_fadeInStarted = false;
        s_fadeOutStarted = true;
    }

    public static void ResetTransition()
    {
        s_timer = 0f;
        s_fadeOutStarted = false;
        s_fadeInStarted = false;
        s_playerDied = false;
        if (s_overlayClear == true)
        {
            s_image.color = Color.clear;
        }
        else if (s_overlayBlack == true)
        {
            s_image.color = Color.black;
        }
    }
}
