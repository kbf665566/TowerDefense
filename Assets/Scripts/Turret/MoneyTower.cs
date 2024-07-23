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
    private LevelManager levelManager => LevelManager.instance;
    public override void SetTower(int uid, TowerData towerData, Vector2Short gridPos)
    {
        base.SetTower(uid, towerData,gridPos);
        towerType = TowerType.Money;
        support_FireRate = 1;

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
        support_FireRate = 1;

        UpdateTowerState();
    }

    public override void UpdateTowerState()
    {
        final_FireRate = (float)Math.Round(originFireRate / support_FireRate, 3);
    }

    private void FixedUpdate()
    {
        MakeMoney();
    }

    private void MakeMoney()
    {
        fireTimer += Time.fixedDeltaTime;
        fireTimer = fireTimer > final_FireRate ? final_FireRate : fireTimer;

        if (levelManager.NowWaveEnd)
            return;

        if (fireTimer >= final_FireRate)
        {
            GetMoney();
            fireTimer = 0;
        }
    }

    private void GetMoney()
    {
        EventHelper.TowerModeMoneyEvent.Invoke(this,GameEvent.TowerMakeMoneyEvent.CreateEvent(getMoney));
        EventHelper.EffectShowTextEvent.Invoke(this, GameEvent.GameEffectShowWithTextEvent.CreateEvent(firePoint.position ,towerData.NormalParticle,"+" + getMoney));
    }
}
