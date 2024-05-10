using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "TowerDefense/MapData")]
public class Maps : ScriptableObject
{
    public List<MapData> MapDataList = new List<MapData>();

    public MapData GetData(int id)
    {
        for(int i =0;i< MapDataList.Count;i++)
        {
            if (MapDataList[i].Id == id)
                return MapDataList[i];
        }
        return null;
    }
}
