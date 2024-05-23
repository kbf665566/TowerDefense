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

    private IObjectPool<Bullet> objectPool;
    public IObjectPool<Bullet> ObjectPool { set => objectPool = value; }

    public void SetBullet(float speed,float explosionRadius,float damage)
    {
        this.speed = speed;
        this.explosionRadius = explosionRadius; 
        this.damage = damage;
    }

    public void Seek(Enemy enemy)
    {
        target = enemy;
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            objectPool.Release(this);
            return;
        }

        Vector3 dir = target.transform.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

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
        EventHelper.EffectShowEvent.Invoke(this,GameEvent.GameEffectShowEvent.CreateEvent(transform.position,GameEffectType.Boom));

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
        target.TakeDamage(damage);
    }

    protected void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();
        if (e != null)
            e.TakeDamage(damage);
    }

    protected void Explode()
    {
        Collider[] cols =  Physics.OverlapSphere(transform.position,explosionRadius);
        foreach(Collider col in cols)
        {
            if(col.CompareTag("Enemy"))
            {
                Damage(col.transform);
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
