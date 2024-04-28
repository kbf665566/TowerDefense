using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System;
using UnityEditor.PackageManager.UI;

public class EnemyEditorWindow : UnitEditorWindow
{
    private static TowerEditorWindow window;

    public static void Init()
    {
        // Get existing open window or if none, make a new one:
        window = (TowerEditorWindow)EditorWindow.GetWindow(typeof(TowerEditorWindow));

        EditorDataManager.Init();

    }

    private Vector2 scrollPos1;
    private Vector2 scrollPos2;

    private static int selectID = 0;
    private float contentHeight = 0;
    private float contentWidth = 0;

    private Rect listVisibleRect;
    private Rect listContentRect;

    private int deleteID = -1;
    private bool minimiseList = false;

    void SelectEnemy(int ID)
    {
        selectID = ID;
        GUI.FocusControl("");

        if (selectID * 35 < scrollPos1.y) scrollPos1.y = selectID * 35;
        if (selectID * 35 > scrollPos1.y + listVisibleRect.height - 40) scrollPos1.y = selectID * 35 - listVisibleRect.height + 40;
    }

    void OnGUI()
    {
        //if (window == null) Init();

        //List<EnemyData> enemyList = EditorDataManager.TowerList;

        //if (GUI.Button(new Rect(window.position.width - 120, 5, 100, 25), "Save")) EditorDataManager.SetDirtyTower();

        //EditorGUI.LabelField(new Rect(5, 7, 150, 17), "Add new Enemy:");
        //EnemyData newEnemy = null;

        //if (GUI.Button(new Rect(100, 7, 140, 17), "Add"))
        //{
        //    newEnemy = new EnemyData();
        //    newEnemy.Name = "NewEnemy";
        //    int newSelectID = EditorDataManager.AddNewTower(newEnemy);
        //    if (newSelectID != -1) SelectEnemy(newSelectID);
        //}




        //float startX = 5;
        //float startY = 50;

        //if (minimiseList)
        //{
        //    if (GUI.Button(new Rect(startX, startY - 20, 30, 18), ">>")) minimiseList = false;
        //}
        //else
        //{
        //    if (GUI.Button(new Rect(startX, startY - 20, 30, 18), "<<")) minimiseList = true;
        //}
        //Vector2 v2 = DrawTowerList(startX, startY, towerList);

        //startX = v2.x + 25;

        //if (towerList.Count == 0) return;

        //selectID = Mathf.Clamp(selectID, 0, towerList.Count - 1);

        //cont = new GUIContent("Tower Prefab:");
        //EditorGUI.LabelField(new Rect(startX, startY, width, height), cont);

        //if (towerList[selectID].TowerLevelData == null)
        //{
        //    towerList[selectID].TowerLevelData = new List<TowerLevelData>();
        //    towerList[selectID].TowerLevelData.Add(new TowerLevelData());
        //}

        //EditorGUI.ObjectField(new Rect(startX + 90, startY, 185, height), towerList[selectID].TowerLevelData[0].towerPrefab, typeof(GameObject), false);

        //startY += spaceY + 10;


        //Rect visibleRect = new Rect(startX, startY, window.position.width - startX - 10, window.position.height - startY - 5);
        //Rect contentRect = new Rect(startX, startY, contentWidth - startY, contentHeight);


        //scrollPos2 = GUI.BeginScrollView(visibleRect, scrollPos2, contentRect);

        //v2 = DrawTowerConfigurator(startX, startY, towerList);
        //contentWidth = v2.x;
        //contentHeight = v2.y;

        //GUI.EndScrollView();


        //if (GUI.changed) EditorDataManager.SetDirtyTower();
    }
}
