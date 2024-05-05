using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TurretBlueprint 
{
    public GameObject turretPrefab;
    public GameObject upgradeTurretPrefab;
    public int Cost;
    public Vector3 BuildOffset;
    public int UpgradeCost;

    public int GetSellPirce()
    {
        return Cost / 2;
    }
}
