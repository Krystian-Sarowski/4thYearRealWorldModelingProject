using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;

public class GameControllerSuite
{
    GameObject controllerObject;

    string path = "Prefabs/GameController";

    [OneTimeSetUp]
    public void Setup()
    {
        if (controllerObject == null)
        {
            GameObject controllerPrefab = Resources.Load<GameObject>(path);

            controllerObject = GameObject.Instantiate(controllerPrefab, new Vector2(0.0f, 0.0f), Quaternion.identity);
        }
    }

    [UnityTest, Order(1)]
    public IEnumerator GameStates()
    {
        Debug.ClearDeveloperConsole();

        Assert.True(Time.timeScale == 1);
        Assert.True(GameController.s_gameState == GameState.Running);

        GameController.PauseGame();

        Assert.True(Time.timeScale == 0);
        Assert.True(GameController.s_gameState == GameState.Paused);

        GameController.UnpauseGame();

        Assert.True(Time.timeScale == 1);
        Assert.True(GameController.s_gameState == GameState.Running);

        yield return new WaitForSeconds(0.2f);
        Assert.True(GameController.s_gameState == GameState.Running);

        GameController.s_gameState = GameState.Victory;
        yield return new WaitForSeconds(0.1f);
        LogAssert.Expect(LogType.Log, "Level Has Been Completed");

        GameController.s_gameState = GameState.Gameover;
        yield return new WaitForSeconds(0.1f);
        LogAssert.Expect(LogType.Log, "Gameover");

        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest, Order(2)]
    public IEnumerator ExtraControllerDestoryed()
    {
        Debug.ClearDeveloperConsole();

        GameObject controllerPrefab = Resources.Load<GameObject>(path);

        GameObject controllerObject2 = GameObject.Instantiate(controllerPrefab, new Vector2(0.0f, 0.0f), Quaternion.identity);

        yield return new WaitForSeconds(0.1f);

        Assert.True(controllerObject2 == null);

        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest, Order(3)]
    public IEnumerator UpdateScore()
    {
        Debug.ClearDeveloperConsole();

        Assert.True(GameController.GetScore() == 0);

        GameController.UpdateScore(10);
        Assert.True(GameController.GetScore() == 10);

        GameController.UpdateScore(-5);
        Assert.True(GameController.GetScore() == 5);

        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest, Order(4)]
    public IEnumerator ResetScore()
    {
        Debug.ClearDeveloperConsole();

        Assert.True(GameController.GetScore() == 5);

        GameController.ResetScore();
        Assert.True(GameController.GetScore() == 0);

        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest, Order(5)]
    public IEnumerator Timer()
    {
        GameController.s_gameState = GameState.Restarting;
        GameController.LoadScene("TestLevel1");

        yield return new WaitForSeconds(0.4f);

        GameController.SetUpdateTimer(true);

        Debug.ClearDeveloperConsole();

        GameController.SetTimer(2.0f);

        yield return new WaitForSeconds(GameController.GetTimeRemaning());
        
        LogAssert.Expect(LogType.Log, "The Level is restarting");

        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest, Order(6)]
    public IEnumerator StopTimerUpdating()
    {
        float time = GameController.GetTimeRemaning();

        GameController.SetUpdateTimer(false);

        yield return new WaitForSeconds(0.4f);

        Assert.True(time == GameController.GetTimeRemaning());
    }
}

