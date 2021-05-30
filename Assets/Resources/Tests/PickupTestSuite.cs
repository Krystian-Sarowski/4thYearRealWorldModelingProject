using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PickupTestSuite
{
    string lifePrefab = "Prefabs/Life Pickup";
    string timePrefab = "Prefabs/Time Pickup";
    string speedPrefab = "Prefabs/Speed Pickup";
    string invulPrefab = "Prefabs/Invul Pickup";
    string playerPrefab = "Prefabs/Player";
    string gcPrefab = "Prefabs/GameController";

    GameObject pickupObj;
    GameObject playerObj;
    GameObject gcObj;

    [UnityTest]
    public IEnumerator LifePickupTest()
    {
        GameObject go = Resources.Load<GameObject>(lifePrefab);
        pickupObj = GameObject.Instantiate(go, new Vector3(-5, 0), Quaternion.identity);
        pickupObj.name = "Life Pickup";

        go = Resources.Load<GameObject>(playerPrefab);
        playerObj = GameObject.Instantiate(go, new Vector3(5, 0), Quaternion.identity);

        go = Resources.Load<GameObject>(gcPrefab);
        gcObj = GameObject.Instantiate(go, Vector3.zero, Quaternion.identity);

        int playerLives = GameController.s_playerLives;

        playerObj.transform.position = pickupObj.transform.position;

        yield return new WaitForSeconds(0.1f);

        Assert.True(null == pickupObj.gameObject);
        Assert.AreNotEqual(GameController.s_playerLives, playerLives);
    }

    [UnityTest]
    public IEnumerator TimePickupTest()
    {
        GameObject go = Resources.Load<GameObject>(timePrefab);
        pickupObj = GameObject.Instantiate(go, new Vector3(-5, 0), Quaternion.identity);
        pickupObj.name = "Time Pickup";

        go = Resources.Load<GameObject>(playerPrefab);
        playerObj = GameObject.Instantiate(go, new Vector3(5, 0), Quaternion.identity);

        go = Resources.Load<GameObject>(gcPrefab);
        gcObj = GameObject.Instantiate(go, Vector3.zero, Quaternion.identity);

        float timeRemaining = GameController.s_timeRemaning;

        playerObj.transform.position = pickupObj.transform.position;

        yield return new WaitForSeconds(0.1f);

        Assert.True(null == pickupObj.gameObject);
        Assert.AreNotEqual(GameController.s_timeRemaning, timeRemaining);
    }

    [UnityTest]
    public IEnumerator SpeedPickupTest()
    {
        GameObject go = Resources.Load<GameObject>(speedPrefab);
        pickupObj = GameObject.Instantiate(go, new Vector3(-5, 0), Quaternion.identity);
        pickupObj.name = "Speed Pickup";

        go = Resources.Load<GameObject>(playerPrefab);
        playerObj = GameObject.Instantiate(go, new Vector3(5, 0), Quaternion.identity);

        go = Resources.Load<GameObject>(gcPrefab);
        gcObj = GameObject.Instantiate(go, Vector3.zero, Quaternion.identity);

        float playerSpeed = PlayerController.s_speed;

        playerObj.transform.position = pickupObj.transform.position;

        yield return new WaitForSeconds(0.1f);
        Assert.True(null == pickupObj.gameObject);
        Assert.AreNotEqual(PlayerController.s_speed, playerSpeed);
    }

    [UnityTest]
    public IEnumerator InvulPickupTest()
    {
        GameObject go = Resources.Load<GameObject>(invulPrefab);
        pickupObj = GameObject.Instantiate(go, new Vector3(-5, 0), Quaternion.identity);
        pickupObj.name = "Invul Pickup";

        go = Resources.Load<GameObject>(playerPrefab);
        playerObj = GameObject.Instantiate(go, new Vector3(5, 0), Quaternion.identity);

        go = Resources.Load<GameObject>(gcPrefab);
        gcObj = GameObject.Instantiate(go, Vector3.zero, Quaternion.identity);

        playerObj.transform.position = pickupObj.transform.position;

        yield return new WaitForSeconds(0.1f);

        go = GameObject.Find("Bubble(Clone)");

        Assert.True(null == pickupObj.gameObject);
        Assert.False(null == go);
    }
}

