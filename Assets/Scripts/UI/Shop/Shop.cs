using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private TurretBlueprint standardTurret;
    [SerializeField] private TurretBlueprint missleLauncher;
    [SerializeField] private TurretBlueprint laserBeamer;
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
        shopTurretItems[0].GetComponent<ShopTurretItem>().SetCost(standardTurret.Cost);
        shopTurretItems[1].GetComponent<ShopTurretItem>().SetCost(missleLauncher.Cost);
        shopTurretItems[2].GetComponent<ShopTurretItem>().SetCost(laserBeamer.Cost);
    }
}
