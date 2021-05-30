using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum PlayeState
{ 
    Up,
    Down,
    Left,
    Right,
    Idle
}

public class PlayerController : MonoBehaviour
{
    Vector3 m_position;
    Vector3 m_previousPosition;
    Vector3 m_velocity;

    PlayeState m_currentState;
    PlayeState m_previousState;

    float m_friction = 0.99f;

    const float m_DEFAULT_SPEED = 1.0f;

    public static float s_speed = m_DEFAULT_SPEED;

    string m_bubblePath = "Prefabs/Bubble";

    bool m_isAnimatiorActive = true;

    public Animator m_animator;

    public UpgradeSystem m_upgradeSystem;

    private void Awake()
    {
        m_upgradeSystem = new UpgradeSystem();
        m_upgradeSystem.SetUpgradeList(UpgradeSystem.LoadUpgrades(m_upgradeSystem.GetUpgradeList()));
        Bomb.SetBombRange(m_upgradeSystem.GetUpgrade("Range").GetUpgradeLevel());
        BombSpawner.ChangeMaxBombCount(m_upgradeSystem.GetUpgrade("Bomb Count").GetUpgradeLevel());
    }

    // Start is called before the first frame update
    void Start()
    {
        m_position = transform.position;
        m_previousPosition = m_position;
        Vector3 velocity = Vector3.zero;

        m_currentState = PlayeState.Idle;
        m_currentState = PlayeState.Down;

        s_speed = m_DEFAULT_SPEED + (0.05f * m_upgradeSystem.GetUpgrade("Speed").GetUpgradeLevel());
    }

    void FixedUpdate()
    {
        m_previousPosition = m_position;
        m_position += m_velocity * m_friction;
        transform.position = m_position;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.s_gameState != GameState.Paused)
        {
            InputController();

            if (m_isAnimatiorActive == true)
            {
                switch (m_currentState)
                {
                    case PlayeState.Down:
                        m_animator.Play("walkDown");
                        break;
                    case PlayeState.Left:
                        m_animator.Play("walkLeft");
                        break;
                    case PlayeState.Right:
                        m_animator.Play("walkRight");
                        break;
                    case PlayeState.Up:
                        m_animator.Play("walkUp");
                        break;
                    case PlayeState.Idle:
                        switch (m_previousState)
                        {
                            case PlayeState.Down:
                                m_animator.Play("IdleDown");
                                break;
                            case PlayeState.Left:
                                m_animator.Play("IdleLeft");
                                break;
                            case PlayeState.Right:
                                m_animator.Play("IdleRight");
                                break;
                            case PlayeState.Up:
                                m_animator.Play("IdleUp");
                                break;
                            case PlayeState.Idle:
                                break;
                        }
                        break;
                    default:
                        break;
                }            
            }                   
        }
    }

    void InputController()
    {

        if(Input.GetKey("w"))
        {
            Move(new Vector3(0, 0.01f,0), PlayeState.Up);
        }

        else if (Input.GetKey("a"))
        {
            Move(new Vector3(-0.01f, 0, 0), PlayeState.Left);
        }

        else if (Input.GetKey("s"))
        {
            Move(new Vector3(0, -0.01f, 0), PlayeState.Down);
        }

        else if (Input.GetKey("d"))
        {
            Move(new Vector3(0.01f, 0, 0), PlayeState.Right);
        }

        else if(m_currentState != PlayeState.Idle)
        {
            Move(Vector3.zero, PlayeState.Idle);
        }

    }

    void Move(Vector3 t_vel, PlayeState t_newState)
    {
        m_velocity = t_vel * s_speed;
        m_previousState = m_currentState;
        m_currentState = t_newState;
    }

    // public to allow map access
    public void StopMovement()
    {
        Debug.Log("Stop Called");
        Move(Vector3.zero, PlayeState.Idle);
        m_position = m_previousPosition;
        transform.position = m_position;
    }

    public IEnumerator StartInvul()
    {
        if (FindObjectOfType<SoundManager>() != null)
        {
            FindObjectOfType<SoundManager>().Play("Invurnable");
        }

        StartCoroutine(SpawnBubble());
        GameController.s_invulnerability = true;
        yield return new WaitForSeconds(10f);
        GameController.s_invulnerability = false;
    }

    public IEnumerator SpawnBubble()
    {
        if (!GameController.s_invulnerability)
        {
            GameObject go = Resources.Load<GameObject>(m_bubblePath);
            GameObject bubble = Instantiate(go, transform.position, Quaternion.identity);
            bubble.transform.SetParent(transform);

            yield return new WaitForSeconds(7f);

            int count = 0;

            while (count < 3)
            {
                bubble.GetComponent<SpriteRenderer>().color = Color.clear;
                yield return new WaitForSeconds(0.5f);
                bubble.GetComponent<SpriteRenderer>().color = Color.white;
                yield return new WaitForSeconds(0.5f);
                count++;
            }

            Destroy(bubble);
        }
        yield return null;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Character")
        {
            m_velocity = new Vector2(0.0f, 0.0f);
            GameController.DeductLife(false);
        }

        if (other.gameObject.tag == "Pickup")
        {
            switch (other.gameObject.name)
            {
                case "Bomb Pickup":
                    m_upgradeSystem.GetUpgrade("Bomb Count").IncreaseLevel();
                    BombSpawner.ChangeMaxBombCount(m_upgradeSystem.GetUpgrade("Bomb Count").GetUpgradeLevel());
                    break;
                case "Range Pickup":
                    m_upgradeSystem.GetUpgrade("Range").IncreaseLevel();
                    Bomb.SetBombRange(m_upgradeSystem.GetUpgrade("Range").GetUpgradeLevel());
                    break;
                case "Life Pickup":
                    GameController.s_playerLives++;
                    break;
                case "Time Pickup":
                    GameController.s_timeRemaning += 15;
                    break;
                case "Speed Pickup":
                    if (m_upgradeSystem.GetUpgrade("Speed").GetUpgradeLevel() < 10)
                    {
                        m_upgradeSystem.GetUpgrade("Speed").IncreaseLevel();
                        s_speed = m_DEFAULT_SPEED + (0.05f * m_upgradeSystem.GetUpgrade("Speed").GetUpgradeLevel());
                    }
                    break;
                case "Invul Pickup":
                    if(!GameController.s_invulnerability)
                    {
                        StartCoroutine(StartInvul());
                    }
                    break;
                default:
                    break;
            }

            if (FindObjectOfType<SoundManager>() != null)
            {
                FindObjectOfType<SoundManager>().Play("Pickup");
            }

            Destroy(other.gameObject);
        }
    }
}
