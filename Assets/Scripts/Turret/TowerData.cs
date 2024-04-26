using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TowerData 
{
    public TowerLevelData TowerLevelData;
    public TowerType towerType;
    public string TowerInformation;
}

[Serializable]
public enum TowerType
{
    Attack,
    Support
}
[Serializable]
public class TowerLevelData
{
    public float Damage;
    public float ShootRange;
    public float FireRate;
    public int BuildUpgradeCost;
    public int SoldPrice;
}