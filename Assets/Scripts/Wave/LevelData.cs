using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
[CreateAssetMenu(menuName = "TowerDefense/LevelData")]
/// <summary> �O���C�����d�t�m </summary>
public class LevelData : ScriptableObject
{
    public List<Waves> LevelDataList;
}
