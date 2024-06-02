using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportTower : TowerInLevel,ITowerRange
{
    private float shootRange;

    private float buffAddDamage;
    private float buffAddShootRange;
    private float buffAddFireRate;

    private Collider[] hitTargets = new Collider[30];
    [SerializeField] private LayerMask towerLayer = 1 << 8;
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
        SupportOtherTower();
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

    public void SupportOtherTower()
    {
       var towerAmount = Physics.OverlapSphereNonAlloc(transform.position, shootRange, hitTargets, towerLayer);
        if(towerAmount > 0)
        {
            foreach (Collider col in hitTargets)
            {
                if (col == null)
                    continue;

                if (col.gameObject.activeSelf)
                {
                    var tower = col.GetComponent<TowerInLevel>();
                    if(tower != null)
                    {
                        tower.TowerGetSupport(buffAddDamage,buffAddShootRange,buffAddFireRate);
                    }
                }
            }
        }
    }

    public void SellToResetSupport()
    {
        var towerAmount = Physics.OverlapSphereNonAlloc(transform.position, shootRange, hitTargets, towerLayer);
        if (towerAmount > 0)
        {
            foreach (Collider col in hitTargets)
            {
                if (col == null)
                    continue;

                if (col.gameObject.activeSelf)
                {
                    var tower = col.GetComponent<TowerInLevel>();
                    if (tower != null)
                    {
                        tower.ResetTowerSupport();
                    }
                }
            }
        }
    }
}
