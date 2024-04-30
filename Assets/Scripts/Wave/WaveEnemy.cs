using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
/// <summary> �C�@�ռĤH </summary>
public class WaveEnemy
{
    public GameObject EnemyPrefab;
    /// <summary> �@�����ͪ��q </summary>
    public int SpawnAmount;
    /// <summary> ���ͪ����j </summary>
    public float SpawnInterval = 0.5f;
}

[Serializable]
/// <summary> �C�@�i���Ҧ��ĤH </summary>
public class WaveData
{
    public int Id;
    public List<WaveEnemy> WaveEnemyList;
}

[Serializable]
/// <summary> �C�@�����Ҧ��ĤH </summary>
public class Waves
{
    public int Id;
    public string LevelName;
    public Sprite LevelIcon;
    public List<WaveData> WaveList;
}