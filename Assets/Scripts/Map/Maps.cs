using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "TowerDefense/MapData")]
public class Maps : ScriptableObject
{
    public List<MapData> MapDataList = new List<MapData>();
}
