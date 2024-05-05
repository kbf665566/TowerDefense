using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System;
using static UnityEngine.EventSystems.EventTrigger;

public class LevelEditorWindow : UnitEditorWindow
{
    private static LevelEditorWindow window;

    public static void Init()
    {
        // Get existing open window or if none, make a new one:
        window = (LevelEditorWindow)GetWindow(typeof(LevelEditorWindow));

        EditorDataManager.Init();
        selectID = -1;
    }

    private Vector2 scrollPos1;
    private Vector2 scrollPos2;

    private static int selectID = 0;
    private float contentHeight = 0;
    private float contentWidth = 0;

    private Rect listVisibleRect;
    private Rect listContentRect;

    private int deleteLevelID = -1;
    private bool minimiseList = false;

    private static List<WaveData> waveList = new List<WaveData>();
    private static List<int> waveIDList = new List<int>();

    private int deleteWaveID = -1;
    private int deleteEnemyID = -1;
    /// <summary> 儲存按了哪個Wave的敵人的Remove </summary>
    private int clickWaveID = -1;

    void SelectLevel(int ID)
    {
        selectID = ID;
        GUI.FocusControl("");

        if (selectID * 35 < scrollPos1.y)
            scrollPos1.y = selectID * 35;
        if (selectID * 35 > scrollPos1.y + listVisibleRect.height - 40) 
            scrollPos1.y = selectID * 35 - listVisibleRect.height + 40;

        var waves = EditorDataManager.LevelDataList[selectID];
        waveIDList.Clear();
        for (int i = 0; i < waves.WaveList.Count; i++)
        {
            if (waveIDList.Contains(waves.WaveList[i].Id))
                waves.WaveList[i].Id = EditorDataManager.GenerateNewID(waveIDList);
            waveIDList.Add(waves.WaveList[i].Id);
        }
    }

    void OnGUI()
    {
        if (window == null) Init();

        List<Waves> levelDataList = EditorDataManager.LevelDataList;

        EditorGUI.LabelField(new Rect(5, 7, 150, 17), "Add new Level:");
        Waves newLevel = null;

        if (GUI.Button(new Rect(100, 7, 140, 17), "Add"))
        {
            newLevel = new Waves();
            newLevel.LevelName = "NewLevel";
            int newSelectID = EditorDataManager.AddNewLevel(newLevel);
            if (newSelectID != -1) 
                SelectLevel(newSelectID);
        }




        float startX = 5;
        float startY = 50;

        //放大縮小左邊的List
        if (minimiseList)
        {
            if (GUI.Button(new Rect(startX, startY - 20, 30, 18), ">>")) 
                minimiseList = false;
        }
        else
        {
            if (GUI.Button(new Rect(startX, startY - 20, 30, 18), "<<")) 
                minimiseList = true;
        }
        Vector2 v2 = DrawLevelList(startX, startY, levelDataList);

        startX = v2.x + 25;

        if (levelDataList.Count == 0) return;

        if (selectID == -1)
            SelectLevel(0);

        selectID = Mathf.Clamp(selectID, 0, levelDataList.Count - 1);

        Rect visibleRect = new Rect(startX, startY, window.position.width - startX - 10, window.position.height - startY - 5);
        Rect contentRect = new Rect(startX, startY, contentWidth - startY, contentHeight);


        scrollPos2 = GUI.BeginScrollView(visibleRect, scrollPos2, contentRect);

        v2 = DrawLevelConfigurator(startX, startY, levelDataList);
        contentWidth = v2.x;
        contentHeight = v2.y;

        GUI.EndScrollView();


        if (GUI.changed) 
            EditorDataManager.SetDirtyLevel();
    }

    private Vector2 DrawLevelList(float startX, float startY, List<Waves> levelDataList)
    {

        float width = 260;
        if (minimiseList) 
            width = 60;

        if (!minimiseList)
        {
            if (GUI.Button(new Rect(startX + 180, startY - 20, 45, 18), "up"))
            {
                if (selectID > 0)
                {
                    Waves waves = levelDataList[selectID];
                    levelDataList[selectID] = levelDataList[selectID - 1];
                    levelDataList[selectID - 1] = waves;
                    selectID -= 1;

                    if (selectID * 35 < scrollPos1.y) 
                        scrollPos1.y = selectID * 35;
                }
            }
            if (GUI.Button(new Rect(startX + 227, startY - 20, 45, 18), "down"))
            {
                if (selectID < levelDataList.Count - 1)
                {
                    Waves waves = levelDataList[selectID];
                    levelDataList[selectID] = levelDataList[selectID + 1];
                    levelDataList[selectID + 1] = waves;
                    selectID += 1;

                    if (listVisibleRect.height - 35 < selectID * 35) 
                        scrollPos1.y = (selectID + 1) * 35 - listVisibleRect.height + 5;
                }
            }
        }


        listVisibleRect = new Rect(startX, startY, width + 15, window.position.height - startY - 5);
        listContentRect = new Rect(startX, startY, width, levelDataList.Count * 35 + 5);

        GUI.color = new Color(.8f, .8f, .8f, 1f);
        GUI.Box(listVisibleRect, "");
        GUI.color = Color.white;

        scrollPos1 = GUI.BeginScrollView(listVisibleRect, scrollPos1, listContentRect);


        startY += 5; startX += 5;

        for (int i = 0; i < levelDataList.Count; i++)
        {
            if (minimiseList)
            {
                if (selectID == i) 
                    GUI.color = new Color(0, 1f, 1f, 1f);
                if (GUI.Button(new Rect(startX, startY + (i * 35), 30, 30), "")) 
                    SelectLevel(i);
                GUI.color = Color.white;

                continue;
            }

            if (selectID == i) 
                GUI.color = new Color(0, 1f, 1f, 1f);
            if (GUI.Button(new Rect(startX + 35, startY + (i * 35), 150, 30), levelDataList[i].LevelName)) 
                SelectLevel(i);
            GUI.color = Color.white;

            if (deleteLevelID == i)
            {

                if (GUI.Button(new Rect(startX + 190, startY + (i * 35), 60, 15), "cancel"))
                    deleteLevelID = -1;

                GUI.color = Color.red;
                if (GUI.Button(new Rect(startX + 190, startY + (i * 35) + 15, 60, 15), "confirm"))
                {
                    if (selectID >= deleteLevelID) 
                        SelectLevel(Mathf.Max(0, selectID - 1));
                    EditorDataManager.RemoveLevel(deleteLevelID);
                    deleteLevelID = -1;
                }
                GUI.color = Color.white;
            }
            else
            {
                if (GUI.Button(new Rect(startX + 190, startY + (i * 35), 60, 15), "remove")) 
                    deleteLevelID = i;
            }
        }

        GUI.EndScrollView();

        return new Vector2(startX + width, startY);
    }

    Vector3 v3 = Vector3.zero;

    private Vector2 DrawLevelConfigurator(float startX, float startY, List<Waves> levelDataList)
    {
        Waves waves = levelDataList[selectID];

        float cachedY = startY;
        float cachedX = startX;

        cont = new GUIContent("關卡名稱:");
        EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        EditorGUI.TextField(new Rect(startX + spaceX - 35, startY, width - 5, height), waves.LevelName);

        startX = cachedX;
        spaceX = 110;
        startY += 30;

        cont = new GUIContent("關卡內容:", "設定每波的敵人");
        EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);


        EditorGUI.LabelField(new Rect(startX, startY += spaceY, 150, 17), "Add new Wave:");

        //新增新的一波敵人
        if (GUI.Button(new Rect(startX + spaceX, startY, 140, 17), "Add Wave"))
        {
            WaveData newWave = new WaveData();
            newWave.WaveEnemyList = new List<WaveEnemy>();
            newWave.Id = EditorDataManager.GenerateNewID(waveIDList);
            waves.WaveList.Add(newWave);
        }

        for (int i = 0; i < waves.WaveList.Count; i++)
        {
            cont = new GUIContent("第" +( i + 1) +"波:");
            EditorGUI.LabelField(new Rect(startX + 5, startY += spaceY * 2, width, height), cont);

            //新增新的敵人
            if (GUI.Button(new Rect(startX + 50, startY + 2, 100, 17), "Add Enemy"))
            {
                WaveEnemy newWaveEnemy = new WaveEnemy();
                waves.WaveList[i].WaveEnemyList.Add(newWaveEnemy);
            }
            if (deleteWaveID == i)
            {

                if (GUI.Button(new Rect(startX + 170, startY - 6, 60, 15), "cancel")) 
                    deleteWaveID = -1;

                GUI.color = Color.red;
                if (GUI.Button(new Rect(startX + 170, startY + 12, 60, 15), "confirm"))
                {
                    waves.WaveList.RemoveAt(i);
                    deleteWaveID = -1;
                    continue;
                }
                GUI.color = Color.white;
            }
            else
            {
                if (GUI.Button(new Rect(startX + 170, startY + 2, 100, 17), "Remove Wave"))
                    deleteWaveID = i;
            }
           var temp = DrawWaveEnemyList(startX + 15,startY + spaceY + 10, waves.WaveList[i].Id, waves.WaveList[i].WaveEnemyList);
            startY = temp.y;
        }

        return new Vector2(startX, startY);
    }


    private Vector2 DrawWaveEnemyList(float startX, float startY,int Id, List<WaveEnemy> enemyList)
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            cont = new GUIContent("Enemy Prefab:", "");
            EditorGUI.LabelField(new Rect(startX, startY += (spaceY + 3) * i, width, height), cont);
            enemyList[i].EnemyPrefab = (GameObject)EditorGUI.ObjectField(new Rect(startX += spaceX -20, startY, 80, height), enemyList[i].EnemyPrefab, typeof(GameObject), false);

            cont = new GUIContent("產生的數量:");
            EditorGUI.LabelField(new Rect(startX += spaceX, startY , width, height), cont);
            enemyList[i].SpawnAmount = EditorGUI.IntField(new Rect(startX  += spaceX - 30, startY, 40, height), enemyList[i].SpawnAmount);

            cont = new GUIContent("產生間隔:");
            EditorGUI.LabelField(new Rect(startX += spaceX - 50, startY, width, height), cont);
            enemyList[i].SpawnInterval = EditorGUI.FloatField(new Rect(startX += spaceX - 50, startY, 40, height), enemyList[i].SpawnInterval);

            cont = new GUIContent("PathId:","要走哪個路徑");
            EditorGUI.LabelField(new Rect(startX += spaceX - 50, startY, width, height), cont);
            enemyList[i].PathId = EditorGUI.IntField(new Rect(startX += spaceX - 60, startY, 40, height), enemyList[i].PathId);

            if (deleteEnemyID == i && Id == clickWaveID)
            {

                if (GUI.Button(new Rect(startX + spaceX  + 30, startY, 60, 15), "cancel"))
                {
                    deleteEnemyID = -1;
                    clickWaveID = -1;
                }

                GUI.color = Color.red;
                if (GUI.Button(new Rect(startX + spaceX - 30, startY, 60, 15), "confirm"))
                {
                    enemyList.RemoveAt(i);
                    deleteEnemyID = -1;
                    clickWaveID = -1;
                }
                GUI.color = Color.white;
            }
            else
            {
                if (GUI.Button(new Rect(startX + spaceX - 30, startY, 100, 17), "Remove Enemy"))
                {
                    deleteEnemyID = i;
                    clickWaveID = Id;
                }
            }
        }

        return new Vector2(startX, startY);
    }
}
