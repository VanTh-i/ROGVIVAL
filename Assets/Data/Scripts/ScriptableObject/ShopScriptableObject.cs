using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopScriptableObject", menuName = "ScriptableObject/Shop")]
public class ShopScriptableObject : ScriptableObject
{
    public UpgradeItem upgradeItems;
}
[System.Serializable]
public class UpgradeItem
{
    public string upgradeName;
    public UpgradeInfo[] upgradelevels;
}
[System.Serializable]
public class UpgradeInfo
{
    public int upgradeCost;
    public float upgradeValue;
}