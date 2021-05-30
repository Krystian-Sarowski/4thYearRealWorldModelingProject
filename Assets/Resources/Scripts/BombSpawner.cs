using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    public GameObject m_bombPrefab;

    string m_bombPath = "Prefabs/Bomb";

    [SerializeField]
    public int m_bombCount;
    [SerializeField]
    static int s_maxBombCount = 1;

    [HideInInspector]
    public Map m_map;

    void Start()
    {
        m_map = FindObjectOfType<Map>();
        m_bombPrefab = Resources.Load<GameObject>(m_bombPath);
        m_bombCount = 0;
    }

    void Update()
    {
        if (GameController.s_gameState != GameState.Paused)
        {
            if (Input.GetButtonDown("Jump"))
            {
                if (s_maxBombCount > m_bombCount)
                {
                    BombPlant();
                }
            }
        }
    }

    public void BombPlant()
    { 
        MapIndex mapIndex = m_map.WorldPositionToMapIndex(transform.position);

        if(m_map.GetTile(mapIndex).GetIsTraversable())
        {
            Vector2 pos = m_map.MapIndexToWorldPosition(mapIndex);
            Instantiate(m_bombPrefab, pos, Quaternion.identity);

            GameController.s_gameData.bombs_placed += 1;

            if(FindObjectOfType<SoundManager>() != null)
            {
                FindObjectOfType<SoundManager>().Play("Bomb Plant");
            }

            BombCountAdd();
        }
    }

    public void BombDead()
    {
        m_bombCount -= 1;
    }

    public int BombCountAdd()
    {
        m_bombCount += 1;

        return m_bombCount;
    }

    public static int GetBombCount()
    {
        return s_maxBombCount;
    }

    public static void ChangeMaxBombCount(int count)
    {
        s_maxBombCount = count;
    }
}
