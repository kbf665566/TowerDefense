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
    public bool HasMoney { get { return gameManager.Money >= turretToBuild.Cost; } }

    public void BuildTurretOn(Node node)
    {
        if (gameManager.Money < turretToBuild.Cost)
        {
            Debug.Log("½a");
            return;
        }

        gameManager.CostMoney(turretToBuild.Cost);

        GameObject turret = Instantiate(turretToBuild.turretPrefab, node.GetBuildPos(turretToBuild.BuildOffset), Quaternion.identity);
        node.BuildTurret(turret);

        GameObject effect = Instantiate(buildEffect, node.GetBuildPos(turretToBuild.BuildOffset),Quaternion.identity);
        Destroy(effect,5f);
    }

    public void SelectTurretToBuild(TurretBlueprint turret)
    {
        turretToBuild = turret;
    }
}
