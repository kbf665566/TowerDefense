using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
/// <summary> 每一組敵人 </summary>
public class WaveEnemy
{
    public GameObject EnemyPrefab;
    /// <summary> 一次產生的量 </summary>
    public int SpawnAmount;
    /// <summary> 產生的間隔 </summary>
    public float SpawnInterval = 0.5f;
}

[Serializable]
/// <summary> 每一波的所有敵人 </summary>
public class WaveData
{
    public int Id;
    public List<WaveEnemy> WaveEnemyList;
}

[Serializable]
/// <summary> 每一關的所有敵人 </summary>
public class Waves
{
    public int Id;
    public string LevelName;
    public Sprite LevelIcon;
    public List<WaveData> WaveList;
}