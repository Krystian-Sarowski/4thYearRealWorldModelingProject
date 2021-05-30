using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using UnityEngine.SceneManagement;

public class ButtonsAndDoorSuite
{
    GameObject controllerObject;
    GameController gameController;
    string path = "Prefabs/GameController";

    WorldButton[] buttonArray;

    [OneTimeSetUp]
    public void Setup()
    {
        SceneManager.LoadScene("TestButtonScene");

        if (controllerObject == null)
        {
            GameObject controllerPrefab = Resources.Load<GameObject>(path);
            controllerObject = GameObject.Instantiate(controllerPrefab, new Vector2(0.0f, 0.0f), Quaternion.identity);
            gameController = controllerObject.GetComponent<GameController>();
        } 
    }

    [UnityTest, Order(1)]
    public IEnumerator OneButtonPressed()
    {
        GameController.FindAllButtons();

        buttonArray = GameObject.FindObjectsOfType<WorldButton>();

        buttonArray[0].SetIsPressed(true);

        Assert.True(buttonArray[0].GetIsPressed());

        Assert.False(GameController.GetAreAllButtonsPressed());

        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest, Order(2)]
    public IEnumerator AllButtonsPressed()
    {
        GameController.FindAllButtons();

        buttonArray = GameObject.FindObjectsOfType<WorldButton>();

        buttonArray[0].SetIsPressed(true);
        buttonArray[1].SetIsPressed(true);
        buttonArray[2].SetIsPressed(true);

        Assert.True(buttonArray[0].GetIsPressed());
        Assert.True(buttonArray[1].GetIsPressed());
        Assert.True(buttonArray[2].GetIsPressed());

        Assert.True(GameController.GetAreAllButtonsPressed());

        yield return new WaitForSeconds(0.1f);
    }
}
