using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNTScript : MonoBehaviour
{
    Map m_map;

    GameObject m_explosionPrefab;
    string m_explosionPath = "Prefabs/Explosion";

    public bool m_explosionActive;

    public bool dead;

    void Start()
    {
        m_map = FindObjectOfType<Map>();
        m_map.AddEntity(m_map.WorldPositionToMapIndex(transform.position), gameObject);

        m_explosionPrefab = Resources.Load(m_explosionPath, typeof(GameObject)) as GameObject;

        m_explosionActive = false;
    }

    private void Update()
    {
        if (m_explosionActive == true)
        {
            SetExplosion();
        }
    }

    public void SetExplosion()
    {
        MapIndex mapIndex = m_map.WorldPositionToMapIndex(transform.position);

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

        Instantiate(m_explosionPrefab, m_map.MapIndexToWorldPosition(mapIndex), Quaternion.identity);
        HitCharacters(mapIndex);

        for (int i = 1; i <= 2; i++)
        {
            for (int j = 0; j < directions.Length; j++)
            {
                if (!directionStopped[j])
                {
                    MapIndex nextIndex = new MapIndex(mapIndex.m_x + directions[j].m_x * i,
                    mapIndex.m_y + directions[j].m_y * i);

                    if (m_map.GetTile(nextIndex) == null)
                    {
                        directionStopped[j] = true;
                    }

                    else if (!m_map.GetTile(nextIndex).GetIsTraversable())
                    {
                        directionStopped[j] = true;

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

        explosion();
    }

    void explosion()
    {
        if (FindObjectOfType<SoundManager>() != null)
        {
            FindObjectOfType<SoundManager>().Play("Bomb Explosion");
        }

        m_map.RemoveEntity(m_map.WorldPositionToMapIndex(transform.position), gameObject);
        Destroy(gameObject);
        dead = true;
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
            }
            else if (gameObjects[i].tag == "Button")
            {
                gameObjects[i].GetComponent<WorldButton>().SetIsPressed(true);
            }
        }
    }

   public void SetExplosionActive()
    {
        m_explosionActive = true;
    }

    public bool GetExplosionActive()
    {
        return m_explosionActive;
    }

    public bool GetStatus()
    {
        return dead;
    }
}
