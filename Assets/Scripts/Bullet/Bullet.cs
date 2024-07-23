using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using UnityEngine;
using UnityEngine.Pool;
using Color = UnityEngine.Color;
using Debug = UnityEngine.Debug;

public class Bullet : MonoBehaviour
{
    protected Enemy target;
    protected Vector3 targetPos;
    protected bool isSpecificPoint;

    protected float speed = 70f;
    protected float explosionRadius = 0f;
    protected float damage = 50;

    protected float bulletHitRange = 0.4f;

    protected DebuffType debuff;

    protected float amount;
    protected float defuffDuration;


    protected float existTime;
    protected WaitForSeconds hideTime;

    protected IObjectPool<Bullet> objectPool;
    public IObjectPool<Bullet> ObjectPool { set => objectPool = value; }

    protected LayerMask enemyLayer => GameSetting.EnemyLayer;
    protected Collider[] rangeHitTargets;
    protected Collider[] bulletHitTargets = new Collider[1];

    protected EnemyManager enemyManager => EnemyManager.instance;

    protected float towerRange;

    protected RaycastHit[] rayHitTatget = new RaycastHit[1];

    public virtual void SetBullet(float speed, float explosionRadius, float damage,
        float amount, float duration, DebuffType debuff, float existTime)
    {
        this.speed = speed;
        this.explosionRadius = explosionRadius;
        this.damage = damage;

        this.amount = amount;
        defuffDuration = duration;
        this.debuff = debuff;

        if (rangeHitTargets == null && explosionRadius > 0)
            rangeHitTargets = new Collider[50];
        isSpecificPoint = false;
    }

    public void Seek(Enemy enemy)
    {
        target = enemy;
    }

    public void Seek(Vector3 targetPos)
    {
        this.targetPos = new Vector3(targetPos.x,transform.position.y, targetPos.z);
        isSpecificPoint = true;
    }

    public void SetHideRange(float range)
    {
        towerRange = range;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    protected virtual void Move()
    {

    }

    protected virtual void HitTarget(Transform target = null)
    {
        //發出特效事件
        EventHelper.EffectShowEvent.Invoke(this,GameEvent.GameEffectShowEvent.CreateEvent(transform.position,GameEffectType.BulletHit));
        if (explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            if (target != null)
                Damage(target);
        }
        Hide();
    }

    protected void Damage(Transform enemy)
    {
        if (enemy.TryGetComponent<Enemy>(out var e))
            e.TakeDamage(damage, amount, defuffDuration, debuff);
    }

    protected void Explode()
    {
        var amount = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, rangeHitTargets, enemyLayer);
        if (amount == 0)
            return;

        foreach (Collider col in rangeHitTargets)
        {
            if (col == null)
                continue;

            if (col.gameObject.activeSelf && col.CompareTag("Enemy"))
            {
                Damage(col.transform);
            }
        }
    }

    /// <summary>
    /// 超出範圍就自動回收
    /// </summary>
    protected virtual bool OutRangeToHide()
    {
        var distance = Vector3.Distance(transform.localPosition,Vector3.zero);
        if(distance >= towerRange)
        {
            Hide();
            return true;
        }
        return false;
    }

    protected virtual void Hide()
    {
        target = null;
        objectPool.Release(this);
    }

    protected bool TargetIsDied()
    {
        return target == null || target.gameObject.activeSelf == false;
    }


    protected virtual void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.activeSelf && col.CompareTag("Enemy"))
        {
            HitTarget(col.transform);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,explosionRadius);
    }
#endif
}