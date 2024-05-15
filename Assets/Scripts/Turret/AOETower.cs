using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOETower : TowerInLevel
{
    private float originDamage;
    private float originShootRange;
    private float originFireRate;

    private float support_Damage;
    private float support_ShootRange;
    private float support_FireRate;

    private float final_Damage;
    private float final_ShootRange;
    private float final_FireRate;

    private bool canSlow;
    private bool canStun;

    private float slowAmount;
    private float slowDuration;

    private float stunProbability;
    private float stunDuration;

    private float fireTimer;

    [SerializeField] private Transform firePoint;
    public override void SetTower(int uid, TowerData towerData)
    {
        base.SetTower(uid, towerData);
        towerType = TowerType.AOE;
        canSlow = towerData.CanSlowEnemy;
        canStun = towerData.CanStunEnemy;

        SetLevelData(nowLevel);

        fireTimer = originFireRate;
    }

    public override void LevelUp()
    {
        base.LevelUp();

        SetLevelData(nowLevel);
    }

    public override void SetLevelData(int level)
    {
        originDamage = towerLevelData[level].Damage;
        originShootRange = towerLevelData[level].ShootRange;
        originFireRate = towerLevelData[level].FireRate;

        if (canSlow)
        {
            slowAmount = towerLevelData[level].SlowAmount;
            slowDuration = towerLevelData[level].SlowDuration;
        }

        if (canStun)
        {
            stunProbability = towerLevelData[level].StunProbability;
            stunDuration = towerLevelData[level].StunDuration;
        }

        UpdateTowerState();
    }

    public override void TowerGetSupport(float addDamage, float addRange, float addFireRate)
    {
        support_Damage = support_Damage > addDamage ? support_Damage : addDamage;
        support_ShootRange = support_ShootRange > addRange ? support_ShootRange : addRange;
        support_FireRate = support_FireRate > addFireRate ? support_FireRate : addFireRate;

        UpdateTowerState();
    }

    public override void ResetTowerSupport()
    {
        support_Damage = 0;
        support_ShootRange = 0;
        support_FireRate = 0;

        UpdateTowerState();
    }

    public override void UpdateTowerState()
    {
        final_Damage = support_Damage == 0 ? originDamage : originDamage * support_Damage;
        final_ShootRange = support_ShootRange == 0 ? originShootRange : originShootRange * support_ShootRange;
        final_FireRate = support_FireRate == 0 ? originFireRate : (float)Math.Round(originFireRate / support_FireRate, 3);
    }
}
