using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class AOETower : TowerInLevel,IAttackTower,ITowerRange
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

    private DebuffType debuff;

    private float slowAmount;
    private float slowDuration;

    private float stunProbability;
    private float stunDuration;

    private float fireTimer;

    private EnemyManager enemyManager => EnemyManager.instance;

    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask enemyLayer;

    private Collider[] hitTargets = new Collider[30];
    private bool nowCanFindEnemy = false;
    public override void SetTower(int uid, TowerData towerData, Vector2Short gridPos)
    {
        base.SetTower(uid, towerData,gridPos);
        towerType = TowerType.AOE;
        debuff = towerData.Debuff;

        support_Damage = 1;
        support_ShootRange = 1;
        support_FireRate = 1;

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

        if (debuff == DebuffType.Slow)
        {
            slowAmount = towerLevelData[level].SlowAmount;
            slowDuration = towerLevelData[level].SlowDuration;
        }

        if (debuff == DebuffType.Stun)
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
        support_Damage = 1;
        support_ShootRange = 1;
        support_FireRate = 1;

        UpdateTowerState();
    }

    public override void UpdateTowerState()
    {
        final_Damage =  originDamage * support_Damage;
        final_ShootRange = originShootRange * support_ShootRange;
        final_FireRate = (float)Math.Round(originFireRate / support_FireRate, 3);
    }

    private void FixedUpdate()
    {
        FireToEnemy();
    }

    public void FindEnemy()
    {
        nowCanFindEnemy = enemyManager.GetNowEnemyAmount() > 0;
    }

    public void FireToEnemy()
    {
        fireTimer += Time.fixedDeltaTime;
        fireTimer = fireTimer > final_FireRate ? final_FireRate : fireTimer;

        FindEnemy();

        if (!nowCanFindEnemy)
            return;
        

        if (fireTimer >= final_FireRate)
        {
            int count = 0;
            if (Time.frameCount % 10 == 0)
                count = Physics.OverlapSphereNonAlloc(transform.position, final_ShootRange, hitTargets, enemyLayer);

            if (count > 0)
            {
                Shoot();
                fireTimer = 0;
            }
        }
    }

    public (float amount, float duration) DebuffProcess()
    {
        float amount = 0f;
        float duration = 0f;
        if (debuff == DebuffType.Stun)
        {
            amount = stunProbability;
            duration = stunDuration;

        }
        else if (debuff == DebuffType.Slow)
        {
            amount = slowAmount;
            duration = slowDuration;
        }

        return (amount, duration);
    }

    public void Shoot()
    {
        var debuffCount = DebuffProcess();

        EventHelper.EffectShowEvent.Invoke(this, GameEvent.GameEffectShowEvent.CreateEvent(firePoint.transform.position, towerData.AttackParticle,final_ShootRange));
        foreach (Collider col in hitTargets)
        {
            if (col == null)
                continue;

            if (col.gameObject.activeSelf && col.CompareTag("Enemy"))
            {
                var enemy = col.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(final_Damage, debuffCount.amount, debuffCount.duration, debuff);
                }
            }
        }
    }

    public float GetShootRange()
    {
        return final_ShootRange;
    }
}
