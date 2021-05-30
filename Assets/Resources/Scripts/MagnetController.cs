using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetController : MonoBehaviour
{
    int magRotate = 0;
    Map m_map;
    public bool hit = false;

    Vector2 p_magnet;
    int magnetRange = 3;

    public Vector2 newBombPos;

    private void Start()
    {
        p_magnet = transform.position;

        m_map = FindObjectOfType<Map>();
    }

    public void FixedUpdate()
    {
        PullBomb();
    }

    public void RotateMag()
    {
        transform.Rotate(0, 0, -90, 0);
        magRotate = (magRotate + 1) % 4;
    }


    public bool PullBomb()
    {
        MapIndex mapIndex = m_map.WorldPositionToMapIndex(p_magnet);

        MapIndex[] directions = new MapIndex[4];

        //up
        directions[0] = new MapIndex(0, 1);
        //right
        directions[1] = new MapIndex(1, 0);
        //down
        directions[2] = new MapIndex(0, -1);
        //left
        directions[3] = new MapIndex(-1, 0);
        
        MapIndex check;

        bool wallCheck = false;

        for (int j = 1; j <= magnetRange && wallCheck == false; j++)
        {
            check.m_x = mapIndex.m_x + (directions[magRotate].m_x * j);
            check.m_y = mapIndex.m_y + (directions[magRotate].m_y * j);

            List<GameObject> enityList = m_map.GetEntity(check);

            foreach (GameObject gameObject in enityList)
            {
                if (gameObject.tag == "Bomb")
                {
                    m_map.RemoveEntity(check, gameObject);
                    MapIndex newBombIndex = new MapIndex(mapIndex.m_x + directions[magRotate].m_x, mapIndex.m_y + directions[magRotate].m_y);
                    m_map.AddEntity(newBombIndex, gameObject);
                    gameObject.transform.position = m_map.MapIndexToWorldPosition(newBombIndex);
                    return true;
                }

                if(gameObject.tag == "Object")
                {
                    return false;
                }
            }

            if(m_map.GetTile(check).GetIsTraversable() == false)
            {
                wallCheck = true;
            }
        }

        return false;
    }
}
