using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UpgradeTestSuite
{

    [UnityTest, Order(1)]
    public IEnumerator UpgradeObjectCreationTest()
    {
        Upgrade testUpgrade = new Upgrade("Attack", 1);
        Assert.IsNotNull(testUpgrade);
        int tempUpgradeLevel = testUpgrade.GetUpgradeLevel();
        testUpgrade.IncreaseLevel();
        Assert.Greater(testUpgrade.GetUpgradeLevel(), tempUpgradeLevel);
        yield return null;
    }

    [UnityTest, Order(2)]
    public IEnumerator UpgradeSystemObjectCreationTest()
    {
        UpgradeSystem upgradeSystem = new UpgradeSystem();
        int listCount = upgradeSystem.GetUpgradeCount();
        Assert.AreEqual(listCount, 0);
        Upgrade testUpgrade = new Upgrade("Attack", 1);
        upgradeSystem.AddUpgrade(testUpgrade);
        listCount = upgradeSystem.GetUpgradeCount();
        Assert.AreEqual(listCount, 1);
        yield return null;
    }

    [UnityTest, Order(3)]
    public IEnumerator UpgradeObjectRetrievalTest()
    {
        UpgradeSystem upgradeSystem = new UpgradeSystem();
        Upgrade testUpgrade = new Upgrade("Attack", 1);
        upgradeSystem.AddUpgrade(testUpgrade);
        Upgrade testUpgrade2 = upgradeSystem.GetUpgrade("Attack");
        Assert.AreEqual("Attack", testUpgrade2.GetUpgradeName());
        yield return null;
    }

    [UnityTest, Order(4)]
    public IEnumerator UpgradeLevelTest()
    {
        UpgradeSystem upgradeSystem = new UpgradeSystem();
        Upgrade testUpgrade = new Upgrade("Attack", 1);
        upgradeSystem.AddUpgrade(testUpgrade);
        int upgradeLevel = upgradeSystem.GetUpgrade("Attack").GetUpgradeLevel();
        upgradeSystem.GetUpgrade("Attack").IncreaseLevel();
        Assert.AreNotEqual(upgradeLevel, upgradeSystem.GetUpgrade("Attack").GetUpgradeLevel());
        upgradeLevel = upgradeSystem.GetUpgrade("Attack").GetUpgradeLevel();
        upgradeSystem.GetUpgrade("Attack").SetLevel(0);
        Assert.AreNotEqual(upgradeLevel, upgradeSystem.GetUpgrade("Attack").GetUpgradeLevel());
        yield return null;
    }

    [UnityTest, Order(5)]
    public IEnumerator JSONWriteTest()
    {
        UpgradeSystem upgradeSystem = new UpgradeSystem();
        Upgrade testUpgrade = new Upgrade("Bomb Count", 1);
        upgradeSystem.AddUpgrade(testUpgrade);
        testUpgrade = new Upgrade("Speed", 2);
        upgradeSystem.AddUpgrade(testUpgrade);
        testUpgrade = new Upgrade("Range", 5);
        upgradeSystem.AddUpgrade(testUpgrade);
        UpgradeSystem.SaveUpgrades(upgradeSystem.GetUpgradeList());
        yield return null;
    }

    [UnityTest, Order(6)]
    public IEnumerator JSONReadTest()
    {
        UpgradeSystem upgradeSystem = new UpgradeSystem();
        int listCount = upgradeSystem.GetUpgradeCount();
        upgradeSystem.SetUpgradeList(UpgradeSystem.LoadUpgrades(upgradeSystem.GetUpgradeList()));
        Assert.AreNotEqual(listCount, upgradeSystem.GetUpgradeCount());
        yield return null;
    }
}

