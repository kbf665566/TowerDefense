using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEditor;
using UnityEngine;

public class EditorDataManager : EditorWindow
{
    private static bool init = false;
    public static void Init()
    {
        if (init) return;
        LoadTower();

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
    /// 讀取Tower資料
    /// </summary>
    private static void LoadTower()
    {
        //載入指定路徑的檔案
        towers = AssetDatabase.LoadAssetAtPath<Towers>(towerDatapath);
        towerList = towers.TowerList;

        for (int i = 0; i < towerList.Count; i++)
        {
            //towerList[i].prefabID=i;
            if (towerList[i] != null)
            {
                towerIDList.Add(towerList[i].Id);
                //if (towerList[i].stats.Count == 0) towerList[i].stats.Add(new UnitStat());
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
        //for (int i = 0; i < towerList.Count; i++) EditorUtility.SetDirty(towerList[i]);
    }

    public static int AddNewTower(TowerData newTower)
    {
        if (towerList.Contains(newTower)) return -1;

        int ID = GenerateNewID(towerIDList);
        newTower.Id = ID;
        towerIDList.Add(ID);
        towerList.Add(newTower);

        UpdateTowerNameList();

        //if (newTower.stats.Count == 0) newTower.stats.Add(new UnitStat());

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


    private static int GenerateNewID(List<int> list)
    {
        int ID = 0;
        while (list.Contains(ID)) ID += 1;
        return ID;
    }
}
