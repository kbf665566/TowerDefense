using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnitEditorWindow : EditorWindow
{
    protected static GUIContent cont;
    protected static GUIContent[] contL;

    protected static float spaceX = 110;
    protected static float spaceY = 20;
    protected static float width = 150;
    protected static float height = 18;



    public static Vector3 DrawIconAndName(string name,Sprite icon, float startX, float startY)
    {
        float cachedX = startX;
        float cachedY = startY;

        DrawSprite(new Rect(startX, startY, 60, 60), icon);
        startX += 65;

        cont = new GUIContent("¦WºÙ:");
        EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        EditorGUI.TextField(new Rect(startX + spaceX - 65, startY, width - 5, height), name);

        cont = new GUIContent("Icon:");
        EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
        EditorGUI.ObjectField(new Rect(startX + spaceX - 65, startY, width - 5, height), icon, typeof(Sprite), false);

        startX -= 65;
        startY = cachedY;
        startX += 35 + spaceX + width;
        GUI.color = Color.white;


        float contentWidth = startX - cachedX + spaceX + 25;

        return new Vector3(startX, startY + spaceY, contentWidth);
    }



    public static bool DrawSprite(Rect rect, Sprite sprite, bool addXButton = false, bool drawBox = true)
    {
        if (drawBox) GUI.Box(rect, "");

        if (sprite != null)
        {
            Texture t = sprite.texture;
            Rect tr = sprite.textureRect;
            Rect r = new Rect(tr.x / t.width, tr.y / t.height, tr.width / t.width, tr.height / t.height);

            rect.x += 2;
            rect.y += 2;
            rect.width -= 4;
            rect.height -= 4;
            GUI.DrawTextureWithTexCoords(rect, t, r);
        }

        if (addXButton)
        {
            rect.width = 12; rect.height = 12;
            bool flag = GUI.Button(rect, "X", GetXButtonStyle());
            return flag;
        }

        return false;
    }

    private static GUIStyle xButtonStyle;
    public static GUIStyle GetXButtonStyle()
    {
        if (xButtonStyle == null)
        {
            xButtonStyle = new GUIStyle("Button");
            xButtonStyle.alignment = TextAnchor.MiddleCenter;
            xButtonStyle.padding = new RectOffset(0, 0, 0, 0);
        }
        return xButtonStyle;
    }
}
