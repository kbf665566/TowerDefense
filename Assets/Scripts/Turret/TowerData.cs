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
    /// <summary> 是否使用發射物 </summary>
    public bool IsUseBullet;
    public bool CanSlowEnemy;
    public bool CanStunEnemy;
    public string TowerInformation;
}

[Serializable]
public enum TowerType
{
    /// <summary> 一般的攻擊塔 </summary>
    Normal,
    /// <summary> 輔助塔 </summary>
    Support,
    /// <summary> 範圍攻擊塔 </summary>
    AOE,
    /// <summary> 增加資源的塔 </summary>
    Money
}
[Serializable]
public class TowerLevelData
{
    /// <summary> 攻擊 </summary>
    public float Damage;
    /// <summary> 射程 </summary>
    public float ShootRange;
    /// <summary> 攻速 </summary>
    public float FireRate;
    /// <summary> 使用的發射物 </summary>
    public Bullet TowerBullet;
    /// <summary> 發射物速度 </summary>
    public float BulletSpeed;
    /// <summary> 發射物爆炸範圍 </summary>
    public float BulletExplosionRadius;

    //Support用
    /// <summary> Buff增加的攻擊 </summary>
    public float BuffAddDamage;
    /// <summary> Buff增加的範圍 </summary>
    public float BuffAddRange;
    /// <summary> Buff增加的攻速 </summary>
    public float BuffAddFireRate;

    //Money用
    /// <summary> 每次觸發能取得到的資源 </summary>
    public int GetMoney;

    //Debuff
    /// <summary> 緩速的程度 </summary>
    public float SlowAmount;
    /// <summary> 緩速的持續時間 </summary>
    public float SlowDuration;

    /// <summary> 擊暈的機率 </summary>
    public float StunProbability;
    /// <summary> 擊暈的持續時間 </summary>
    public float StunDuration;

    //價錢
    public int BuildUpgradeCost;
    public int SoldPrice;

    public GameObject towerPrefab;
}