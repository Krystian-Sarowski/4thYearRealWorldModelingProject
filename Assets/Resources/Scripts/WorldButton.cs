using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WorldButton : MonoBehaviour
{
    SpriteRenderer m_spriteRenderer;
    string m_buttonPressedPath = "Sprites/ButtonPressed";
    string m_buttonNotPressedPath = "Sprites/ButtonNotPressed";

    bool m_isPressed = false;

    void Awake()
    {
        m_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        m_spriteRenderer.sprite = Resources.Load(m_buttonNotPressedPath, typeof(Sprite)) as Sprite;
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            SetIsPressed(true);
        }
    }

    public void SetIsPressed(bool t_isPressed)
    {
        m_isPressed = t_isPressed;

        if(m_isPressed)
        {
            m_spriteRenderer.sprite = Resources.Load(m_buttonPressedPath, typeof(Sprite)) as Sprite;
        }
        else
        {
            m_spriteRenderer.sprite = Resources.Load(m_buttonNotPressedPath, typeof(Sprite)) as Sprite;
        }
    }

    public bool GetIsPressed()
    {
        return m_isPressed;
    }
}
