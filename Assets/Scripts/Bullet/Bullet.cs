using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    protected Enemy target;
    protected float speed = 70f;
    protected float explosionRadius = 0f;
    protected float damage = 50;

    protected DebuffType debuff;
    
    protected float amount;
    protected float duration;

    private IObjectPool<Bullet> objectPool;
    public IObjectPool<Bullet> ObjectPool { set => objectPool = value; }

    [SerializeField] private LayerMask enemyLayer;

    public void SetBullet(float speed, float explosionRadius, float damage,float amount, float duration,DebuffType debuff)
    {
        this.speed = speed;
        this.explosionRadius = explosionRadius;
        this.damage = damage;

        this.amount = amount;
        this.duration = duration;
        this.debuff = debuff;
    }

    public void Seek(Enemy enemy)
    {
        target = enemy;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(target == null)
        {
            objectPool.Release(this);
            return;
        }

        Vector3 dir = target.transform.position - transform.position;
        float distanceThisFrame = speed * Time.fixedDeltaTime;

        if(dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame,Space.World);
        transform.LookAt(target.transform);
    }

    protected virtual void HitTarget()
    {
        //發出特效事件
        EventHelper.EffectShowEvent.Invoke(this,GameEvent.GameEffectShowEvent.CreateEvent(transform.position,GameEffectType.BulletHit));
        if (explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage();
        }


        objectPool.Release(this);
    }

    protected void Damage()
    {
        target.TakeDamage(damage,amount,duration,debuff);
    }

    protected void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();
        if (e != null)
            e.TakeDamage(damage, amount, duration, debuff);
    }

    protected void Explode()
    {
        Collider[] cols =  Physics.OverlapSphere(transform.position,explosionRadius, enemyLayer);
        foreach(Collider col in cols)
        {
            if (col.gameObject.activeSelf)
            {
                if (col.CompareTag("Enemy"))
                {
                    Damage(col.transform);
                }
            }
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