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

    [MenuItem("Tools/TowerDefense/LevelEditor", false, 10)]
    static void OpenWaveEditor()
    {
        LevelEditorWindow.Init();
    }

    [MenuItem("Tools/TowerDefense/MapEditor", false, 10)]
    static void OpenMapEditor()
    {
        MapEditorWindow.Init();
    }

    [MenuItem("Tools/TowerDefense/LanguageEditor", false, 10)]
    static void OpenLanguageEditor()
    {
        LanguageEditorWindow.Init();
    }
}
