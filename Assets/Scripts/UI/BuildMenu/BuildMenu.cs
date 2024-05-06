using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenu : MonoBehaviour
{
    [SerializeField] private TurretBlueprint standardTurret;
    [SerializeField] private TurretBlueprint missleLauncher;
    [SerializeField] private TurretBlueprint laserBeamer;
    [SerializeField] private BuildMenuItem buildMenuItemObj;
    [SerializeField] private Transform menuItemParent;
    private BuildManager buildManager => BuildManager.instance;

    [SerializeField] private Transform[] shopTurretItems;

    public void SelectStandardTurret()
    {
        buildManager.SelectTurretToBuild(standardTurret);
    }

    public void SelectMissleLauncher()
    {
        buildManager.SelectTurretToBuild(missleLauncher);
    }

    public void SelectLaserBeamer()
    {
        buildManager.SelectTurretToBuild(laserBeamer);
    }

    [ContextMenu("SetCost")]
    public void SetTurretCost()
    {
        shopTurretItems[0].GetComponent<BuildMenuItem>().SetCost(standardTurret.Cost);
        shopTurretItems[1].GetComponent<BuildMenuItem>().SetCost(missleLauncher.Cost);
        shopTurretItems[2].GetComponent<BuildMenuItem>().SetCost(laserBeamer.Cost);
    }
}
