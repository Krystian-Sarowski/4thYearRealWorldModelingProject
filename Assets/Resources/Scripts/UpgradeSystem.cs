using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class Upgrade
{
    [JsonProperty]
    private string m_upgradeName;
    [JsonProperty]
    private int m_upgradeLevel;

    public Upgrade(string name, int level) 
    {
        m_upgradeName = name;
        m_upgradeLevel = level;
    }

    public void IncreaseLevel()
    {
        m_upgradeLevel += 1;
    }

    public void DecreaseLevel()
    {
        m_upgradeLevel -= 1;
    }

    public void ResetLevel()
    {
        m_upgradeLevel = 0;
    }

    public void SetLevel(int t_level)
    {
        m_upgradeLevel = t_level;
    }

    public string GetUpgradeName()
    {
        return m_upgradeName;
    }

    public int GetUpgradeLevel()
    {
        return m_upgradeLevel;
    }
}

public class UpgradeSystem
{
    [JsonProperty]
    private List<Upgrade> m_upgradeList = new List<Upgrade>();
    static string jsonSavePath = Application.persistentDataPath + "/upgrades.json";
    static string jsonLoadPath = Application.streamingAssetsPath + "/defaultUpgrades.json";

    public UpgradeSystem() { }

    public void AddUpgrade(Upgrade t_upgrade)
    {
        m_upgradeList.Add(t_upgrade);
    }

    public int GetUpgradeCount()
    {
        return m_upgradeList.Count;
    }

    public List<Upgrade> GetUpgradeList()
    {
        return m_upgradeList;
    }

    public void SetUpgradeList(List<Upgrade> upgradeList)
    {
        m_upgradeList = upgradeList;
    }

    public Upgrade GetUpgrade(string t_name)
    {
        foreach (Upgrade upgrade in m_upgradeList)
        {
            if (upgrade.GetUpgradeName() == t_name)
            {
                return upgrade;
            }
        }
        return null;
    }

    public static void SaveUpgrades(List<Upgrade> upgradeList)
    {
        string jsonString;

        jsonString = JsonConvert.SerializeObject(upgradeList);
 
        File.WriteAllText(jsonSavePath, jsonString);
    }

    public static List<Upgrade> LoadUpgrades(List<Upgrade> upgradeList)
    {
        string jsonString;

        jsonString = File.ReadAllText(jsonSavePath);

        upgradeList = JsonConvert.DeserializeObject<List<Upgrade>>(jsonString);

        return upgradeList;
    }

    public static void ResetUpgrades()
    {
        string jsonString;

        jsonString = File.ReadAllText(jsonLoadPath);

        List<Upgrade> upgradeList = JsonConvert.DeserializeObject<List<Upgrade>>(jsonString);

        SaveUpgrades(upgradeList);
    }
}
