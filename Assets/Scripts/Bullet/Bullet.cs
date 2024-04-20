using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject impactEffect;
    private Transform target;
    [SerializeField] private float speed = 70f;

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
    }

    private void HitTarget()
    {
        GameObject effectObj = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectObj,2f);

        Destroy(gameObject);
    }
}
