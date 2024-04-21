using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected GameObject impactEffect;
    protected Transform target;
    [SerializeField] protected float speed = 70f;
    [SerializeField] protected float explosionRadius = 0f;

    public void Seek(Transform enemy)
    {
        target = enemy;
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if(dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame,Space.World);
        transform.LookAt(target);
    }

    protected virtual void HitTarget()
    {
        GameObject effectObj = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectObj,5f);

        if (explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(target);
        }


        Destroy(gameObject);
    }

    protected void Damage(Transform enemy)
    {
        Destroy(enemy.gameObject);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,explosionRadius);
    }
}
