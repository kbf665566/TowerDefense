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
    /// <summary> 要走的路徑 </summary>
    public int PathId;
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
    public List<WaveData> WaveList;
}