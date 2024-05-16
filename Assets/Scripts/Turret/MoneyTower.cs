using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyTower : TowerInLevel
{
    private int getMoney;

    private float originFireRate;

    private float support_FireRate;

    private float final_FireRate;

    private float fireTimer;

    [SerializeField] private Transform firePoint;
    public override void SetTower(int uid, TowerData towerData, Vector2Short gridPos)
    {
        base.SetTower(uid, towerData,gridPos);
        towerType = TowerType.Money;


        SetLevelData(nowLevel);
    }

    public override void LevelUp()
    {
        base.LevelUp();

        SetLevelData(nowLevel);
    }

    public override void SetLevelData(int level)
    {
        originFireRate = towerLevelData[level].FireRate;
        getMoney = towerLevelData[level].GetMoney;

        UpdateTowerState();
    }

    public override void TowerGetSupport(float addDamage, float addRange, float addFireRate)
    {
        support_FireRate = support_FireRate > addFireRate ? support_FireRate : addFireRate;

        UpdateTowerState();
    }

    public override void ResetTowerSupport()
    {
        support_FireRate = 0;

        UpdateTowerState();
    }

    public override void UpdateTowerState()
    {
        final_FireRate = support_FireRate == 0 ? originFireRate : (float)Math.Round(originFireRate / support_FireRate, 3);
    }
}
