using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Menu = 1,
    Running = 2,
    Paused = 3,
    Restarting = 4,
    Victory = 5,
    Gameover = 6
}

public class GameController : MonoBehaviour
{
    [HideInInspector]
    public static GameController s_instance;

    [HideInInspector]
    public static GameState s_gameState;

    [HideInInspector]
    public static GameData s_gameData = new GameData();

    public static float s_timeRemaning = 100.0f;
    static float s_startTime = 120.0f;
    static float s_lastUpdateTime;

    static List<GameObject> s_buttonObjectList = new List<GameObject>();

    //Player Lives
    public static int s_playerLives;

    public static bool s_invulnerability;

    //Player Score
    static int s_score;

    //The current Level
    public static int s_currentLevel = 1;

    //Bool for if the remaning time should get updated.
    static bool s_updateTimer = true;

    public static string s_currentSceneName = "";

    void Awake()
    {
        if (s_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            s_instance = this;

            ResetScore();
            DontDestroyOnLoad(gameObject);
        }

        s_playerLives = 3;
        s_gameState = GameState.Running;

        s_gameData.device_id = SystemInfo.deviceUniqueIdentifier;
        s_gameData.level_id = s_currentLevel;

        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Transition.ResetTransition();
        Transition.StartFadeIn();

        if (FindObjectOfType<SoundManager>() != null)
        {
            FindObjectOfType<SoundManager>().Stop("Invurnable");
        }

        s_gameState = GameState.Running;

        s_currentSceneName = SceneManager.GetActiveScene().name;

        if (s_currentSceneName.Contains("Level"))
        {
            StartLevel();
        }

        SetUpdateTimer(true);
    }

    void Update()
    {
        // Check how many lives the player has 
        GetPlayerLives();
        switch (s_gameState)
        {
            case GameState.Running:
                if(s_updateTimer)
                {
                    UpdateTime();
                }

                break;
            case GameState.Menu:
                SceneManager.LoadScene("MainMenu");
                break;
            case GameState.Paused:
                break;
            case GameState.Restarting:
                Debug.Log("The Level is restarting");
                Transition.s_playerDied = true;
                Transition.StartFadeOut();
                if (Transition.s_overlayBlack == true)
                {
                    Transition.ResetTransition();
                    ReloadCurrentScene();
                }
                break;
            case GameState.Victory:
                Debug.Log("Level Has Been Completed");
                Transition.StartFadeOut();
                if (Transition.s_overlayBlack == true)
                {
                    Transition.ResetTransition();
                    LoadNextLevel();
                }
                break;
            case GameState.Gameover:
                Debug.Log("Gameover");
                Transition.s_playerDied = true;
                Transition.StartFadeOut();
                if (Transition.s_overlayBlack == true)
                {
                    Transition.ResetTransition();
                    LoadScene("Gameover");
                }
                break;
            default:
                break;
        }
    }

    static void UpdateText()
    {
        if(GameObject.Find("Gameplay Canvas") != null)
        {
            GameObject.Find("Score Text").GetComponent<Text>().text = "00000" + s_score.ToString();

            int time = (int)s_timeRemaning;

            GameObject.Find("Time Text").GetComponent<Text>().text = time.ToString();

            GameObject.Find("Life Text").GetComponent<Text>().text = s_playerLives.ToString();

            GameObject.Find("Bomb Count Text").GetComponent<Text>().text = BombSpawner.GetBombCount().ToString();

            GameObject.Find("Range Text").GetComponent<Text>().text = Bomb.GetBombRange().ToString();
        }
    }

    /// <summary>
    /// Pauses the game by setting the time scale to 0 and the state is set to paused.
    /// </summary>
    public static void PauseGame()
    {
        s_gameState = GameState.Paused;
        Time.timeScale = 0;
    }

    /// <summary>
    /// Unpauses the game by setting the time scale to 1 and the state is set to unpaused.
    /// </summary>
    public static void UnpauseGame()
    {
        s_gameState = GameState.Running;
        Time.timeScale = 1;
    }

    ///<summary>
    /// Game restart 
    /// </summary>
    public void ResetGame()
    {
        s_gameState = GameState.Restarting;
    }

    /// <summary>
    /// Updates the current value of the score by the passed in amount.
    /// This value can be either positve or negative.
    /// </summary>
    /// <param name="t_amount">The amount by which the score will increase/decrease accordingly</param>
    public static void UpdateScore(int t_amount)
    {
        s_score += t_amount;

        UpdateText();
    }

    /// <summary>
    /// Getter method for the score value.
    /// </summary>
    /// <returns>The current value of the score</returns>
    public static int GetScore()
    {
        return s_score;
    }

    /// <summary>
    /// Sets the score to the base starting value which is 0.
    /// </summary>
    public static void ResetScore()
    {
        s_score = 0;

        UpdateText();
    }

    /// <summary>
    /// Updates the time remaning before the player runs out of time
    /// and the level restarts.
    /// </summary>
    static void UpdateTime()
    {
        s_timeRemaning -= (Time.time - s_lastUpdateTime);
        s_gameData.time_remaining = (int)s_timeRemaning;
        s_lastUpdateTime = Time.time;

        UpdateText();

        if (s_timeRemaning <= 0.0f)
        {
            DeductLife(true);
        }
    }

    /// <summary>
    /// Getter method for the time remaning before the level restarts.
    /// </summary>
    /// <returns></returns>
    public static float GetTimeRemaning()
    {
        return s_timeRemaning;
    }

    ///<summary>
    /// Gets number of lives from player class 
    ///</summary>
    static public int GetPlayerLives()
    {
        return s_playerLives;
    }

    static public void ResetLives()
    {
        s_playerLives = 3;
    }

    public static void DeductLife(bool t_ignoreInvul)
    {
        if (s_invulnerability && !t_ignoreInvul)
        {
            return;
        }

        s_playerLives = s_playerLives - 1;

        UpdateText();

        if (FindObjectOfType<PlayerController>() != null)
        {
            UpgradeSystem.SaveUpgrades(FindObjectOfType<PlayerController>().m_upgradeSystem.GetUpgradeList());
        }

        if (s_playerLives == 0)
        {
            if (FindObjectOfType<PlayerController>() != null)
            {
                UpgradeSystem.ResetUpgrades();
            }

            s_gameState = GameState.Gameover;
        }
        else
        {
            s_gameState = GameState.Restarting;
        }

        s_gameData.state = "Died: " + s_currentLevel;
        s_instance.StartCoroutine(AnalyticsManager.PostMethod(JsonUtility.ToJson(s_gameData)));
        ResetGameData();
    }

    public static bool GetAreAllButtonsPressed()
    {
        foreach(GameObject buttonObject in s_buttonObjectList)
        {
            if(!buttonObject.GetComponent<WorldButton>().GetIsPressed())
            {
                return false;
            }
        }

        return true;
    }

    public static void FindAllButtons()
    {
        s_buttonObjectList.Clear();

        //Get all the buttons within the level.
        WorldButton[] buttonComponents = GameObject.FindObjectsOfType<WorldButton>();

        foreach (WorldButton button in buttonComponents)
        {
            s_buttonObjectList.Add(button.gameObject);
        }
    }

    public static void LoadScene(string t_sceneName)
    {
        SceneManager.LoadScene(t_sceneName);
    }

    public static void ReloadCurrentScene()
    {
        SceneManager.LoadScene(s_currentSceneName);
    }

    public static void SetUpdateTimer(bool t_updateTimer)
    {
        s_updateTimer = t_updateTimer;
    }

    public static void SetTimer(float t_newTime)
    {
        s_timeRemaning = t_newTime;
    }

    static void LoadNextLevel()
    {
        if (s_currentLevel != 4)
        {
            s_gameData.state = "Completed: " + s_currentLevel;

            s_currentLevel++;
            string levelName = "Level" + s_currentLevel;

            s_instance.StartCoroutine(AnalyticsManager.PostMethod(JsonUtility.ToJson(s_gameData)));
            s_gameData.level_id = s_currentLevel;
            ResetGameData();

            if (FindObjectOfType<PlayerController>() != null)
            {
                UpgradeSystem.SaveUpgrades(FindObjectOfType<PlayerController>().m_upgradeSystem.GetUpgradeList());
            }

            LoadScene(levelName);
        }
        else
        {
            s_gameData.state = "Completed: Victory";
            s_instance.StartCoroutine(AnalyticsManager.PostMethod(JsonUtility.ToJson(s_gameData)));
            LoadScene("Victory");
        }
    }

    public static void LoadFirstLevel()
    {
        if (FindObjectOfType<SoundManager>() != null)
        {
            FindObjectOfType<SoundManager>().PlayRandomBackgroundTrack();
        }

        s_currentLevel = 0;
        UpgradeSystem.ResetUpgrades();
        ResetLives();
        ResetScore();
        LoadNextLevel();
    }

    static void StartLevel()
    {
        s_lastUpdateTime = Time.time;
        s_timeRemaning = s_startTime + 10.0f * s_currentLevel;
        s_gameState = GameState.Running;
        s_invulnerability = false;

        if (FindObjectOfType<Map>() != null)
        {
            FindObjectOfType<Map>().gameObject.AddComponent<LevelSetup>();
        }

        if (FindObjectOfType<PlayerController>() != null)
        {
            FindObjectOfType<PlayerController>().m_upgradeSystem.SetUpgradeList(
                UpgradeSystem.LoadUpgrades(FindObjectOfType<PlayerController>().m_upgradeSystem.GetUpgradeList()));
        }

        SetUpdateTimer(true);
    }

    static void ResetGameData()
    {
        s_gameData.bombs_placed = 0;
        s_gameData.enemies_killed = 0;
        s_gameData.boxes_destroyed = 0;
        s_gameData.state = "";
    }
}
