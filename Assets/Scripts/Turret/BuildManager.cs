using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    private TurretBlueprint turretToBuild;
    public TurretBlueprint TurretTobuild => turretToBuild;

    [SerializeField]private GameObject standardTurretPrefab;
    public GameObject StandardTurretPrefab => standardTurretPrefab;

    [SerializeField] private GameObject missleLauncherPrefab;
    public GameObject MissleLauncherPrefab => missleLauncherPrefab;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("¦³¦h­ÓBuildManager");
            return;
        }
        instance = this;
    }

    public bool CanBuild { get { return turretToBuild != null; } }

    public void BuildTurretOn(Node node)
    {
        if (GameManager.instance.Money < turretToBuild.cost)
        {
            Debug.Log("½a");
            return;
        }

        GameManager.instance.CostMoney(turretToBuild.cost);

        GameObject turret = Instantiate(turretToBuild.turretPrefab, node.GetBuildPos(), Quaternion.identity);
        node.BuildTurret(turret);
    }

    public void SelectTurretToBuild(TurretBlueprint turret)
    {
        turretToBuild = turret;
    }
}
