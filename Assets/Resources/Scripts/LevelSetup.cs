using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetup : MonoBehaviour
{
    MapEditor m_mapEditor;

    void Awake()
    {
        m_mapEditor = GetComponent<MapEditor>();
        m_mapEditor.LoadLevel(GameController.s_currentSceneName, true);

        GameController.FindAllButtons();
    }
}