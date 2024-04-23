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

    public bool CanBuild { get { return turretToBuild != null; } }
    public bool HasMoney { get { return gameManager.Money >= turretToBuild.Cost; } }

    private Node selectNode;
    public delegate void NodeSelected(Node node);
    public static NodeSelected nodeSelected;
    public delegate void NodeCancelSelected();
    public static NodeCancelSelected nodeCancelSelected;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("¦³¦h­ÓBuildManager");
            return;
        }
        instance = this;
        nodeSelected += SelectNode;
        nodeCancelSelected += DeselectNode;
    }

    public void SelectTurretToBuild(TurretBlueprint turret)
    {
        turretToBuild = turret;
        nodeCancelSelected?.Invoke();
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
