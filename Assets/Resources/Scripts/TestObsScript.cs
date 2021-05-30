using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObsScript : MonoBehaviour
{
    public Map map;
    public GameObject ubPrefab;
    public GameObject bPrefab;

    void Awake()
    {
        GameObject gameOb = ObstacleScript.InstantiateObs(ubPrefab, new Vector2(0.48f, 0.80f), Quaternion.identity, false);
        gameOb.tag = "Object";

        map.AddEntity(new MapIndex(1, 2), gameOb);
        map.AddEntityToList(gameOb);

        GameObject gameOb2 = ObstacleScript.InstantiateObs(bPrefab, new Vector2(0.80f, 0.80f), Quaternion.identity, true);
        gameOb2.tag = "Object";

        map.AddEntity(new MapIndex(2, 2), gameOb2);
        map.AddEntityToList(gameOb2);
    }
}
