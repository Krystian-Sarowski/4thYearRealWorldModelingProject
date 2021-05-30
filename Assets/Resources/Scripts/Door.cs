using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    bool m_isOpen = false;

    Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = gameObject.GetComponent<Animator>();
    }

    //Update is called once per frame
    void Update()
    {
        if(!m_isOpen)
        {
            if (GameController.GetAreAllButtonsPressed())
            {
                OpenDoor();

                if (FindObjectOfType<SoundManager>() != null)
                {
                    FindObjectOfType<SoundManager>().Play("Unlock");
                }
            }
        }
    }

    void OpenDoor()
    {
        m_isOpen = true;
        m_animator.SetBool("m_isOpen", m_isOpen);
    }

    public bool GetIsOpen()
    {
        return m_isOpen;
    }
}
