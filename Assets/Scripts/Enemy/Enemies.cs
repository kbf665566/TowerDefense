using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "TowerDefense/Enemy Data")]
public class Enemies : ScriptableObject
{
    public List<EnemyData> EnemyList;
}
