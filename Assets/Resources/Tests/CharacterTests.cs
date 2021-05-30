using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

public class CharacterTests
{

    string player = "Prefabs/Player";
    string enemy = "Prefabs/Enemy1";

    GameObject playerObject;
    GameObject enemyObject;

    [SetUp]
    public void Setup()
    {
        GameObject controllerPrefab = Resources.Load<GameObject>(player);

        playerObject = GameObject.Instantiate(controllerPrefab, new Vector2(50.0f, 50.0f), Quaternion.identity);

        GameObject controllerEnemyPrefab = Resources.Load<GameObject>(enemy);

        enemyObject = GameObject.Instantiate(controllerEnemyPrefab, new Vector2(0.0f, 0.0f), Quaternion.identity);
    }

    [UnityTest, Order(1)]
    public IEnumerator EnemyMovement()
    {
        enemyObject.transform.position = new Vector2(0, 0);

        Vector3 initialPos = enemyObject.transform.position;

        yield return new WaitForSeconds(0.5f);

        Assert.AreNotEqual(enemyObject.transform.position, initialPos);
    }

    [UnityTest, Order(3)]
    public IEnumerator EnemyCollision()
    {
        int playerLife = GameController.GetPlayerLives();

        playerObject.transform.position = enemyObject.transform.position;

        yield return new WaitForSeconds(0.4f);

        Assert.AreEqual(playerLife, GameController.GetPlayerLives());
    }

    [UnityTest, Order(2)]
    public IEnumerator PlayerUpgradeTest()
    {
        int bombCountLevel = playerObject.GetComponent<PlayerController>().m_upgradeSystem.GetUpgrade("Bomb Count").GetUpgradeLevel();
        int bombRangeLevel = playerObject.GetComponent<PlayerController>().m_upgradeSystem.GetUpgrade("Range").GetUpgradeLevel();

        playerObject.GetComponent<PlayerController>().m_upgradeSystem.GetUpgrade("Bomb Count").IncreaseLevel();
        playerObject.GetComponent<PlayerController>().m_upgradeSystem.GetUpgrade("Range").IncreaseLevel();

        yield return new WaitForSeconds(0.1f);

        Assert.AreNotEqual(bombCountLevel, playerObject.GetComponent<PlayerController>().m_upgradeSystem.GetUpgrade("Bomb Count").GetUpgradeLevel());
        Assert.AreNotEqual(bombRangeLevel, playerObject.GetComponent<PlayerController>().m_upgradeSystem.GetUpgrade("Range").GetUpgradeLevel());
    }
}
