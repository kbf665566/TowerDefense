using Codice.Client.BaseCommands;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class EditorDataManager : EditorWindow
{
    private static bool init = false;
    private static Texture2D consoleBackground;
    public static GUIStyle BoxStyle;
    public static void Init()
    {
        if (init) return;

        consoleBackground = new Texture2D(1, 1, TextureFormat.RGBAFloat, false);
        consoleBackground.SetPixel(0, 0, new Color(1, 1, 1, 1f));
        consoleBackground.Apply();

        BoxStyle = new GUIStyle(GUIStyle.none);
        BoxStyle.normal.background = consoleBackground;

        LoadTower();
        LoadEnemy();
        LoadLevel();
        LoadMap();
    }

    #region 塔
    private static Towers towers;
    private static List<TowerData> towerList = new List<TowerData>();
    public static List<TowerData> TowerList => towerList;
    private static List<int> towerIDList = new List<int>();
    public static List<int> TowerIDList => towerIDList;
    private static string[] towerNameList = new string[0];
    public static string[] TowerNameList => towerNameList;
    private static string towerDatapath = "Assets/Data/TowerData.asset";

    /// <summary>
    /// 讀取Tower資料
    /// </summary>
    private static void LoadTower()
    {
        //載入指定路徑的檔案
        towers = AssetDatabase.LoadAssetAtPath<Towers>(towerDatapath);
        towerList = towers.TowerList;

        for (int i = 0; i < towerList.Count; i++)
        {
            if (towerList[i] != null)
            {
                if (towerIDList.Contains(towerList[i].Id))
                    towerList[i].Id = GenerateNewID(towerIDList);
                towerIDList.Add(towerList[i].Id);
            }
            else
            {
                towerList.RemoveAt(i);
                i -= 1;
            }
        }

        UpdateTowerNameList();
    }
    private static void UpdateTowerNameList()
    {
        List<string> tempList = new List<string>();
        tempList.Add(" - ");
        for (int i = 0; i < towerList.Count; i++)
        {
            string name = towerList[i].Name;
            while (tempList.Contains(name)) name += ".";
            tempList.Add(name);
        }

        towerNameList = new string[tempList.Count];
        for (int i = 0; i < tempList.Count; i++)
            towerNameList[i] = tempList[i];
    }


    public static void SetDirtyTower()
    {
        EditorUtility.SetDirty(towers);
    }

    public static int AddNewTower(TowerData newTower)
    {
        if (towerList.Contains(newTower)) return -1;

        int ID = GenerateNewID(towerIDList);
        newTower.Id = ID;
        towerIDList.Add(ID);
        towerList.Add(newTower);

        UpdateTowerNameList();

        SetDirtyTower();

        return towerList.Count - 1;
    }
    public static void RemoveTower(int listID)
    {
        towerIDList.Remove(towerList[listID].Id);
        towerList.RemoveAt(listID);
        UpdateTowerNameList();
        SetDirtyTower();
    }
    #endregion

    #region 敵人
    private static Enemies enemies;
    private static List<EnemyData> enemyList = new List<EnemyData>();
    public static List<EnemyData> EnemyList => enemyList;
    private static List<int> enemyIDList = new List<int>();
    public static List<int> EnemyIDList => enemyIDList;
    private static string[] enemyNameList = new string[0];
    public static string[] EnemyNameList => enemyNameList;
    private static string enemyDatapath = "Assets/Data/EnemyData.asset";


    /// <summary>
    /// 讀取Enemy資料
    /// </summary>
    private static void LoadEnemy()
    {
        //載入指定路徑的檔案
        enemies = AssetDatabase.LoadAssetAtPath<Enemies>(enemyDatapath);
        enemyList = enemies.EnemyList;

        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i] != null)
            {
                if (enemyIDList.Contains(enemyList[i].Id))
                    enemyList[i].Id = GenerateNewID(enemyIDList);
                enemyIDList.Add(enemyList[i].Id);
            }
            else
            {
                enemyList.RemoveAt(i);
                i -= 1;
            }
        }

        UpdateEnemyNameList();
    }
    private static void UpdateEnemyNameList()
    {
        List<string> tempList = new List<string>();
        tempList.Add(" - ");
        for (int i = 0; i < enemyList.Count; i++)
        {
            string name = enemyList[i].Name;
            while (tempList.Contains(name)) name += ".";
            tempList.Add(name);
        }

        enemyNameList = new string[tempList.Count];
        for (int i = 0; i < tempList.Count; i++) 
            enemyNameList[i] = tempList[i];
    }


    public static void SetDirtyEnemy()
    {
        EditorUtility.SetDirty(enemies);
    }

    public static int AddNewEnemy(EnemyData newEnemy)
    {
        if (enemyList.Contains(newEnemy)) return -1;

        int ID = GenerateNewID(enemyIDList);
        newEnemy.Id = ID;
        enemyIDList.Add(ID);
        enemyList.Add(newEnemy);

        UpdateEnemyNameList();

        SetDirtyEnemy();

        return enemyList.Count - 1;
    }
    public static void RemoveEnemy(int listID)
    {
        enemyIDList.Remove(enemyList[listID].Id);
        enemyList.RemoveAt(listID);
        UpdateEnemyNameList();
        SetDirtyEnemy();
    }
    #endregion

    #region 關卡

    private static LevelData levelData;
    private static List<Waves> levelDataList = new List<Waves>();
    public static List<Waves> LevelDataList => levelDataList;
    private static List<int> levelIDList = new List<int>();
    public static List<int> LevelIDList => levelIDList;
    private static string[] levelNameList = new string[0];
    public static string[] LevelNameList => levelNameList;
    private static string levelDatapath = "Assets/Data/LevelData.asset";

    /// <summary>
    /// 讀取Level資料
    /// </summary>
    private static void LoadLevel()
    {
        //載入指定路徑的檔案
        levelData = AssetDatabase.LoadAssetAtPath<LevelData>(levelDatapath);
        levelDataList = levelData.LevelDataList;

        for (int i = 0; i < levelDataList.Count; i++)
        {
            if (levelDataList[i] != null)
            {
                if (levelIDList.Contains(levelDataList[i].Id))
                    levelDataList[i].Id = GenerateNewID(levelIDList);
                levelIDList.Add(levelDataList[i].Id);
            }
            else
            {
                levelDataList.RemoveAt(i);
                i -= 1;
            }
        }
        UpdateLevelNameList();
    }
    private static void UpdateLevelNameList()
    {
        List<string> tempList = new List<string>();
        tempList.Add(" - ");
        for (int i = 0; i < levelDataList.Count; i++)
        {
            string name = levelDataList[i].LevelName;
            while (tempList.Contains(name)) name += ".";
            tempList.Add(name);
        }

        levelNameList = new string[tempList.Count];
        for (int i = 0; i < tempList.Count; i++)
            levelNameList[i] = tempList[i];
    }


    public static void SetDirtyLevel()
    {
        EditorUtility.SetDirty(levelData);
    }

    public static int AddNewLevel(Waves newLevel)
    {
        if (levelDataList.Contains(newLevel)) return -1;

        int ID = GenerateNewID(levelIDList);
        newLevel.Id = ID;
        levelIDList.Add(ID);
        levelDataList.Add(newLevel);

        UpdateLevelNameList();

        SetDirtyLevel();

        return levelDataList.Count - 1;
    }
    public static void RemoveLevel(int listID)
    {
        levelIDList.Remove(levelDataList[listID].Id);
        levelDataList.RemoveAt(listID);
        UpdateLevelNameList();
        SetDirtyLevel();
    }

    #endregion

    #region 地圖
    private static Maps maps;
    private static List<MapData> mapList = new List<MapData>();
    public static List<MapData> MapList => mapList;
    private static List<int> mapIDList = new List<int>();
    public static List<int> MapListIDList => mapIDList;
    private static string[] mapNameList = new string[0];
    public static string[] MapListNameList => mapNameList;
    private static string mapDatapath = "Assets/Data/MapData.asset";

    /// <summary>
    /// 讀取Map資料
    /// </summary>
    private static void LoadMap()
    {
        //載入指定路徑的檔案
        maps = AssetDatabase.LoadAssetAtPath<Maps>(mapDatapath);
        mapList = maps.MapDataList;

        for (int i = 0; i < mapList.Count; i++)
        {
            if (mapList[i] != null)
            {
                if (mapIDList.Contains(mapList[i].Id))
                    mapList[i].Id = GenerateNewID(mapIDList);
                mapIDList.Add(mapList[i].Id);
            }
            else
            {
                mapList.RemoveAt(i);
                i -= 1;
            }
        }

        UpdateMapNameList();
    }
    private static void UpdateMapNameList()
    {
        List<string> tempList = new List<string>();
        tempList.Add(" - ");
        for (int i = 0; i < mapList.Count; i++)
        {
            string name = mapList[i].MapName;
            while (tempList.Contains(name)) name += ".";
            tempList.Add(name);
        }

        mapNameList = new string[tempList.Count];
        for (int i = 0; i < tempList.Count; i++)
            mapNameList[i] = tempList[i];
    }


    public static void SetDirtyMap()
    {
        EditorUtility.SetDirty(maps);
    }

    public static int AddNewMap(MapData newmap)
    {
        if (mapList.Contains(newmap)) return -1;

        int ID = GenerateNewID(mapIDList);
        newmap.Id = ID;
        mapIDList.Add(ID);
        mapList.Add(newmap);

        UpdateMapNameList();

        SetDirtyMap();

        return mapList.Count - 1;
    }
    public static void RemoveMap(int listID)
    {
        mapIDList.Remove(mapList[listID].Id);
        mapList.RemoveAt(listID);
        UpdateMapNameList();
        SetDirtyMap();
    }
    #endregion


    public static int GenerateNewID(List<int> list)
    {
        int ID = 0;
        while (list.Contains(ID)) ID += 1;
        return ID;
    }
}
/*
 public static void DrawUILine(Color color, int thickness = 2, int padding = 10)
{
    Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding+thickness));
    r.height = thickness;
    r.y+=padding/2;
    r.x-=2;
    r.width +=6;
    EditorGUI.DrawRect(r, color);
}
 */