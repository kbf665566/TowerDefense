using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyData 
{
    public int Id;
    public string Name;
    public float HP;
    public float Speed;
    public int Damage;
    public bool ImmuneSlow;
    public bool ImmuneStun;
}
