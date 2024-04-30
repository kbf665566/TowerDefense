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
    /// <summary> �x�s���F����Wave���ĤH��Remove </summary>
    private int clickWaveID = -1;

    void SelectLevel(int ID)
    {
        selectID = ID;
        GUI.FocusControl("");

        if (selectID * 35 < scrollPos1.y)
            scrollPos1.y = selectID * 35;
        if (selectID * 35 > scrollPos1.y + listVisibleRect.height - 40) 
            scrollPos1.y = selectID * 35 - listVisibleRect.height + 40;
    }

    void OnGUI()
    {
        if (window == null) Init();

        List<Waves> levelDataList = EditorDataManager.LevelDataList;

        if (GUI.Button(new Rect(window.position.width - 120, 5, 100, 25), "Save")) 
            EditorDataManager.SetDirtyLevel();

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

        //��j�Y�p���䪺List
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
            DrawSprite(new Rect(startX, startY + (i * 35), 30, 30), levelDataList[i].LevelIcon);
           
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

        waveIDList.Clear();
        for(int i = 0;i<waves.WaveList.Count;i++)
        {
            if (waveIDList.Contains(waves.WaveList[i].Id))
                waves.WaveList[i].Id = EditorDataManager.GenerateNewID(waveIDList);
            waveIDList.Add(waves.WaveList[i].Id);
        }

        float cachedY = startY;
        float cachedX = startX;

        v3 = DrawIconAndName(waves.LevelName, waves.LevelIcon, startX, startY); startY = v3.y;


        startX = cachedX;
        spaceX = 110;
        startY += 30;

        cont = new GUIContent("���d���e:", "�]�w�C�i���ĤH");
        EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);


        EditorGUI.LabelField(new Rect(startX, startY += spaceY, 150, 17), "Add new Wave:");

        //�s�W�s���@�i�ĤH
        if (GUI.Button(new Rect(startX + spaceX, startY, 140, 17), "Add Wave"))
        {
            WaveData newWave = new WaveData();
            newWave.WaveEnemyList = new List<WaveEnemy>();
            newWave.Id = EditorDataManager.GenerateNewID(waveIDList);
            waves.WaveList.Add(newWave);
        }

        for (int i = 0; i < waves.WaveList.Count; i++)
        {
            cont = new GUIContent("��" +( i + 1) +"�i:");
            EditorGUI.LabelField(new Rect(startX + 5, startY += spaceY * 2, width, height), cont);

            //�s�W�s���ĤH
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
            enemyList[i].EnemyPrefab = (GameObject)EditorGUI.ObjectField(new Rect(startX + spaceX -20 , startY, 80, height), enemyList[i].EnemyPrefab, typeof(GameObject), false);

            cont = new GUIContent("���ͪ��ƶq:");
            EditorGUI.LabelField(new Rect(startX + spaceX + width - 70, startY , width, height), cont);
            enemyList[i].SpawnAmount = EditorGUI.IntField(new Rect(startX + spaceX + width, startY, 40, height), enemyList[i].SpawnAmount);

            cont = new GUIContent("���Ͷ��j:");
            EditorGUI.LabelField(new Rect(startX + spaceX * 2 + width - 60, startY, width, height), cont);
            enemyList[i].SpawnInterval = EditorGUI.FloatField(new Rect(startX + spaceX * 2 + width, startY, 40, height), enemyList[i].SpawnInterval);

            if (deleteEnemyID == i && Id == clickWaveID)
            {

                if (GUI.Button(new Rect(startX + spaceX * 4 + 50, startY, 60, 15), "cancel"))
                {
                    deleteEnemyID = -1;
                    clickWaveID = -1;
                }

                GUI.color = Color.red;
                if (GUI.Button(new Rect(startX + spaceX * 4 - 10, startY, 60, 15), "confirm"))
                {
                    enemyList.RemoveAt(i);
                    deleteEnemyID = -1;
                    clickWaveID = -1;
                }
                GUI.color = Color.white;
            }
            else
            {
                if (GUI.Button(new Rect(startX + spaceX * 4 - 10, startY, 100, 17), "Remove Enemy"))
                {
                    deleteEnemyID = i;
                    clickWaveID = Id;
                }
            }
        }

        return new Vector2(startX, startY);
    }
}
