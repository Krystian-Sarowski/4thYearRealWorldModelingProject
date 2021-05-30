using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TNTSuite
{
    public GameObject TNT;
    public TNTScript script;

    public Map map;
    public GameObject GetMap;


    [SetUp]
    public void Setup()
    {
        GetMap = new GameObject();
        GetMap.AddComponent<Map>();
        map = GetMap.GetComponent<Map>();
    }
   
    [UnityTest]
    public IEnumerator Explosion()
    {
        map.SetSize(4, 1);
        map.CreateMap();
        Vector2 pos = map.MapIndexToWorldPosition(new MapIndex(3, 0));
        GameObject TNT = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/TNT"), pos, Quaternion.identity);
        TNT.AddComponent<TNTScript>();
        script = TNT.GetComponent<TNTScript>();
        script.SetExplosionActive();
        yield return new WaitForSeconds(3.0f);
        Assert.IsFalse(script.GetStatus());
    }
  
}