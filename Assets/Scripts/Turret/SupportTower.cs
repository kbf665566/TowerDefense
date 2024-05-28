﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportTower : TowerInLevel,ITowerRange
{
    private float shootRange;

    private float buffAddDamage;
    private float buffAddShootRange;
    private float buffAddFireRate;

    public override void SetTower(int uid, TowerData towerData, Vector2Short gridPos)
    {
        base.SetTower(uid, towerData,gridPos);
        towerType = TowerType.Support;

        SetLevelData(nowLevel);


        UpdateTowerState();
    }

    public override void LevelUp()
    {
        base.LevelUp();

        SetLevelData(nowLevel);
    }

    public override void SetLevelData(int level)
    {
        shootRange = towerLevelData[level].ShootRange;
        buffAddDamage = towerLevelData[level].BuffAddDamage;
        buffAddShootRange = towerLevelData[level].BuffAddRange;
        buffAddFireRate = towerLevelData[level].BuffAddFireRate;
    }

    public float GetShootRange()
    {
        return shootRange;
    }
}
