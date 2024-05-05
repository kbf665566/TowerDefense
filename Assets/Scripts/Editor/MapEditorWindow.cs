using Codice.Client.BaseCommands;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapEditorWindow : UnitEditorWindow
{
    private static MapEditorWindow window;

    
    public static void Init()
    {
        // Get existing open window or if none, make a new one:
        window = (MapEditorWindow)GetWindow(typeof(MapEditorWindow));

        EditorDataManager.Init();
        selectID = -1;
    }

    private Vector2 scrollPos1;
    private Vector2 scrollPos2;

    private static int selectID = -1;
    private float contentHeight = 0;
    private float contentWidth = 0;

    private Rect listVisibleRect;
    private Rect listContentRect;

    private float drawBoxSize = 15f;
    private float mapGridOffset = 3f;

    private int deleteID = -1;
    private bool minimiseList = false;
    private List<int> enemypPathIdList = new List<int>();
    /// <summary> 判斷要不要顯示路徑點，避免多個路徑會拉太長不好編輯 </summary>
    private List<bool> showEnemyPath = new List<bool>();

    private bool showMap = true;
    void SelectMap(int ID)
    {
        selectID = ID;
        GUI.FocusControl("");

        if (selectID * 35 < scrollPos1.y) 
            scrollPos1.y = selectID * 35;
        if (selectID * 35 > scrollPos1.y + listVisibleRect.height - 40) 
            scrollPos1.y = selectID * 35 - listVisibleRect.height + 40;

        enemypPathIdList.Clear();
        showEnemyPath.Clear();
        var map = EditorDataManager.MapList[selectID];
        for (int i = 0; i < map.EnemyPathList.Count; i++)
        {
            if (enemypPathIdList.Contains(map.EnemyPathList[i].Id))
                map.EnemyPathList[i].Id = EditorDataManager.GenerateNewID(enemypPathIdList);
            enemypPathIdList.Add(map.EnemyPathList[i].Id);
            showEnemyPath.Add(true);
        }
    }

    void OnGUI()
    {
        if (window == null) Init();

        List<MapData> mapList = EditorDataManager.MapList;

        if (GUI.Button(new Rect(window.position.width - 120, 5, 100, 25), "Save"))
            EditorDataManager.SetDirtyMap();

        EditorGUI.LabelField(new Rect(5, 7, 150, 17), "Add new Map:");
        MapData newMap = null;

        if (GUI.Button(new Rect(100, 7, 140, 17), "Add"))
        {
            newMap = new MapData();
            newMap.MapName = "NewMap";
            newMap.EnemyPathList = new List<EnemyPathData>();
            int newSelectID = EditorDataManager.AddNewMap(newMap);
            if (newSelectID != -1)
                SelectMap(newSelectID);
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
        Vector2 v2 = DrawMapList(startX, startY, mapList);

        startX = v2.x + 25;

        if (mapList.Count == 0)
            return;

        if (selectID == -1)
            SelectMap(0);

        selectID = Mathf.Clamp(selectID, 0, mapList.Count - 1);

        cont = new GUIContent("場景名稱:","程式讀取用");
        EditorGUI.LabelField(new Rect(startX, startY, width, height), cont);
        EditorGUI.TextField(new Rect(startX + spaceX - 35, startY, width - 5, height), mapList[selectID].SceneName);


        startY += spaceY + 10;


        Rect visibleRect = new Rect(startX, startY, window.position.width - startX - 10, window.position.height - startY - 5);
        Rect contentRect = new Rect(startX, startY, contentWidth - startY, contentHeight);

        scrollPos2 = GUI.BeginScrollView(visibleRect, scrollPos2, contentRect);
        v2 = DrawMapConfigurator(startX, startY, mapList);
        if (mapList[selectID].MapSize.x < 30)
            contentWidth = v2.x;
        else
            contentWidth = mapList[selectID].MapSize.x * (drawBoxSize + mapGridOffset) + 400;
        if (mapList[selectID].MapSize.y < 30)
            contentHeight = v2.y;
        else
            contentHeight = mapList[selectID].MapSize.y * (drawBoxSize + mapGridOffset) + 120;
        

        GUI.EndScrollView();


        if (GUI.changed) EditorDataManager.SetDirtyMap();
    }

    Vector2 DrawMapList(float startX, float startY, List<MapData> mapList)
    {

        float width = 260;
        if (minimiseList) width = 60;

        if (!minimiseList)
        {
            if (GUI.Button(new Rect(startX + 180, startY - 20, 45, 18), "up"))
            {
                if (selectID > 0)
                {
                    MapData map = mapList[selectID];
                    mapList[selectID] = mapList[selectID - 1];
                    mapList[selectID - 1] = map;
                    selectID -= 1;

                    if (selectID * 35 < scrollPos1.y)
                        scrollPos1.y = selectID * 35;
                }
            }
            if (GUI.Button(new Rect(startX + 227, startY - 20, 45, 18), "down"))
            {
                if (selectID < mapList.Count - 1)
                {
                    MapData map = mapList[selectID];
                    mapList[selectID] = mapList[selectID + 1];
                    mapList[selectID + 1] = map;
                    selectID += 1;

                    if (listVisibleRect.height - 35 < selectID * 35)
                        scrollPos1.y = (selectID + 1) * 35 - listVisibleRect.height + 5;
                }
            }
        }


        listVisibleRect = new Rect(startX, startY, width + 15, window.position.height - startY - 5);
        listContentRect = new Rect(startX, startY, width, mapList.Count * 35 + 5);

        GUI.color = new Color(.8f, .8f, .8f, 1f);
        GUI.Box(listVisibleRect, "");
        GUI.color = Color.white;

        scrollPos1 = GUI.BeginScrollView(listVisibleRect, scrollPos1, listContentRect);


        startY += 5; startX += 5;

        for (int i = 0; i < mapList.Count; i++)
        {
            DrawSprite(new Rect(startX, startY + (i * 35), 30, 30), mapList[i].MapIcon);

            if (minimiseList)
            {
                if (selectID == i)
                    GUI.color = new Color(0, 1f, 1f, 1f);
                if (GUI.Button(new Rect(startX + 35, startY + (i * 35), 30, 30), ""))
                    SelectMap(i);
                GUI.color = Color.white;

                continue;
            }



            if (selectID == i)
                GUI.color = new Color(0, 1f, 1f, 1f);
            if (GUI.Button(new Rect(startX + 35, startY + (i * 35), 150, 30), mapList[i].MapName))
                SelectMap(i);
            GUI.color = Color.white;

            if (deleteID == i)
            {

                if (GUI.Button(new Rect(startX + 190, startY + (i * 35), 60, 15), "cancel")) deleteID = -1;

                GUI.color = Color.red;
                if (GUI.Button(new Rect(startX + 190, startY + (i * 35) + 15, 60, 15), "confirm"))
                {
                    if (selectID >= deleteID)
                        SelectMap(Mathf.Max(0, selectID - 1));
                    if (selectID >= deleteID)
                        SelectMap(Mathf.Max(0, selectID - 1));
                    EditorDataManager.RemoveMap(deleteID);
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

    private Vector3 v3 = Vector3.zero;
    private int deleteEnemyPathId = -1;
    private Vector2 DrawMapConfigurator(float startX, float startY, List<MapData> mapList)
    {
        MapData map = mapList[selectID];

        float cachedY = startY;
        float cachedX = startX;
        float fWidth = 35;

        v3 = DrawIconAndName(map.MapName, map.MapIcon, startX, startY); startY = v3.y;

        startX = cachedX;
        spaceX = 110;
        startY += 30;

        cont = new GUIContent("敵人路徑:");
        EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);

        if (GUI.Button(new Rect(startX + 65, startY, 75, 17), "Add Path"))
        {
            EnemyPathData newPath = new EnemyPathData();
            newPath.Path = new List<EnemyPath>();
            newPath.Path.Add(new EnemyPath() { GridPos = Vector2Short.Zero });
            newPath.Id = EditorDataManager.GenerateNewID(enemypPathIdList);
            enemypPathIdList.Add(newPath.Id);
            map.EnemyPathList.Add(newPath);
            showEnemyPath.Add(true);
        }

        for (int i = 0; i < map.EnemyPathList.Count; i++)
        {
            if (showEnemyPath[i])
            {
                if (GUI.Button(new Rect(startX + 3, startY += spaceY, 35, 18), "收縮"))
                    showEnemyPath[i] = false;
            }
            else
            {
                if (GUI.Button(new Rect(startX + 3, startY += spaceY, 35, 18), "展開"))
                    showEnemyPath[i] = true;
            }

            cont = new GUIContent("路徑Id:" + map.EnemyPathList[i].Id);
            EditorGUI.LabelField(new Rect(startX + 40f, startY, width, height), cont);

            if (GUI.Button(new Rect(startX + 100, startY, 100, 18), "Add Pos"))
            {
                map.EnemyPathList[i].Path.Add(new EnemyPath() { GridPos = Vector2Short.Zero });
            }

            if (deleteEnemyPathId == i)
            {

                if (GUI.Button(new Rect(startX + 210 + 60, startY, 60, 18), "cancel"))
                {
                    deleteEnemyPathId = -1;
                }

                GUI.color = Color.red;
                if (GUI.Button(new Rect(startX + 210 - 5, startY, 60, 18), "confirm"))
                {
                    map.EnemyPathList.RemoveAt(i);
                    enemypPathIdList.RemoveAt(i);
                    showEnemyPath.RemoveAt(i);
                    deleteEnemyPathId = -1;
                    continue;
                }
                GUI.color = Color.white;
            }
            else
            {
                if (GUI.Button(new Rect(startX + 210, startY, 100, 18), "Remove Path"))
                {
                    deleteEnemyPathId = i;
                }
            }

            if (showEnemyPath[i])
                startY = DrawEnemyPathSetting(startX + 5, startY + spaceY + 5, fWidth, map.EnemyPathList[i]).y;
        }
        startY += 5;
        GUI.color = Color.red;
        GUI.Box(new Rect(startX, startY + spaceY - 3, 260, 2), "");
        GUI.color = Color.white;
        cont = new GUIContent("障礙設定:", "禁止建塔區域");
        EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        if (GUI.Button(new Rect(startX + 60, startY, 75, 17), "Add Block"))
        {
            BlockData newBlock = new BlockData();
            newBlock.GridPos = Vector2Short.Zero;
            newBlock.Size = Vector2Short.One;
            map.BlockGridList.Add(newBlock);
        }
        DrawBlockSetting(startX + 5, startY + spaceY + 5, fWidth - 10, map.BlockGridList);

        startY = cachedY;
        startX += spaceX * 3;

        cont = new GUIContent("地圖大小(x,y):", "");
        EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        map.MapSize.x = (short)EditorGUI.IntField(new Rect(startX + spaceX, startY, fWidth, height), map.MapSize.x);
        map.MapSize.y = (short)EditorGUI.IntField(new Rect(startX + spaceX + fWidth + 3, startY, fWidth, height), map.MapSize.y);

        cont = new GUIContent("WavesId:", "要使用的Waves的Id");
        EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        map.WavesId = EditorGUI.IntField(new Rect(startX + spaceX, startY, 40, height), map.WavesId);

        cont = new GUIContent("顯示地圖:", "是否顯示地圖");
        EditorGUI.LabelField(new Rect(startX, startY + spaceY * 2.5f, width, height), cont);
        showMap = EditorGUI.Toggle(new Rect(startX + spaceX, startY + spaceY * 2.5f, 40, height), showMap);


        bool pathError = false;
        for (int i = 0; i < map.EnemyPathList.Count; i++)
        {
            if (map.EnemyPathList[i].Path.Count <= 1)
            {
                pathError = true;
                break;
            }
        }

        if (showMap)
        {
            if (map.MapSize.x != 0 && map.MapSize.y != 0 && pathError == false)
                DrawTempMap(630, 200, map.MapSize, map.EnemyPathList, map.BlockGridList);
            else if (map.MapSize.x == 0 || map.MapSize.y == 0)
            {
                GUI.color = Color.red;
                cont = new GUIContent("地圖大小不能有0", "");
                EditorGUI.LabelField(new Rect(570, 200, 150, height), cont);
            }
            else if (pathError)
            {
                GUI.color = Color.red;
                cont = new GUIContent("敵人路徑至少要有兩個點以上", "");
                EditorGUI.LabelField(new Rect(570, 200, 400, height), cont);
            }
        }

        GUI.color = Color.white;
        return new Vector2(startX, startY);
    }

    private Vector2 DrawEnemyPathSetting(float startX, float startY,float width,EnemyPathData enemyPath)
    {
        for (int i = 0; i < enemyPath.Path.Count; i++)
        {
            cont = new GUIContent("路徑點 " + (i + 1) + ":", "");
            EditorGUI.LabelField(new Rect(startX, startY, width + 15, height), cont);
            var PosX = (short)EditorGUI.IntField(new Rect(startX + 60, startY, width, height), enemyPath.Path[i].GridPos.x);
            var PosY = (short)EditorGUI.IntField(new Rect(startX + 60 + width + 3, startY, width, height), enemyPath.Path[i].GridPos.y);
            enemyPath.Path[i].GridPos.SetPos(PosX, PosY);

            if (GUI.Button(new Rect(startX + 60 + width * 2 + 10, startY, 80, 18), "Remove Pos"))
            {
                if (enemyPath.Path.Count > 1)
                    enemyPath.Path.RemoveAt(i);
            }

            startY += spaceY;
        }
        return new Vector2(startX, startY);
    }

    private Vector2 DrawBlockSetting(float startX, float startY, float width, List<BlockData> blockGridList)
    {
        var tempX = startX;
        for (int i = 0; i < blockGridList.Count; i++)
        {
            GUI.Box(new Rect(startX, startY - 3, 240, spaceY * 3 + 10), "");

            cont = new GUIContent("障礙 " + (i + 1) + ":", "");
            EditorGUI.LabelField(new Rect(startX, startY, width + 15, height), cont);

            if (GUI.Button(new Rect(startX + 55, startY, 100, 18), "Remove Block"))
            {
                blockGridList.RemoveAt(i);
                continue;
            }

            cont = new GUIContent("位置(x,y):", "");
            EditorGUI.LabelField(new Rect(startX, startY += (spaceY + 5), width + 30, height), cont);
            var PosX = (short)EditorGUI.IntField(new Rect(startX += 60, startY, width, height), blockGridList[i].GridPos.x);
            var PosY = (short)EditorGUI.IntField(new Rect(startX + width + 3, startY, width, height), blockGridList[i].GridPos.y);
            blockGridList[i].GridPos.SetPos(PosX, PosY);

            cont = new GUIContent("大小(x,y):", "");
            EditorGUI.LabelField(new Rect(startX += 60, startY, width + 30, height), cont);
            var SizeX = (short)EditorGUI.IntField(new Rect(startX += 60, startY, width, height), blockGridList[i].Size.x);
            var SizeY = (short)EditorGUI.IntField(new Rect(startX + width + 3, startY, width, height), blockGridList[i].Size.y);
            blockGridList[i].Size.SetPos(SizeX, SizeY);

            startX = tempX;
            cont = new GUIContent("Block Prefab:");
            EditorGUI.LabelField(new Rect(startX, startY += spaceY, width + 60, height), cont);
            EditorGUI.ObjectField(new Rect(startX + 112, startY, 120, height), blockGridList[i].BlockPrefab, typeof(GameObject), false);

            startY += (spaceY + 10);
        }
        return new Vector2(startX, startY);
    }

    private void DrawTempMap(float startX,float startY,Vector2Short mapSize,List<EnemyPathData> enemyPathList,List<BlockData> blockGridList)
    {
        float tempY = (drawBoxSize + mapGridOffset) * mapSize.y;

        //畫出基本地圖
        GUI.color = Color.black;
        for (int i = 0;i<mapSize.x;i++)
        {
            for (int j = mapSize.y; j > 0; j--)
            {
                GUI.Box(new Rect(startX + (drawBoxSize + mapGridOffset) * i , startY + tempY - (drawBoxSize + mapGridOffset) * j, drawBoxSize, drawBoxSize), "", EditorDataManager.BoxStyle);
            }
        }

        //畫出敵人路徑
        GUI.color = Color.red;
        for(int i = 0;i<enemyPathList.Count;i++)
        {
            for(int j = 0; j < enemyPathList[i].Path.Count - 1;j++)
            {
                //畫出垂直路徑
                if (enemyPathList[i].Path[j].GridPos.x == enemyPathList[i].Path[j + 1].GridPos.x)
                {
                    if (Vector2Short.Distance(Vector2Short.Zero, enemyPathList[i].Path[j].GridPos) > Vector2Short.Distance(Vector2Short.Zero, enemyPathList[i].Path[j + 1].GridPos))
                        GUI.Box(new Rect(startX + (drawBoxSize + mapGridOffset) * enemyPathList[i].Path[j].GridPos.x + drawBoxSize / 2, startY + tempY - (drawBoxSize + mapGridOffset) * enemyPathList[i].Path[j].GridPos.y - drawBoxSize + mapGridOffset, 2, (drawBoxSize + mapGridOffset) * (enemyPathList[i].Path[j].GridPos.y - enemyPathList[i].Path[j + 1].GridPos.y)), "",EditorDataManager.BoxStyle);
                    else
                        GUI.Box(new Rect(startX + (drawBoxSize + mapGridOffset) * enemyPathList[i].Path[j].GridPos.x + drawBoxSize / 2, startY + tempY - (drawBoxSize + mapGridOffset) * enemyPathList[i].Path[j].GridPos.y - drawBoxSize + mapGridOffset, 2, -(drawBoxSize + mapGridOffset) * (enemyPathList[i].Path[j + 1].GridPos.y - enemyPathList[i].Path[j].GridPos.y)), "", EditorDataManager.BoxStyle);
                    
                }
                //畫出水平路徑
                else if (enemyPathList[i].Path[j].GridPos.y == enemyPathList[i].Path[j + 1].GridPos.y)
                {
                    if (Vector2Short.Distance(Vector2Short.Zero, enemyPathList[i].Path[j].GridPos) > Vector2Short.Distance(Vector2Short.Zero, enemyPathList[i].Path[j + 1].GridPos))
                        GUI.Box(new Rect(startX + (drawBoxSize + mapGridOffset) * enemyPathList[i].Path[j].GridPos.x + drawBoxSize / 2, startY + tempY - (drawBoxSize + mapGridOffset) * enemyPathList[i].Path[j].GridPos.y - drawBoxSize + mapGridOffset, -(enemyPathList[i].Path[j].GridPos.x - enemyPathList[i].Path[j + 1].GridPos.x) * (drawBoxSize + mapGridOffset), 2), "", EditorDataManager.BoxStyle);
                    else
                        GUI.Box(new Rect(startX + (drawBoxSize + mapGridOffset) * enemyPathList[i].Path[j].GridPos.x + drawBoxSize / 2, startY + tempY - (drawBoxSize + mapGridOffset) * enemyPathList[i].Path[j].GridPos.y - drawBoxSize + mapGridOffset, enemyPathList[i].Path[j + 1].GridPos.x * (drawBoxSize + mapGridOffset) , 2), "", EditorDataManager.BoxStyle);
                }
            }
        }

        //畫出障礙物
        GUI.color = Color.white;
        for(int i =0;i< blockGridList.Count;i++)
        {
            GUI.Box(new Rect(startX + (drawBoxSize + mapGridOffset) * blockGridList[i].GridPos.x, startY + tempY - (drawBoxSize + mapGridOffset) * blockGridList[i].GridPos.y, blockGridList[i].Size.x * (drawBoxSize + mapGridOffset), -blockGridList[i].Size.y * (drawBoxSize + mapGridOffset)), "", EditorDataManager.BoxStyle);
        }
    }
}
