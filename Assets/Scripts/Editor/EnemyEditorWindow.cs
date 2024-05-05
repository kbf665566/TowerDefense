using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System;
using UnityEditor.PackageManager.UI;

public class EnemyEditorWindow : UnitEditorWindow
{
    private static EnemyEditorWindow window;

    public static void Init()
    {
        // Get existing open window or if none, make a new one:
        window = (EnemyEditorWindow)GetWindow(typeof(EnemyEditorWindow));

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

        if (selectID * 35 < scrollPos1.y) 
            scrollPos1.y = selectID * 35;
        if (selectID * 35 > scrollPos1.y + listVisibleRect.height - 40) 
            scrollPos1.y = selectID * 35 - listVisibleRect.height + 40;
    }

    void OnGUI()
    {
        if (window == null) Init();

        List<EnemyData> enemyList = EditorDataManager.EnemyList;

        EditorGUI.LabelField(new Rect(5, 7, 150, 17), "Add new Enemy:");
        EnemyData newEnemy = null;

        if (GUI.Button(new Rect(100, 7, 140, 17), "Add"))
        {
            newEnemy = new EnemyData();
            newEnemy.Name = "NewEnemy";
            int newSelectID = EditorDataManager.AddNewEnemy(newEnemy);
            if (newSelectID != -1) 
                SelectEnemy(newSelectID);
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
        Vector2 v2 = DrawEnemyList(startX, startY, enemyList);

        startX = v2.x + 25;

        if (enemyList.Count == 0)
            return;

        selectID = Mathf.Clamp(selectID, 0, enemyList.Count - 1);

        cont = new GUIContent("Enemy Prefab:");
        EditorGUI.LabelField(new Rect(startX, startY, width, height), cont);
        EditorGUI.ObjectField(new Rect(startX + 90, startY, 185, height), enemyList[selectID].EnemyPrefab, typeof(GameObject), false);

        startY += spaceY + 10;


        Rect visibleRect = new Rect(startX, startY, window.position.width - startX - 10, window.position.height - startY - 5);
        Rect contentRect = new Rect(startX, startY, contentWidth - startY, contentHeight);


        scrollPos2 = GUI.BeginScrollView(visibleRect, scrollPos2, contentRect);

        v2 = DrawEnemyConfigurator(startX, startY, enemyList);
        contentWidth = v2.x;
        contentHeight = v2.y;

        GUI.EndScrollView();


        if (GUI.changed) EditorDataManager.SetDirtyEnemy();
    }

    Vector2 DrawEnemyList(float startX, float startY, List<EnemyData> enemyList)
    {

        float width = 260;
        if (minimiseList) width = 60;

        if (!minimiseList)
        {
            if (GUI.Button(new Rect(startX + 180, startY - 20, 45, 18), "up"))
            {
                if (selectID > 0)
                {
                    EnemyData enemy = enemyList[selectID];
                    enemyList[selectID] = enemyList[selectID - 1];
                    enemyList[selectID - 1] = enemy;
                    selectID -= 1;

                    if (selectID * 35 < scrollPos1.y)
                        scrollPos1.y = selectID * 35;
                }
            }
            if (GUI.Button(new Rect(startX + 227, startY - 20, 45, 18), "down"))
            {
                if (selectID < enemyList.Count - 1)
                {
                    EnemyData enemy = enemyList[selectID];
                    enemyList[selectID] = enemyList[selectID + 1];
                    enemyList[selectID + 1] = enemy;
                    selectID += 1;

                    if (listVisibleRect.height - 35 < selectID * 35)
                        scrollPos1.y = (selectID + 1) * 35 - listVisibleRect.height + 5;
                }
            }
        }


        listVisibleRect = new Rect(startX, startY, width + 15, window.position.height - startY - 5);
        listContentRect = new Rect(startX, startY, width, enemyList.Count * 35 + 5);

        GUI.color = new Color(.8f, .8f, .8f, 1f);
        GUI.Box(listVisibleRect, "");
        GUI.color = Color.white;

        scrollPos1 = GUI.BeginScrollView(listVisibleRect, scrollPos1, listContentRect);


        startY += 5; startX += 5;

        for (int i = 0; i < enemyList.Count; i++)
        {

            DrawSprite(new Rect(startX, startY + (i * 35), 30, 30), enemyList[i].EnemyIcon);

            if (minimiseList)
            {
                if (selectID == i)
                    GUI.color = new Color(0, 1f, 1f, 1f);
                if (GUI.Button(new Rect(startX + 35, startY + (i * 35), 30, 30), ""))
                    SelectEnemy(i);
                GUI.color = Color.white;

                continue;
            }



            if (selectID == i)
                GUI.color = new Color(0, 1f, 1f, 1f);
            if (GUI.Button(new Rect(startX + 35, startY + (i * 35), 150, 30), enemyList[i].Name))
                SelectEnemy(i);
            GUI.color = Color.white;

            if (deleteID == i)
            {

                if (GUI.Button(new Rect(startX + 190, startY + (i * 35), 60, 15), "cancel")) deleteID = -1;

                GUI.color = Color.red;
                if (GUI.Button(new Rect(startX + 190, startY + (i * 35) + 15, 60, 15), "confirm"))
                {
                    if (selectID >= deleteID)
                        SelectEnemy(Mathf.Max(0, selectID - 1));
                    if (selectID >= deleteID)
                        SelectEnemy(Mathf.Max(0, selectID - 1));
                    EditorDataManager.RemoveEnemy(deleteID);
                    deleteID = -1;
                }
                GUI.color = Color.white;
            }
            else
            {
                if (GUI.Button(new Rect(startX + 190, startY + (i * 35), 60, 15), "remove")) 
                    deleteID = i;
            }
        }

        GUI.EndScrollView();

        return new Vector2(startX + width, startY);
    }

    Vector3 v3 = Vector3.zero;

    Vector2 DrawEnemyConfigurator(float startX, float startY, List<EnemyData> enemyList)
    {
        EnemyData enemy = enemyList[selectID];

        float cachedY = startY;
        float cachedX = startX;

        v3 = DrawIconAndName(enemy.Name, enemy.EnemyIcon, startX, startY); startY = v3.y;

        startX = cachedX;
        spaceX = 110;
        startY += 30;

        cont = new GUIContent("是否免疫緩速:");
        EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        enemy.ImmuneSlow = EditorGUI.Toggle(new Rect(startX + spaceX, startY, 40, height), enemy.ImmuneSlow);

        cont = new GUIContent("是否免疫擊暈:");
        EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        enemy.ImmuneStun = EditorGUI.Toggle(new Rect(startX + spaceX, startY, 40, height), enemy.ImmuneStun);

        startY = cachedY;
        startX += spaceX * 3;
        cont = new GUIContent("HP:");
        EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        enemy.HP = EditorGUI.FloatField(new Rect(startX + spaceX, startY, 40, height), enemy.HP);

        cont = new GUIContent("速度:");
        EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        enemy.Speed = EditorGUI.FloatField(new Rect(startX + spaceX, startY, 40, height), enemy.Speed);

        cont = new GUIContent("傷害:");
        EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        enemy.Damage = EditorGUI.IntField(new Rect(startX + spaceX, startY, 40, height), enemy.Damage);

        cont = new GUIContent("可取得的資源:");
        EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        enemy.Value = EditorGUI.IntField(new Rect(startX + spaceX, startY, 40, height), enemy.Value);

        return new Vector2(startX, startY);
    }

}
