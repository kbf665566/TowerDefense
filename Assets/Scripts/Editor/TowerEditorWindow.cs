using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System;

public class TowerEditorWindow : UnitEditorWindow
{
    private static TowerEditorWindow window;

    public static void Init()
    {
        // Get existing open window or if none, make a new one:
        window = (TowerEditorWindow)GetWindow(typeof(TowerEditorWindow));

        EditorDataManager.Init();

        InitLabel();
    }

    private static string[] towerTypeLabel;
    private static string[] towerTypeTooltip;

    private static void InitLabel()
    {
        int enumLength = Enum.GetValues(typeof(TowerType)).Length;
        towerTypeLabel = new string[enumLength];
        towerTypeTooltip = new string[enumLength];
        for (int i = 0; i < enumLength; i++)
        {
            towerTypeLabel[i] = ((TowerType)i).ToString();
            if ((TowerType)i == TowerType.Normal) 
                towerTypeTooltip[i] = "一般的攻擊塔";
            if ((TowerType)i == TowerType.AOE) 
                towerTypeTooltip[i] = "範圍攻擊塔";
            if ((TowerType)i == TowerType.Support) 
                towerTypeTooltip[i] = "輔助塔";
            if ((TowerType)i == TowerType.Money) 
                towerTypeTooltip[i] = "增加資源的塔";
        }
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

    void SelectTower(int ID)
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

        List<TowerData> towerList = EditorDataManager.TowerList;

        EditorGUI.LabelField(new Rect(5, 7, 150, 17), "Add new tower:");
        TowerData newTower = null;

        if (GUI.Button(new Rect(100, 7, 140, 17), "Add"))
        {
            newTower = new TowerData();
            newTower.Name = "NewTower";
            newTower.towerType = TowerType.Normal;
            newTower.TowerSize = Vector2Short.One;
            newTower.TowerLevelData = new List<TowerLevelData>();
            newTower.TowerLevelData.Add(new TowerLevelData());
            int newSelectID = EditorDataManager.AddNewTower(newTower);
            if (newSelectID != -1) 
                SelectTower(newSelectID);
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
        Vector2 v2 = DrawTowerList(startX, startY, towerList);

        startX = v2.x + 25;

        if (towerList.Count == 0) 
            return;

        selectID = Mathf.Clamp(selectID, 0, towerList.Count - 1);

        startY += spaceY + 10;


        Rect visibleRect = new Rect(startX, startY, window.position.width - startX - 10, window.position.height - startY - 5);
        Rect contentRect = new Rect(startX, startY, contentWidth - startY, contentHeight);


        scrollPos2 = GUI.BeginScrollView(visibleRect, scrollPos2, contentRect);

        v2 = DrawTowerConfigurator(startX, startY, towerList);
        contentWidth = v2.x;
        contentHeight = v2.y;

        GUI.EndScrollView();


        if (GUI.changed) 
            EditorDataManager.SetDirtyTower();
    }

    
    Vector2 DrawTowerList(float startX, float startY, List<TowerData> towerList)
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
                    TowerData tower = towerList[selectID];
                    towerList[selectID] = towerList[selectID - 1];
                    towerList[selectID - 1] = tower;
                    selectID -= 1;

                    if (selectID * 35 < scrollPos1.y) 
                        scrollPos1.y = selectID * 35;
                }
            }
            if (GUI.Button(new Rect(startX + 227, startY - 20, 45, 18), "down"))
            {
                if (selectID < towerList.Count - 1)
                {
                    TowerData tower = towerList[selectID];
                    towerList[selectID] = towerList[selectID + 1];
                    towerList[selectID + 1] = tower;
                    selectID += 1;

                    if (listVisibleRect.height - 35 < selectID * 35) 
                        scrollPos1.y = (selectID + 1) * 35 - listVisibleRect.height + 5;
                }
            }
        }


        listVisibleRect = new Rect(startX, startY, width + 15, window.position.height - startY - 5);
        listContentRect = new Rect(startX, startY, width, towerList.Count * 35 + 5);

        GUI.color = new Color(.8f, .8f, .8f, 1f);
        GUI.Box(listVisibleRect, "");
        GUI.color = Color.white;

        scrollPos1 = GUI.BeginScrollView(listVisibleRect, scrollPos1, listContentRect);


        startY += 5; startX += 5;

        for (int i = 0; i < towerList.Count; i++)
        {

            DrawSprite(new Rect(startX, startY + (i * 35), 30, 30), towerList[i].TowerIcon);

            if (minimiseList)
            {
                if (selectID == i) 
                    GUI.color = new Color(0, 1f, 1f, 1f);
                if (GUI.Button(new Rect(startX + 35, startY + (i * 35), 30, 30), "")) 
                    SelectTower(i);
                GUI.color = Color.white;

                continue;
            }



            if (selectID == i) 
                GUI.color = new Color(0, 1f, 1f, 1f);
            if (GUI.Button(new Rect(startX + 35, startY + (i * 35), 150, 30), towerList[i].Name)) 
                SelectTower(i);
            GUI.color = Color.white;

            if (deleteID == i)
            {

                if (GUI.Button(new Rect(startX + 190, startY + (i * 35), 60, 15), "cancel")) 
                    deleteID = -1;

                GUI.color = Color.red;
                if (GUI.Button(new Rect(startX + 190, startY + (i * 35) + 15, 60, 15), "confirm"))
                {
                    if (selectID >= deleteID) 
                        SelectTower(Mathf.Max(0, selectID - 1));
                    EditorDataManager.RemoveTower(deleteID);
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

    Vector2 DrawTowerConfigurator(float startX, float startY, List<TowerData> towerList)
    {

        float maxWidth = 0;

        TowerData tower = towerList[selectID];

        float cachedY = startY;
        float cachedX = startX;
        startX += 65;  

        int type = (int)tower.towerType;
        cont = new GUIContent("Tower Type:");
        EditorGUI.LabelField(new Rect(startX, startY, width, height), cont);
        contL = new GUIContent[towerTypeLabel.Length];
        for (int i = 0; i < contL.Length; i++) 
            contL[i] = new GUIContent(towerTypeLabel[i], towerTypeTooltip[i]);
        type = EditorGUI.Popup(new Rect(startX + 80, startY, width - 40, 15), new GUIContent(""), type, contL);
        tower.towerType = (TowerType)type;
        startX = cachedX;

        v3 = DrawIconAndName(tower, startX, startY); 
        startY = v3.y;

        startX += spaceX * 2.5f;
        startY = cachedY;
        float fWidth = 35;
        cont = new GUIContent("塔的大小(x,y):", "");
        EditorGUI.LabelField(new Rect(startX, startY , width, height), cont);
        tower.TowerSize.x = (short)EditorGUI.IntField(new Rect(startX + spaceX, startY, fWidth, height), tower.TowerSize.x);
        tower.TowerSize.y = (short)EditorGUI.IntField(new Rect(startX + spaceX + fWidth + 3, startY, fWidth, height), tower.TowerSize.y);

        cont = new GUIContent("建造音效:", "");
        EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        tower.BuildSE = (AudioClip)EditorGUI.ObjectField(new Rect(startX + spaceX - 30, startY, 4 * fWidth - 40, height), tower.BuildSE, typeof(AudioClip), false);

        cont = new GUIContent("建造特效:", "");
        EditorGUI.LabelField(new Rect(startX + spaceX * 2, startY, width, height), cont);
        tower.BuildParticle = (GameObject)EditorGUI.ObjectField(new Rect(startX + spaceX * 3 - 30, startY, 4 * fWidth - 40, height), tower.BuildParticle, typeof(GameObject), false);

        cont = new GUIContent("拆除音效:", "");
        EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        tower.RemoveSE = (AudioClip)EditorGUI.ObjectField(new Rect(startX + spaceX - 30, startY, 4 * fWidth - 40, height), tower.RemoveSE, typeof(AudioClip), false);

        cont = new GUIContent("拆除特效:", "");
        EditorGUI.LabelField(new Rect(startX + spaceX * 2, startY, width, height), cont);
        tower.RemoveParticle = (GameObject)EditorGUI.ObjectField(new Rect(startX + spaceX * 3 - 30, startY, 4 * fWidth - 40, height), tower.RemoveParticle, typeof(GameObject), false);

        if (TowerCanAttack(tower))
        {
            cont = new GUIContent("攻擊音效:", "");
            EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
            tower.AttackSE = (AudioClip)EditorGUI.ObjectField(new Rect(startX + spaceX - 30, startY, 4 * fWidth - 40, height), tower.AttackSE, typeof(AudioClip), false);

            cont = new GUIContent("攻擊特效:", "");
            EditorGUI.LabelField(new Rect(startX + spaceX * 2, startY, width, height), cont);
            tower.AttackParticle = (GameObject)EditorGUI.ObjectField(new Rect(startX + spaceX * 3 - 30, startY, 4 * fWidth - 40, height), tower.AttackParticle, typeof(GameObject), false);
        }
        else
        {
            cont = new GUIContent("一般特效:", "");
            EditorGUI.LabelField(new Rect(startX + spaceX * 2, startY += spaceY, width, height), cont);
            tower.NormalParticle = (GameObject)EditorGUI.ObjectField(new Rect(startX + spaceX * 3 - 30, startY, 4 * fWidth - 40, height), tower.NormalParticle, typeof(GameObject), false);
        }

        if(tower.towerType == TowerType.Special)
        {
            cont = new GUIContent("特殊音效:", "");
            EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
            tower.SpecialMusic = (AudioClip)EditorGUI.ObjectField(new Rect(startX + spaceX - 30, startY, 4 * fWidth - 40, height), tower.SpecialMusic, typeof(AudioClip), false);
        }

        startX = cachedX;
        spaceX = 110;
        startY += 30;

        if (tower.towerType == TowerType.Normal)
        {
            cont = new GUIContent("是否使用發射物:");
            EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
            tower.IsUseBullet = EditorGUI.Toggle(new Rect(startX + spaceX, startY, 40, height), tower.IsUseBullet);
        }

        if(TowerCanAttack(tower))
        {
            cont = new GUIContent("是否可緩速敵人:");
            EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
            tower.CanSlowEnemy = EditorGUI.Toggle(new Rect(startX + spaceX, startY, 40, height), tower.CanSlowEnemy);

            cont = new GUIContent("是否可擊暈敵人:");
            EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
            tower.CanStunEnemy = EditorGUI.Toggle(new Rect(startX + spaceX, startY, 40, height), tower.CanStunEnemy);
        }

        if (startX + spaceX + width > maxWidth) 
            maxWidth = startX + spaceX + width;


        float maxY = startY;
        startY = 240;
        startX = cachedX;

        cont = new GUIContent("LevelData:", "");
        GUI.Label(new Rect(startX, startY += spaceY, 120, height), cont);
        if (GUI.Button(new Rect(startX + spaceX, startY, 50, 15), "-1"))
        {
            if (tower.TowerLevelData.Count > 1) 
                tower.TowerLevelData.RemoveAt(tower.TowerLevelData.Count - 1);
        }
        if (GUI.Button(new Rect(startX + 165, startY, 50, 15), "+1"))
        {
            TowerLevelData towerLevelData = new TowerLevelData();
            tower.TowerLevelData.Add(towerLevelData);
        }



        startY = Mathf.Max(maxY + 20, 240);
        startX = cachedX;

        float maxHeight = 0;
        float maxContentHeight = 0;


        minimiseStat = EditorGUI.Foldout(new Rect(startX, startY += spaceY + 15, width, height), minimiseStat, "Show Stats");
        if (!minimiseStat)
        {
            startY += spaceY;
            startX += 15;

            for (int i = 0; i < tower.TowerLevelData.Count; i++)
            {
                EditorGUI.LabelField(new Rect(startX, startY, width, height), "Level " + (i + 1) + " Data");
                v3 = DrawStat(tower.TowerLevelData[i], startX, startY + spaceY, statContentHeight, tower);

                if (maxContentHeight < v3.y)
                    maxContentHeight = v3.y;

                startX = v3.x + 10;
                if (startX > maxWidth)
                    maxWidth = startX;
                if (maxHeight < v3.y)
                    maxHeight = v3.y;
            }
            statContentHeight = maxContentHeight;
            startY = maxHeight;
        }

        startX = cachedX;
        startY += spaceY;


        GUIStyle style = new GUIStyle("TextArea");
        style.wordWrap = true;
        cont = new GUIContent("塔的資訊: ", "");
        EditorGUI.LabelField(new Rect(startX, startY += spaceY, 400, 20), cont);
        tower.TowerInformation = EditorGUI.TextArea(new Rect(startX, startY + spaceY - 3, 530, 50), tower.TowerInformation, style);

        startX = maxWidth - cachedX + 80;

        return new Vector2(startX, startY);
    }


    public static Vector3 DrawIconAndName(TowerData tower, float startX, float startY)
    {
        float cachedX = startX;
        float cachedY = startY;

        DrawSprite(new Rect(startX, startY, 60, 60), tower.TowerIcon);
        startX += 65;

        cont = new GUIContent("名稱:");
        EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        tower.Name = EditorGUI.TextField(new Rect(startX + spaceX - 65, startY, width - 5, height), tower.Name);

        cont = new GUIContent("Icon:");
        EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        tower.TowerIcon = (Sprite)EditorGUI.ObjectField(new Rect(startX + spaceX - 65, startY, width - 5, height), tower.TowerIcon, typeof(Sprite), false);

        startX -= 65;
        startY = cachedY;
        startX += 35 + spaceX + width;
        GUI.color = Color.white;


        float contentWidth = startX - cachedX + spaceX + 25;

        return new Vector3(startX, startY + spaceY, contentWidth);
    }


    private bool minimiseStat = false;

    private float statContentHeight = 0;
    public static Vector3 DrawStat(TowerLevelData levelData, float startX, float startY, float statContentHeight, TowerData tower)
    {
        float width = 150;
        float fWidth = 35;
        float spaceX = 130;
        float height = 18;
        float spaceY = height + 4;

        GUI.Box(new Rect(startX, startY - 3, 200, statContentHeight - startY + spaceY + 3), "");

        if (tower != null)
        {
            cont = new GUIContent("建造/升級 消耗:");
            EditorGUI.LabelField(new Rect(startX, startY, width, height), cont);
            levelData.BuildUpgradeCost = EditorGUI.IntField(new Rect(startX + spaceX, startY, fWidth, height), levelData.BuildUpgradeCost);

            cont = new GUIContent("賣價:");
            EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
            levelData.SoldPrice = EditorGUI.IntField(new Rect(startX + spaceX, startY, fWidth, height), levelData.SoldPrice);
        }

        if (TowerCanAttack(tower))
        {
            cont = new GUIContent("傷害:", "");
            EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
            levelData.Damage = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.Damage);

            cont = new GUIContent("射程:", "");
            EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
            levelData.ShootRange = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.ShootRange);

            cont = new GUIContent("攻速:", "");
            EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
            levelData.FireRate = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.FireRate);
        }

        if (tower.IsUseBullet && tower.towerType == TowerType.Normal)
        {
            cont = new GUIContent("發射物體:", "");
            EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
            levelData.TowerBullet = (Bullet)EditorGUI.ObjectField(new Rect(startX + spaceX - 48, startY, 3 * fWidth - 20, height), levelData.TowerBullet, typeof(Bullet), false);

            cont = new GUIContent("發射物速度:", "");
            EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
            levelData.BulletSpeed = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.BulletSpeed);


            cont = new GUIContent("發射物爆炸範圍:", "");
            EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
            levelData.BulletExplosionRadius = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.BulletExplosionRadius);
        }


        if (tower.CanSlowEnemy && TowerCanAttack(tower))
        {
            tower.CanStunEnemy = false;
            cont = new GUIContent("緩速程度:", "");
            EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
            levelData.SlowAmount = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.SlowAmount);

            cont = new GUIContent("緩速秒數:", "");
            EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
            levelData.SlowDuration = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.SlowDuration);
        }

        if (tower.CanStunEnemy && TowerCanAttack(tower))
        {
            tower.CanSlowEnemy = false;
            cont = new GUIContent("擊暈機率:", "");
            EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
            levelData.StunProbability = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.StunProbability);

            cont = new GUIContent("擊暈秒數:", "");
            EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
            levelData.StunDuration = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.StunDuration);
        }

        if (tower.towerType == TowerType.Support)
        {
            cont = new GUIContent("影響範圍:", "");
            EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
            levelData.ShootRange = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.ShootRange);

            cont = new GUIContent("Buff增加攻擊:", "");
            EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
            levelData.BuffAddDamage = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.BuffAddDamage);

            cont = new GUIContent("Buff增加範圍:", "");
            EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
            levelData.BuffAddRange = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.BuffAddRange);

            cont = new GUIContent("Buff增加攻速:", "");
            EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
            levelData.BuffAddFireRate = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.BuffAddFireRate);
        }

        if (tower.towerType == TowerType.Money)
        {
            cont = new GUIContent("攻速:", "");
            EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
            levelData.FireRate = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.FireRate);

            cont = new GUIContent("取得的資源量:", "");
            EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
            levelData.GetMoney = EditorGUI.IntField(new Rect(startX + spaceX, startY, fWidth, height), levelData.GetMoney);
        }


        cont = new GUIContent("塔 模型:");
        EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        levelData.towerPrefab = (TowerInLevel)EditorGUI.ObjectField(new Rect(startX + spaceX - 48, startY, 3 * fWidth - 20, height), levelData.towerPrefab, typeof(TowerInLevel), false);


        return new Vector3(startX + 220, startY, statContentHeight);
    }

    private static bool TowerCanAttack(TowerData tower)
    {
        return tower.towerType == TowerType.Normal || tower.towerType == TowerType.AOE;
    }

}
