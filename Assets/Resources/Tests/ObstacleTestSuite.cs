using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public class ObstacleTestSuite
{
    GameObject m_map = null;
    Map m_mapScript = null;

    [SetUp]
    public void Setup()
    {
        m_map = new GameObject();
        m_map.AddComponent<Map>();

        m_mapScript = m_map.GetComponent<Map>();

        m_mapScript.SetSize(2, 2);
        m_mapScript.CreateMap();
    }

    [UnityTest]
    public IEnumerator UnbreakableObstacleTest()
    {
        GameObject obstacle = ObstacleScript.InstantiateObs(new GameObject(), new Vector2(0.16f, 0.16f), Quaternion.identity, false);
        obstacle.tag = "Object";

        Assert.True(m_mapScript.AddEntity(new MapIndex(0, 0), obstacle));

        obstacle.GetComponent<ObstacleScript>().DestroyObj();

        Assert.NotNull(obstacle);

        yield return null;
    }

    [UnityTest]
    public IEnumerator BreakableObstacleTest()
    {
        GameObject obstacle = ObstacleScript.InstantiateObs(new GameObject(), new Vector2(0.16f, 0.16f), Quaternion.identity, true);
        obstacle.tag = "Object";

        Assert.True(m_mapScript.AddEntity(new MapIndex(0, 0), obstacle));

        obstacle.GetComponent<ObstacleScript>().DestroyObj();

        yield return new WaitForSeconds(1.0f);

        Assert.True(obstacle == null);

        yield return null;
    }

    [UnityTest]
    public IEnumerator ExplosionExtender()
    {
        GameObject bombSpawner = new GameObject();

        m_mapScript.SetSize(4, 1);
        m_mapScript.CreateMap();

        List<string> objTags = new List<string>();
        objTags.Add("Object");

        m_mapScript.m_objectsTag = objTags;

        bombSpawner.AddComponent<BombSpawner>();
        bombSpawner.transform.position = m_mapScript.MapIndexToWorldPosition(new MapIndex(0, 0));

        BombSpawner spawner = bombSpawner.GetComponent<BombSpawner>();
        spawner.m_bombPrefab = Resources.Load<GameObject>("Prefabs/Bomb");
        spawner.m_map = m_mapScript;

        Vector2 pos = m_mapScript.MapIndexToWorldPosition(new MapIndex(3, 0));

        GameObject obstacle = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/BreakableObstacle"), pos, Quaternion.identity);

        Assert.True(m_mapScript.AddEntity(new MapIndex(3, 0), obstacle));

        GameObject extenderPrefab = Resources.Load<GameObject>("Prefabs/ExplosionExtender");

        pos = m_mapScript.MapIndexToWorldPosition(new MapIndex(1, 0));

        GameObject explosionExtender = GameObject.Instantiate(extenderPrefab, pos, Quaternion.identity);

        Assert.True(m_mapScript.AddEntity(new MapIndex(1, 0), explosionExtender));

        Bomb.SetBombRange(1);

        spawner.BombPlant();

        yield return new WaitForSeconds(0.1f);

        GameObject.FindObjectOfType<Bomb>().Explode();

        yield return new WaitForSeconds(1.0f);

        Assert.False(obstacle == null);

        bombSpawner.transform.position = pos;

        spawner.BombPlant();

        yield return new WaitForSeconds(0.1f);

        GameObject.FindObjectOfType<Bomb>().Explode();

        yield return new WaitForSeconds(1.0f);

        Assert.True(obstacle == null);
    }
}
    