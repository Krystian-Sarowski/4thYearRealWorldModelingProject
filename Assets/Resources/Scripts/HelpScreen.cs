using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpScreen : MonoBehaviour
{
    public void MainMenu()
    {
        gameObject.SetActive(false);
        FindObjectOfType<MainMenu>().gameObject.SetActive(true);
    }
}
