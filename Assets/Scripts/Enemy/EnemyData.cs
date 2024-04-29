using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyData 
{
    public int Id;
    public string Name;
    public Sprite EnemyIcon;
    public float HP;
    public float Speed;
    /// <summary> �i�H���o���귽 </summary>
    public int Value;
    /// <summary> �缾�a�y�����ˮ` </summary>
    public int Damage;
    /// <summary> �K�̽w�t </summary>
    public bool ImmuneSlow;
    /// <summary> �K�����w </summary>
    public bool ImmuneStun;
    public GameObject EnemyPrefab;

}
