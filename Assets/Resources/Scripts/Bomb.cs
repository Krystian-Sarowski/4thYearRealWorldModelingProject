using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Bomb : MonoBehaviour
{
    static int s_bombRange = 1;

    float m_count = 3.0f;
    string m_explosionPath = "Prefabs/Explosion";

    GameObject m_explosionPrefab;

    BombSpawner m_bombSpawner;

    Map m_map;

    void Start()
    {
        m_explosionPrefab = Resources.Load<GameObject>(m_explosionPath);

        m_bombSpawner = FindObjectOfType<BombSpawner>();

        m_map = FindObjectOfType<Map>();
        m_map.AddEntity(m_map.WorldPositionToMapIndex(transform.position), gameObject);
    }

    void Update()
    {
        BombLife();
    }

    public static void SetBombRange(int t_bombRange)
    {
        s_bombRange = t_bombRange;
    }

    public static int GetBombRange()
    {
        return s_bombRange;
    }

    void BombLife()
    {
        m_count -= Time.deltaTime;

        if (Input.GetButtonDown("Boom") || m_count <= 0.0f)
        {
            Explode();
        }        
    }

    public void Explode()
    {
        if(FindObjectOfType<SoundManager>() != null)
        {
            FindObjectOfType<SoundManager>().Play("Bomb Explosion");
        }

        m_map.RemoveEntity(m_map.WorldPositionToMapIndex(transform.position), gameObject);
        StartExplosion();
        Destroy(gameObject);
        m_bombSpawner.BombDead();
    }

    void StartExplosion()
    {
        MapIndex startIndex = m_map.WorldPositionToMapIndex(transform.position);

        Instantiate(m_explosionPrefab, m_map.MapIndexToWorldPosition(startIndex), Quaternion.identity);

        HitObjects(startIndex);
        HitCharacters(startIndex);

        int explosionRange = s_bombRange;

        List<GameObject> entityList = m_map.GetEntity(startIndex);

        foreach(GameObject gameObject in entityList)
        {
            if(gameObject.tag == "Extender")
            {
                explosionRange = explosionRange * 2;
            }
        }

        CreateExplosions(startIndex, explosionRange);
    }

    void CreateExplosions(MapIndex t_startIndex, int t_explosionRange)
    {
        MapIndex[] directions = new MapIndex[4];
        directions[0] = new MapIndex(0, 1);
        directions[1] = new MapIndex(1, 0);
        directions[2] = new MapIndex(0, -1);
        directions[3] = new MapIndex(-1, 0);

        bool[] directionStopped = new bool[4];
        directionStopped[0] = false;
        directionStopped[1] = false;
        directionStopped[2] = false;
        directionStopped[3] = false;

        for (int i = 1; i <= t_explosionRange; i++)
        {
            for (int j = 0; j < directions.Length; j++)
            {
                if (!directionStopped[j])
                {
                    MapIndex nextIndex = new MapIndex(t_startIndex.m_x + directions[j].m_x * i,
                    t_startIndex.m_y + directions[j].m_y * i);

                    if (m_map.GetTile(nextIndex) == null)
                    {
                        directionStopped[j] = true;
                    }

                    else if (!m_map.GetTile(nextIndex).GetIsTraversable())
                    {
                        directionStopped[j] = true;
                        HitObjects(nextIndex);

                        List<GameObject> gameObjects = m_map.GetEntity(nextIndex);

                        foreach (GameObject gameObject in gameObjects)
                        {

                            if (gameObject.tag == "Object")
                            {
                                gameObject.GetComponent<ObstacleScript>().Break();
                            }
                            else if (gameObject.tag == "TNT")
                            {
                                gameObject.GetComponent<TNTScript>().SetExplosionActive();
                            }
                            else if (gameObject.tag == "Magnet")
                            {
                                gameObject.GetComponent<MagnetController>().RotateMag();
                            }
                        }

                    }
                   

                    else
                    {
                        Instantiate(m_explosionPrefab, m_map.MapIndexToWorldPosition(nextIndex), Quaternion.identity);
                        HitCharacters(nextIndex);
                    }


                }
            }
        }
    }

    void HitObjects(MapIndex t_index)
    {
        List<GameObject> gameObjects = m_map.GetEntity(t_index);

        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.tag == "Object")
            {
                gameObject.GetComponent<ObstacleScript>().Break();
            }
        }
    }

    void HitCharacters(MapIndex t_index)
    {
        List<GameObject> gameObjects = m_map.GetEntity(t_index);

        for (int i = gameObjects.Count - 1; i >= 0; i--)
        {
            if (gameObjects[i].tag == "Player")
            {
                GameController.DeductLife(false);
            }
            else if (gameObjects[i].tag == "Character")
            {
                gameObjects[i].GetComponent<EnemyController>().KillEnemy();
                GameController.UpdateScore(100);
                GameController.s_gameData.enemies_killed += 1;
            }
            else if (gameObjects[i].tag == "Button")
            {
                gameObjects[i].GetComponent<WorldButton>().SetIsPressed(true);
            }
        }
    }
}
