using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
[CreateAssetMenu(menuName = "TowerDefense/LevelData")]
/// <summary> 記錄每個關卡配置 </summary>
public class LevelData : ScriptableObject
{
    public List<LevelAllWaves> LevelDataList;

    public LevelAllWaves GetData(int id)
    {
        for (int i = 0; i < LevelDataList.Count; i++)
        {
            if (LevelDataList[i].Id == id)
                return LevelDataList[i];
        }
        return null;
    }
}
