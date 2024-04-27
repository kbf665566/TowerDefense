using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class TowerData 
{
    public int Id;
    public string Name;
    public List<TowerLevelData> TowerLevelData;
    public Sprite TowerIcon;
    public TowerType towerType;
    public string TowerInformation;
}

[Serializable]
public enum TowerType
{
    Normal,
    Support,
    AOE,
    Money
}
[Serializable]
public class TowerLevelData
{
    public float Damage;
    public float ShootRange;
    public float FireRate;

    //Support用
    public float BuffDamage;
    public float BuffRange;
    public float BuffFireRate;

    //Money用
    public int GetMoney;

    public float SlowAmount;
    
    //價錢
    public int BuildUpgradeCost;
    public int SoldPrice;

    public GameObject towerPrefab;
}