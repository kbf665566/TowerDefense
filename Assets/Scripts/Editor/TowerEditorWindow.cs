using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System;
using System.Security.Cryptography;

public class TowerEditorWindow : UnitEditorWindow
{
    private static TowerEditorWindow window;


    public static void Init()
    {
        // Get existing open window or if none, make a new one:
        window = (TowerEditorWindow)EditorWindow.GetWindow(typeof(TowerEditorWindow));

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
            if ((TowerType)i == TowerType.Normal) towerTypeTooltip[i] = "一般的攻擊塔";
            if ((TowerType)i == TowerType.AOE) towerTypeTooltip[i] = "範圍攻擊塔";
            if ((TowerType)i == TowerType.Support) towerTypeTooltip[i] = "輔助塔";
            if ((TowerType)i == TowerType.Money) towerTypeTooltip[i] = "增加資源的塔";
        }
    }

    void SelectTower(int ID)
    {
        selectID = ID;
        GUI.FocusControl("");

        if (selectID * 35 < scrollPos1.y) scrollPos1.y = selectID * 35;
        if (selectID * 35 > scrollPos1.y + listVisibleRect.height - 40) scrollPos1.y = selectID * 35 - listVisibleRect.height + 40;
    }

    private Vector2 scrollPos1;
    private Vector2 scrollPos2;

    private static int selectID = 0;
    private float contentHeight = 0;
    private float contentWidth = 0;


    void OnGUI()
    {
        if (window == null) Init();

        List<TowerData> towerList = EditorDataManager.TowerList;

        if (GUI.Button(new Rect(window.position.width - 120, 5, 100, 25), "Save")) EditorDataManager.SetDirtyTower();

        EditorGUI.LabelField(new Rect(5, 7, 150, 17), "Add new tower:");
        TowerData newTower = null;

        //改成Button
        //newTower = (TowerData)EditorGUI.ObjectField(new Rect(100, 7, 140, 17), newTower, typeof(TowerData), false);
        if (newTower != null)
        {
            int newSelectID = EditorDataManager.AddNewTower(newTower);
            if (newSelectID != -1) SelectTower(newSelectID);
        }


        float startX = 5;
        float startY = 50;

        if (minimiseList)
        {
            if (GUI.Button(new Rect(startX, startY - 20, 30, 18), ">>")) minimiseList = false;
        }
        else
        {
            if (GUI.Button(new Rect(startX, startY - 20, 30, 18), "<<")) minimiseList = true;
        }
        Vector2 v2 = DrawTowerList(startX, startY, towerList);

        startX = v2.x + 25;

        if (towerList.Count == 0) return;

        selectID = Mathf.Clamp(selectID, 0, towerList.Count - 1);

        cont = new GUIContent("Tower Prefab:");
        EditorGUI.LabelField(new Rect(startX, startY, width, height), cont);
        EditorGUI.ObjectField(new Rect(startX + 90, startY, 185, height), towerList[selectID].TowerLevelData[0].towerPrefab, typeof(GameObject), false);


        startY += spaceY + 10;


        Rect visibleRect = new Rect(startX, startY, window.position.width - startX - 10, window.position.height - startY - 5);
        Rect contentRect = new Rect(startX, startY, contentWidth - startY, contentHeight);


        scrollPos2 = GUI.BeginScrollView(visibleRect, scrollPos2, contentRect);

        v2 = DrawTowerConfigurator(startX, startY, towerList);
        contentWidth = v2.x;
        contentHeight = v2.y;

        GUI.EndScrollView();


        if (GUI.changed) EditorDataManager.SetDirtyTower();
    }

    private Rect listVisibleRect;
    private Rect listContentRect;

    private int deleteID = -1;
    private bool minimiseList = false;
    Vector2 DrawTowerList(float startX, float startY, List<TowerData> towerList)
    {

        float width = 260;
        if (minimiseList) width = 60;

        if (!minimiseList)
        {
            if (GUI.Button(new Rect(startX + 180, startY - 20, 40, 18), "up"))
            {
                if (selectID > 0)
                {
                    TowerData tower = towerList[selectID];
                    towerList[selectID] = towerList[selectID - 1];
                    towerList[selectID - 1] = tower;
                    selectID -= 1;

                    if (selectID * 35 < scrollPos1.y) scrollPos1.y = selectID * 35;
                }
            }
            if (GUI.Button(new Rect(startX + 222, startY - 20, 40, 18), "down"))
            {
                if (selectID < towerList.Count - 1)
                {
                    TowerData tower = towerList[selectID];
                    towerList[selectID] = towerList[selectID + 1];
                    towerList[selectID + 1] = tower;
                    selectID += 1;

                    if (listVisibleRect.height - 35 < selectID * 35) scrollPos1.y = (selectID + 1) * 35 - listVisibleRect.height + 5;
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
                if (selectID == i) GUI.color = new Color(0, 1f, 1f, 1f);
                if (GUI.Button(new Rect(startX + 35, startY + (i * 35), 30, 30), "")) SelectTower(i);
                GUI.color = Color.white;

                continue;
            }



            if (selectID == i) GUI.color = new Color(0, 1f, 1f, 1f);
            if (GUI.Button(new Rect(startX + 35, startY + (i * 35), 150, 30), towerList[i].Name)) SelectTower(i);
            GUI.color = Color.white;

            if (deleteID == i)
            {

                if (GUI.Button(new Rect(startX + 190, startY + (i * 35), 60, 15), "cancel")) deleteID = -1;

                GUI.color = Color.red;
                if (GUI.Button(new Rect(startX + 190, startY + (i * 35) + 15, 60, 15), "confirm"))
                {
                    if (selectID >= deleteID) SelectTower(Mathf.Max(0, selectID - 1));
                    EditorDataManager.RemoveTower(deleteID);
                    deleteID = -1;
                }
                GUI.color = Color.white;
            }
            else
            {
                if (GUI.Button(new Rect(startX + 190, startY + (i * 35), 60, 15), "remove")) deleteID = i;
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
        for (int i = 0; i < contL.Length; i++) contL[i] = new GUIContent(towerTypeLabel[i], towerTypeTooltip[i]);
        type = EditorGUI.Popup(new Rect(startX + 80, startY, width - 40, 15), new GUIContent(""), type, contL);
        tower.towerType = (TowerType)type;
        startX = cachedX;

        v3 = DrawIconAndName(tower.Name,tower.TowerIcon, startX, startY); startY = v3.y;


        startX = cachedX;
        spaceX = 110;


        if (startX + spaceX + width > maxWidth) maxWidth = startX + spaceX + width;


        startY = cachedY;
        startX += spaceX + width + 35;


        float maxY = startY;
        startY = 270;

        startX = cachedX; cachedY = startY;

        cont = new GUIContent("LevelData:", "");
        GUI.Label(new Rect(startX, startY += spaceY, 120, height), cont);
        if (GUI.Button(new Rect(startX + spaceX, startY, 50, 15), "-1"))
        {
            if (tower.TowerLevelData.Count > 1) tower.TowerLevelData.RemoveAt(tower.TowerLevelData.Count - 1);
        }
        if (GUI.Button(new Rect(startX + 165, startY, 50, 15), "+1"))
        {
            tower.TowerLevelData.Add(tower.TowerLevelData[tower.TowerLevelData.Count - 1]);
        }



        startY = Mathf.Max(maxY + 20, 310);
        startX = cachedX;

        float maxHeight = 0;
        float maxContentHeight = 0;

        minimiseStat = EditorGUI.Foldout(new Rect(startX, startY, width, height), minimiseStat, "Show Stats");
        if (!minimiseStat)
        {
            startY += spaceY;
            startX += 15;

            if (tower.TowerLevelData.Count == 0) tower.TowerLevelData.Add(new TowerLevelData());
            for (int i = 0; i < tower.TowerLevelData.Count; i++)
            {
                EditorGUI.LabelField(new Rect(startX, startY, width, height), "Level " + (i + 1) + " Data");
                v3 = DrawStat(tower.TowerLevelData[i], startX, startY + spaceY, statContentHeight, tower);
                if (maxContentHeight < v3.z) maxContentHeight = v3.z;

                startX = v3.x + 10;
                if (startX > maxWidth) maxWidth = startX;
                if (maxHeight < v3.y) maxHeight = v3.y;
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


    private bool minimiseStat = false;

    private float statContentHeight = 0;

    public static Vector3 DrawStat(TowerLevelData levelData, float startX, float startY, float statContentHeight, TowerData tower)
    {
        float width = 150;
        float fWidth = 35;
        float spaceX = 130;
        float height = 18;
        float spaceY = height + 2;

        GUI.Box(new Rect(startX, startY, 220, statContentHeight - startY), "");
        if (tower != null)
        {
            cont = new GUIContent("建造/升級 消耗:");
            EditorGUI.LabelField(new Rect(startX, startY, width, height), cont);
            levelData.BuildUpgradeCost = EditorGUI.IntField(new Rect(startX + spaceX, startY, fWidth, height), levelData.BuildUpgradeCost);

            startY += spaceY + 5;
        }

        //if (TowerUseShootObject(tower))
        //{
        //    cont = new GUIContent("ShootObject:", "The shootObject used by the unit.\nUnit that intended to shoot at the target will not function correctly if this is left unassigned.");
        //    EditorGUI.LabelField(new Rect(startX, startY, width, height), cont);
        //    levelData.shootObject = (ShootObject)EditorGUI.ObjectField(new Rect(startX + spaceX - 50, startY, 4 * fWidth - 20, height), levelData.shootObject, typeof(ShootObject), false);
        //    startY += 5;
        //}

        //if (TowerUseShootObjectT(tower)
        //{
        //    cont = new GUIContent("ShootObject:", "The shootObject used by the unit.\nUnit that intended to shoot at the target will not function correctly if this is left unassigned.");
        //    EditorGUI.LabelField(new Rect(startX, startY, width, height), cont);
        //    levelData.shootObjectT = (Transform)EditorGUI.ObjectField(new Rect(startX + spaceX - 50, startY, 4 * fWidth - 20, height), levelData.shootObjectT, typeof(Transform), false);
        //    startY += 5;
        //}

        //if (TowerDealDamage(tower))
        //{
        //    cont = new GUIContent("Damage(Min/Max):", "");
        //    EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        //    levelData.damageMin = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.damageMin);
        //    levelData.damageMax = EditorGUI.FloatField(new Rect(startX + spaceX + fWidth, startY, fWidth, height), levelData.damageMax);

        //    cont = new GUIContent("Cooldown:", "Duration between each attack");
        //    EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        //    levelData.cooldown = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.cooldown);



        //    cont = new GUIContent("Range:", "Effect range of the unit");
        //    EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        //    levelData.range = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.range);

        //    cont = new GUIContent("AOE Radius:", "Area-of-Effective radius. When the shootObject hits it's target, any other hostile unit within the area from the impact position will suffer the same target as the target.\nSet value to >0 to enable. ");
        //    EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        //    levelData.aoeRadius = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.aoeRadius);



        //    cont = new GUIContent("Stun", "");
        //    EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont); startY -= spaceY;

        //    cont = new GUIContent("        - Chance:", "Chance to stun the target in each successful attack. Takes value from 0-1 with 0 being 0% and 1 being 100%");
        //    EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        //    levelData.stun.chance = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.stun.chance);

        //    cont = new GUIContent("        - Duration:", "The stun duration in second");
        //    EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        //    levelData.stun.duration = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.stun.duration);



        //    cont = new GUIContent("Critical", "");
        //    EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont); startY -= spaceY;

        //    cont = new GUIContent("            - Chance:", "Chance to score critical hit in attack. Takes value from 0-1 with 0 being 0% and 1 being 100%");
        //    EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        //    levelData.crit.chance = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.crit.chance);

        //    cont = new GUIContent("            - Multiplier:", "Damage multiplier for successful critical hit. Takes value from 0 and above with with 0.5 being 50% of normal damage as bonus");
        //    EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        //    levelData.crit.dmgMultiplier = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.crit.dmgMultiplier);



        //    cont = new GUIContent("Slow", "");
        //    EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont); startY -= spaceY;

        //    cont = new GUIContent("         - Duration:", "The effect duration in second");
        //    EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        //    levelData.slow.duration = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.slow.duration);

        //    cont = new GUIContent("         - Multiplier:", "Move speed multiplier. Takes value from 0-1 with with 0.7 being decrese default speed by 30%");
        //    EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        //    levelData.slow.slowMultiplier = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.slow.slowMultiplier);



        //    cont = new GUIContent("Dot", "Damage over time");
        //    EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont); startY -= spaceY;

        //    cont = new GUIContent("        - Duration:", "The effect duration in second");
        //    EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        //    levelData.dot.duration = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.dot.duration);

        //    cont = new GUIContent("        - Interval:", "Duration between each tick. Damage is applied at each tick.");
        //    EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        //    levelData.dot.interval = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.dot.interval);

        //    cont = new GUIContent("        - Damage:", "Damage applied at each tick");
        //    EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        //    levelData.dot.value = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.dot.value);


        //}



        //if (tower.type == _TowerType.Support)
        //{
        //    cont = new GUIContent("Range:", "Effect range of the unit");
        //    EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        //    levelData.range = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.range);
        //    startY += 5;

        //    cont = new GUIContent("Buff:", "Note: Buffs from multple tower doesnt stack, however when there's difference in the buff strength, the stronger buff applies. A tower can gain maximum dmage buff from one source and maximum range buff from another");
        //    EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont); startY -= spaceY;

        //    cont = new GUIContent("        - Damage:", "Damage buff multiplier. Takes value from 0 and above with 0.5 being 50% increase in damage");
        //    EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        //    levelData.buff.damageBuff = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.buff.damageBuff);

        //    cont = new GUIContent("        - Cooldown:", "Dooldown buff multiplier. Takes value from 0-1 with 0.2 being reduce cooldown by 20%");
        //    EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        //    levelData.buff.cooldownBuff = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.buff.cooldownBuff);

        //    cont = new GUIContent("        - Range:", "Range buff multiplier. Takes value from 0 and above with 0.5 being 50% increase in range");
        //    EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        //    levelData.buff.rangeBuff = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.buff.rangeBuff);

        //    cont = new GUIContent("        - Critical:", "Critical hit chance buff modifier. Takes value from 0 and above with 0.25 being 25% increase in critical hit chance");
        //    EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        //    levelData.buff.criticalBuff = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.buff.criticalBuff);

        //    cont = new GUIContent("        - HP Regen:", "HP Regeneration Buff. Takes value from 0 and above with 2 being gain 2HP second ");
        //    EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        //    levelData.buff.regenHP = EditorGUI.FloatField(new Rect(startX + spaceX, startY, fWidth, height), levelData.buff.regenHP);
        //}

        //statContentHeight = startY + spaceY + 5;

        return new Vector3(startX + 220, startY, statContentHeight);
    }
}
