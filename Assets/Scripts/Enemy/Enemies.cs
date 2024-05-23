using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "TowerDefense/Enemy Data")]
public class Enemies : ScriptableObject
{
    public List<EnemyData> EnemyList;

    public EnemyData GetData(int id)
    {
        for (int i = 0; i < EnemyList.Count; i++)
        {
            if (EnemyList[i].Id == id)
                return EnemyList[i];
        }
        return null;
    }
}
