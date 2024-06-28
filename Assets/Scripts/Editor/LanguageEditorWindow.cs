using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LanguageEditorWindow : UnitEditorWindow
{
    private static LanguageEditorWindow window;

    public static void Init()
    {
        // Get existing open window or if none, make a new one:
        window = (LanguageEditorWindow)GetWindow(typeof(LanguageEditorWindow));

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

    void SelectLanguageKey(int ID)
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

        List<LanguageData> languageDataList = EditorDataManager.LanguageDataList;

        EditorGUI.LabelField(new Rect(5, 7, 150, 17), "Add new LanguageKey:");
        LanguageData newLanguageKey = null;

        if (GUI.Button(new Rect(100, 7, 140, 17), "Add"))
        {
            newLanguageKey = new LanguageData();
            newLanguageKey.Key = "NewKey";
            int newSelectID = EditorDataManager.AddNewLanguageKey(newLanguageKey);
            if (newSelectID != -1)
                SelectLanguageKey(newSelectID);
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
        Vector2 v2 = DrawLanguageKeyList(startX, startY, languageDataList);

        startX = v2.x + 25;

        if (languageDataList.Count == 0)
            return;

        selectID = Mathf.Clamp(selectID, 0, languageDataList.Count - 1);

        Rect visibleRect = new Rect(startX, startY, window.position.width - startX - 10, window.position.height - startY - 5);
        Rect contentRect = new Rect(startX, startY, contentWidth - startY, contentHeight);


        scrollPos2 = GUI.BeginScrollView(visibleRect, scrollPos2, contentRect);

        v2 = DrawLanguageKeyConfigurator(startX, startY, languageDataList);
        contentWidth = v2.x;
        contentHeight = v2.y;

        GUI.EndScrollView();


        if (GUI.changed) EditorDataManager.SetDirtyLanguage();
    }

    Vector2 DrawLanguageKeyList(float startX, float startY, List<LanguageData> languageDataList)
    {

        float width = 260;
        if (minimiseList) width = 60;

        if (!minimiseList)
        {
            if (GUI.Button(new Rect(startX + 180, startY - 20, 45, 18), "up"))
            {
                if (selectID > 0)
                {
                    LanguageData languageKey = languageDataList[selectID];
                    languageDataList[selectID] = languageDataList[selectID - 1];
                    languageDataList[selectID - 1] = languageKey;
                    selectID -= 1;

                    if (selectID * 35 < scrollPos1.y)
                        scrollPos1.y = selectID * 35;
                }
            }
            if (GUI.Button(new Rect(startX + 227, startY - 20, 45, 18), "down"))
            {
                if (selectID < languageDataList.Count - 1)
                {
                    LanguageData languageKey = languageDataList[selectID];
                    languageDataList[selectID] = languageDataList[selectID + 1];
                    languageDataList[selectID + 1] = languageKey;
                    selectID += 1;

                    if (listVisibleRect.height - 35 < selectID * 35)
                        scrollPos1.y = (selectID + 1) * 35 - listVisibleRect.height + 5;
                }
            }
        }


        listVisibleRect = new Rect(startX, startY, width + 15, window.position.height - startY - 5);
        listContentRect = new Rect(startX, startY, width, languageDataList.Count * 35 + 5);

        GUI.color = new Color(.8f, .8f, .8f, 1f);
        GUI.Box(listVisibleRect, "");
        GUI.color = Color.white;

        scrollPos1 = GUI.BeginScrollView(listVisibleRect, scrollPos1, listContentRect);


        startY += 5; startX += 5;

        for (int i = 0; i < languageDataList.Count; i++)
        {
            if (minimiseList)
            {
                if (selectID == i)
                    GUI.color = new Color(0, 1f, 1f, 1f);
                if (GUI.Button(new Rect(startX + 35, startY + (i * 35), 30, 30), ""))
                    SelectLanguageKey(i);
                GUI.color = Color.white;

                continue;
            }



            if (selectID == i)
                GUI.color = new Color(0, 1f, 1f, 1f);
            if (GUI.Button(new Rect(startX + 35, startY + (i * 35), 150, 30), languageDataList[i].Key))
                SelectLanguageKey(i);
            GUI.color = Color.white;

            if (deleteID == i)
            {

                if (GUI.Button(new Rect(startX + 190, startY + (i * 35), 60, 15), "cancel")) deleteID = -1;

                GUI.color = Color.red;
                if (GUI.Button(new Rect(startX + 190, startY + (i * 35) + 15, 60, 15), "confirm"))
                {
                    if (selectID >= deleteID)
                        SelectLanguageKey(Mathf.Max(0, selectID - 1));
                    if (selectID >= deleteID)
                        SelectLanguageKey(Mathf.Max(0, selectID - 1));
                    EditorDataManager.RemoveLanguageKey(deleteID);
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

    Vector2 DrawLanguageKeyConfigurator(float startX, float startY, List<LanguageData> languageDataList)
    {
        LanguageData languageKey = languageDataList[selectID];

        GUIStyle style = new GUIStyle("TextArea");
        style.wordWrap = true;

        cont = new GUIContent("Key:");
        EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        languageKey.Key = EditorGUI.TextField(new Rect(startX + spaceX, startY, 120, 20), languageKey.Key);

        cont = new GUIContent("Tw:");
        EditorGUI.LabelField(new Rect(startX, startY += spaceY +10, width, height), cont);
        languageKey.LanguageString[(int)LanguageType.tw] = EditorGUI.TextField(new Rect(startX + spaceX, startY, 120, 20), languageKey.LanguageString[(int)LanguageType.tw]);

        cont = new GUIContent("En:");
        EditorGUI.LabelField(new Rect(startX, startY += spaceY + 10, width, height), cont);
        languageKey.LanguageString[(int)LanguageType.en] = EditorGUI.TextField(new Rect(startX + spaceX, startY, 120, 20), languageKey.LanguageString[(int)LanguageType.en]);

        return new Vector2(startX, startY);
    }

}
