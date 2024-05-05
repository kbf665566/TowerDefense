using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapData 
{
    public int Id;
    /// <summary> UI顯示用 </summary>
    public string MapName;
    /// <summary> 程式讀取用 </summary>
    public string SceneName;
    public Sprite MapIcon;
    public Vector2Short MapSize;
    public List<EnemyPathData> EnemyPathList = new List<EnemyPathData>();
    public List<BlockData> BlockGridList;
    public int WavesId;
    [HideInInspector]
    public Waves Waves;
}

[Serializable]
public class EnemyPathData
{
    public int Id;
    public List<EnemyPath> Path = new List<EnemyPath>();
}
[Serializable]
public class EnemyPath
{
    public Vector2Short GridPos;
}

[Serializable]
public class BlockData
{
    public GameObject BlockPrefab;
    public Vector2Short GridPos;
    public Vector2Short Size;
}