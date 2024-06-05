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
    /// <summary> 起始生命 </summary>
    public int StartLive;
    /// <summary> 起始資源 </summary>
    public int StartMoney;
    public Sprite MapIcon;
    public Vector2Short MapSize;
    public List<EnemyPathData> EnemyPathList = new List<EnemyPathData>();
    public List<BlockData> BlockGridList;
    public int WavesId;
    public AudioClip MapMusic;
    [HideInInspector]
    public LevelAllWaves Waves;



    public List<Vector2Short> GetBlockGridPosList(List<BlockData> blockDatas)
    {
        var blockGridPosList = new List<Vector2Short>();
        for (int i = 0; i < blockDatas.Count; i++)
        {
            for (int x = blockDatas[i].GridPos.x; x < blockDatas[i].GridPos.x + blockDatas[i].Size.x; x++)
            {
                for (int y = blockDatas[i].GridPos.y; y < blockDatas[i].GridPos.y + blockDatas[i].Size.y; y++)
                {
                    blockGridPosList.Add(new Vector2Short(x, y));
                }
            }
        }
        return blockGridPosList;
    }

    public List<Vector2Short> GetEnemyPathPosList(List<EnemyPathData> enemyPathDatas)
    {
        var enemyPathPosList = new List<Vector2Short>();
        for (int i = 0; i < enemyPathDatas.Count; i++)
        {
            for (int j = 0; j < enemyPathDatas[i].Path.Count - 1; j++)
            {
                if (enemyPathDatas[i].Path[j].GridPos.x < enemyPathDatas[i].Path[j + 1].GridPos.x)
                {
                    for (int x = enemyPathDatas[i].Path[j].GridPos.x; x <= enemyPathDatas[i].Path[j + 1].GridPos.x; x++)
                    {
                        if (enemyPathDatas[i].Path[j].GridPos.y < enemyPathDatas[i].Path[j + 1].GridPos.y)
                        {
                            for (int y = enemyPathDatas[i].Path[j].GridPos.y; y <= enemyPathDatas[i].Path[j + 1].GridPos.y; y++)
                            {
                                enemyPathPosList.Add(new Vector2Short(x, y));
                            }
                        }
                        else
                        {
                            for (int y = enemyPathDatas[i].Path[j].GridPos.y; y >= enemyPathDatas[i].Path[j + 1].GridPos.y; y--)
                            {
                                enemyPathPosList.Add(new Vector2Short(x, y));
                            }
                        }
                    }
                }
                else
                {
                    for (int x = enemyPathDatas[i].Path[j].GridPos.x; x >= enemyPathDatas[i].Path[j + 1].GridPos.x; x--)
                    {
                        if (enemyPathDatas[i].Path[j].GridPos.y < enemyPathDatas[i].Path[j + 1].GridPos.y)
                        {
                            for (int y = enemyPathDatas[i].Path[j].GridPos.y; y <= enemyPathDatas[i].Path[j + 1].GridPos.y; y++)
                            {
                                enemyPathPosList.Add(new Vector2Short(x, y));
                            }
                        }
                        else
                        {
                            for (int y = enemyPathDatas[i].Path[j].GridPos.y; y >= enemyPathDatas[i].Path[j + 1].GridPos.y; y--)
                            {
                                enemyPathPosList.Add(new Vector2Short(x, y));
                            }
                        }
                    }
                }
            }
        }
        return enemyPathPosList;
    }
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