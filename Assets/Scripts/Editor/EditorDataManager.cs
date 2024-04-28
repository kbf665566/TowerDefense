using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class EditorDataManager : EditorWindow
{
    private static bool init = false;
    public static void Init()
    {
        if (init) return;
        LoadTower();
        LoadEnemy();
    }

    private static Towers towers;
    private static List<TowerData> towerList = new List<TowerData>();
    public static List<TowerData> TowerList => towerList;
    private static List<int> towerIDList = new List<int>();
    public static List<int> TowerIDList => towerIDList;
    private static string[] towerNameList = new string[0];
    public static string[] TowerNameList => towerNameList;
    private static string towerDatapath = "Assets/Data/TowerData.asset";

    /// <summary>
    /// Ū��Tower���
    /// </summary>
    private static void LoadTower()
    {
        //���J���w���|���ɮ�
        towers = AssetDatabase.LoadAssetAtPath<Towers>(towerDatapath);
        towerList = towers.TowerList;

        for (int i = 0; i < towerList.Count; i++)
        {
            if (towerList[i] != null)
            {
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
        for (int i = 0; i < tempList.Count; i++) towerNameList[i] = tempList[i];
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

    private static Enemies enemies;
    private static List<EnemyData> enemyList = new List<EnemyData>();
    public static List<EnemyData> EnemyList => enemyList;
    private static List<int> enemyIDList = new List<int>();
    public static List<int> EnemyIDList => enemyIDList;
    private static string[] enemyNameList = new string[0];
    public static string[] EnemyNameList => enemyNameList;
    private static string enemyDatapath = "Assets/Data/EnemyData.asset";


    /// <summary>
    /// Ū��Enemy���
    /// </summary>
    private static void LoadEnemy()
    {
        //���J���w���|���ɮ�
        enemies = AssetDatabase.LoadAssetAtPath<Enemies>(enemyDatapath);
        enemyList = enemies.EnemyList;

        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i] != null)
            {
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
        for (int i = 0; i < tempList.Count; i++) enemyNameList[i] = tempList[i];
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

    private static int GenerateNewID(List<int> list)
    {
        int ID = 0;
        while (list.Contains(ID)) ID += 1;
        return ID;
    }
}
