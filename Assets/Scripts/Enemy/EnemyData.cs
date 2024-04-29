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
    /// <summary> 可以取得的資源 </summary>
    public int Value;
    /// <summary> 對玩家造成的傷害 </summary>
    public int Damage;
    /// <summary> 免疫緩速 </summary>
    public bool ImmuneSlow;
    /// <summary> 免疫擊暈 </summary>
    public bool ImmuneStun;
    public GameObject EnemyPrefab;

}
