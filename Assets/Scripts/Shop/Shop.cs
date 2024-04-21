using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private TurretBlueprint standardTurret;
    [SerializeField] private TurretBlueprint missleLauncher;
    private BuildManager buildManager => BuildManager.instance;

    public void SelectStandardTurret()
    {
        buildManager.SelectTurretToBuild(standardTurret);

    }

    public void SelectMissleLauncher()
    {
        buildManager.SelectTurretToBuild(missleLauncher);

    }
}
