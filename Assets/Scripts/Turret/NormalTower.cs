using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class NormalTower : TowerInLevel,IAttackTower,ITowerRange
{
    protected float originDamage;
    protected float originShootRange;
    protected float originFireRate;

    protected float support_Damage;
    protected float support_ShootRange;
    protected float support_FireRate;

    protected float final_Damage;
    protected float final_ShootRange;
    protected float final_FireRate;

    protected float bulletSpeed;
    protected float bulletExplsionRadius;

    protected bool useBullet;
    protected DebuffType debuff;

    protected float slowAmount;
    protected float slowDuration;

    protected float stunProbability;
    protected float stunDuration;

    protected float fireTimer;

    protected bool isNeedTurn = true;
    /// <summary> 判斷這個角度能不能攻擊 </summary>
    protected bool angleCanFire = false;

    [SerializeField] protected Transform firePoint;
    [SerializeField] protected Transform partToRotation;
    [SerializeField] protected float turnSpeed = 10f;

    protected EnemyManager enemyManager => EnemyManager.instance;
    protected Enemy targetEnemy;

    private IObjectPool<Bullet> bulletPool;
    private bool collectionCheck = true;
    private int defaultCapacity = 20;
    private int maxSize = 100;

    public override void SetTower(int uid, TowerData towerData, Vector2Short gridPos)
    {
        base.SetTower(uid, towerData, gridPos);
        towerType = TowerType.Normal;

        useBullet = true;
        debuff = towerData.Debuff;

        isNeedTurn = towerData.IsNeedTurn;
        if (isNeedTurn == false)
            angleCanFire = true;

        support_Damage = 1;
        support_ShootRange = 1;
        support_FireRate = 1;

        SetLevelData(nowLevel);

        fireTimer = originFireRate;

        if (bulletPool == null && useBullet == true)
        {
            bulletPool = new ObjectPool<Bullet>(CreateBullet,
           OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
           collectionCheck, defaultCapacity, maxSize);
        }

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

        bulletSpeed = towerLevelData[level].BulletSpeed;
        bulletExplsionRadius = towerLevelData[level].BulletExplosionRadius;

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
        support_FireRate = support_FireRate> addFireRate ? support_FireRate : addFireRate;

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
        final_Damage = originDamage * support_Damage;
        final_ShootRange = originShootRange * support_ShootRange;
        final_FireRate = (float)Math.Round(originFireRate / support_FireRate,3);
    }

    private void FixedUpdate()
    {
        FireToEnemy();
    }

    public void FindEnemy()
    {
        targetEnemy = enemyManager.FindNearestEnemy(transform.localPosition, final_ShootRange);
    }

    public void FireToEnemy()
    {
        fireTimer += Time.fixedDeltaTime;
        fireTimer = fireTimer > final_FireRate ? final_FireRate : fireTimer;

        if (targetEnemy == null)
        {
            FindEnemy();
            return;
        }

        if (targetEnemy.IsDead || Vector3.Distance(targetEnemy.transform.position, transform.position) > final_ShootRange)
        {
            targetEnemy = null;
            return;
        }

        if (isNeedTurn == true)
            LockOnTarget();

        if (fireTimer >= final_FireRate && angleCanFire == true)
        {
            Shoot();
            fireTimer = 0;
        }
    }
    protected void LockOnTarget()
    {
        Vector3 dir = targetEnemy.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotation.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotation.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        angleCanFire = MathF.Abs(Quaternion.Angle(partToRotation.rotation, lookRotation)) <= 5f;
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

    public virtual void Shoot()
    {
        var bullet = bulletPool.Get();
        if (bullet != null)
        {
            var debuffCount = DebuffProcess();

            EventHelper.EffectShowEvent.Invoke(this, GameEvent.GameEffectShowEvent.CreateEvent(firePoint.transform.position, towerData.AttackParticle));
            bullet.SetBullet(towerData.TowerLevelData[nowLevel].BulletSpeed,
                  towerData.TowerLevelData[nowLevel].BulletExplosionRadius, final_Damage,
                  debuffCount.amount, debuffCount.duration, debuff);

            bullet.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
            bullet.Seek(targetEnemy);
        }
    }

    public float GetShootRange()
    {
        return final_ShootRange;
    }

    #region  物件池
    // 物件池中的物件不夠時，建立新的物件去填充物件池
    private Bullet CreateBullet()
    {
        var obj = Instantiate(towerData.TowerLevelData[nowLevel].TowerBullet);
        Bullet newBullet = obj.GetComponent<Bullet>();

        obj.transform.SetParent(transform);
        newBullet.ObjectPool = bulletPool;

        return newBullet;
    }

    // 將物件放回物件池
    private void OnReleaseToPool(Bullet pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }

    // 從物件池中取出物件
    private void OnGetFromPool(Bullet pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }

    // 當超出物件池的上限時，將物件Destroy
    private void OnDestroyPooledObject(Bullet pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }

    #endregion
}
