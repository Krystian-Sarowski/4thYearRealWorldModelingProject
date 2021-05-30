using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class BombTestSuite
{
    GameObject m_bomb;

    BombSpawner m_bombSpawner;

    [SetUp]
    public void Setup()
    {
       m_bomb = new GameObject();
       m_bomb.AddComponent<BombSpawner>();
       
       m_bombSpawner = m_bomb.GetComponent<BombSpawner>();
    }

    [UnityTest]
    public IEnumerator TestSpawnAndDespawn()
    {   
        Input.GetKeyDown(KeyCode.Space);

        yield return new WaitForSeconds(0.1f);

        m_bombSpawner.BombCountAdd();

        yield return new WaitForSeconds(3.0f);

        m_bombSpawner.BombDead();
    }

    [UnityTest]
    public IEnumerator TestDet()
    {
        Input.GetKeyDown(KeyCode.Space);

        yield return new WaitForSeconds(0.1f);

        m_bombSpawner.BombCountAdd();

        Input.GetKeyDown(KeyCode.B);

        yield return new WaitForSeconds(0.1f);

        m_bombSpawner.BombDead();
    }
}
