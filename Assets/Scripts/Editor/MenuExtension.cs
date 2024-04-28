using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class MenuExtension : EditorWindow
{
    [MenuItem("Tools/TowerDefense/TowerEditor", false, 10)]
    static void OpenTowerEditor()
    {
        TowerEditorWindow.Init();
    }

    [MenuItem("Tools/TowerDefense/EnemyEditor", false, 10)]
    static void OpenEnemyEditor()
    {
        EnemyEditorWindow.Init();
    }
}
