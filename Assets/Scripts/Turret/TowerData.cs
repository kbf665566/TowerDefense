﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using JetBrains.Annotations;

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
    public bool IsNeedTurn;
    public DebuffType Debuff;
    public string TowerInformation;
    public Vector2Short TowerSize;
    //-----音效---------
    public AudioClip SpecialMusic;
    //------------------

    //-----特效---------
    public GameEffectType AttackParticle;
    public GameEffectType NormalParticle;
    public GameEffectType BuildParticle;
    public GameEffectType RemoveParticle;
    //------------------

    public TowerAttackMode AttackMode;

    public TowerInLevel TowerPrefab;
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
    Money,
    Special,
}

[Serializable]//新增攻擊最後一個
public enum TowerAttackMode
{
    None = 0,
    /// <summary> 最近 </summary>
    Nearest = 1 << 0,
    /// <summary> 第一個 </summary>
    First = 1 << 1,
    /// <summary> 最後一個 </summary>
    Last = 1 << 2,
    /// <summary> 總血量最高 </summary>
    HighestHP = 1 << 3,
    /// <summary> 當前血量最少 </summary>
    Weakest = 1 << 4,
    /// <summary> 攻擊特定地點 </summary>
    Fixedpoint = 1 << 5,
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
    public GameObject TowerBullet;
    /// <summary> 發射物速度 </summary>
    public float BulletSpeed;
    /// <summary> 發射物爆炸範圍 </summary>
    public float BulletExplosionRadius;

    /// <summary> 一次發射的發射物數量 </summary>
    public int BulletAmount;
    /// <summary> 發射物射擊方式 </summary>
    public BulletShootType BulletShoot;
    /// <summary> 發射物移動方式 </summary>
    public BulletMoveType BulletMove;
    /// <summary> 發射物存在時間 </summary>
    public float BulletExistTime;

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
}

[Serializable]
public enum DebuffType
{
    None,
    Stun,
    Slow,
}
[Serializable]
public enum BulletShootType
{
    /// <summary> 向前方發射 </summary>
    Front,
    /// <summary> 以自身為範圍 </summary>
    Self,
}
[Serializable]
public enum BulletMoveType
{
    /// <summary> 擊中敵人即消失 </summary>
    Normal,
    /// <summary> 追蹤 </summary>
    Follow,
    /// <summary> 貫穿 </summary>
    Penetrate
}