using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    private GameManager gameManager => GameManager.instance;

    private TurretBlueprint turretToBuild;
    public TurretBlueprint TurretTobuild => turretToBuild;

    [SerializeField]private GameObject standardTurretPrefab;
    public GameObject StandardTurretPrefab => standardTurretPrefab;

    [SerializeField] private GameObject missleLauncherPrefab;
    public GameObject MissleLauncherPrefab => missleLauncherPrefab;

    [SerializeField] private GameObject buildEffect;
    public GameObject BuildEffect => buildEffect;
    [SerializeField] private GameObject sellEffect;
    public GameObject SellEffect => sellEffect;

    private GridManager gridManager;

    public bool CanBuild { get { return turretToBuild != null; } }

    private Node selectNode;
    public delegate void NodeSelected(Node node);
    public static NodeSelected nodeSelected;
    public delegate void NodeCancelSelected();
    public static NodeCancelSelected nodeCancelSelected;

#if UNITY_EDITOR
    private GridDebugger gridDebugger;
#endif

    /*建一個Prefab用來顯示選取、預覽的網格狀態*/
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("有多個BuildManager");
            return;
        }
        instance = this;

        var mapData = gameManager.NowMapData;
        gridManager = new GridManager(Vector2Short.Zero, mapData.MapSize, mapData.GetBlockGridPosList(mapData.BlockGridList), mapData.GetEnemyPathPosList(mapData.EnemyPathList));

    #if UNITY_EDITOR
        gridDebugger = new GridDebugger();
        gridDebugger.Create(gameManager.NowMapData.MapSize);
    #endif

}

public void SelectTurretToBuild(TurretBlueprint turret)
    {
        turretToBuild = turret;
        nodeCancelSelected?.Invoke();
    }

    public GridState GetGridState(Vector2Short gridPos)
    {
        var gridState = gridManager.GetGridState(gridPos.x,gridPos.y);
        return gridState;
    }
    public GridState GetGridState(int x,int y)
    {
        var gridState = gridManager.GetGridState(x, y);
        return gridState;
    }

    private void SelectNode(Node node)
    {
        if(selectNode == node)
        {
            nodeCancelSelected?.Invoke();
            return;
        }

        selectNode = node;
        turretToBuild = null;
    }

    public void DeselectNode()
    {
        selectNode = null;
    }

    private void OnDisable()
    {
        nodeSelected -= SelectNode;
        nodeCancelSelected -= DeselectNode;
    }
}
