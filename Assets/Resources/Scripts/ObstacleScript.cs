using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    public bool m_isBreakable;

    public List<GameObject> m_pickupPrefabs = new List<GameObject>();
    
    [HideInInspector]
    public Animator m_anim;

    public static int s_randomChance;

    public static GameObject InstantiateObs(GameObject o, Vector3 p, Quaternion r, bool b)
    {
        GameObject tempO;

        tempO = Instantiate(o, p, r);
        tempO.AddComponent<ObstacleScript>();
        tempO.GetComponent<ObstacleScript>().m_isBreakable = b;

        return tempO;
    }

    public void Break()
    {
        if (m_anim == null)
        {
            m_anim = GetComponent<Animator>();
        }

        if (m_isBreakable)
        {
            m_anim.SetBool("destroyed", true);
        }
    }

    public void DestroyObj()
    {
        if (m_isBreakable)
        {
            GameController.s_gameData.boxes_destroyed += 1;
            s_randomChance = Random.Range(0, 101);
            Map map = FindObjectOfType<Map>();

            MapIndex index = map.WorldPositionToMapIndex(transform.position);
            if (s_randomChance > 70)
            {
                SpawnPickup();
            }
            map.RemoveEntity(index, gameObject);
            Destroy(gameObject);
        }
    }

    void SpawnPickup()
    {
        if(m_pickupPrefabs.Count != 0)
        {
            int listIndex = Random.Range(0, m_pickupPrefabs.Count);
            GameObject go = Instantiate(m_pickupPrefabs[listIndex], transform.position, Quaternion.identity);
            go.name = go.name.Replace("(Clone)", "");
        }
    }
}
