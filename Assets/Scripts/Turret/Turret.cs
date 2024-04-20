using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;
    [Header("Attributes")]
    [SerializeField] private float range = 15f;
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float fireCountdown = 0f;

    [Header("Setting")]
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private Transform partToRotation;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(UpdateTarget),0f,0.5f);
    }

    private void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach(GameObject enemy in enemies)
        {
            float distaceToEnemy = Vector3.Distance(transform.position,enemy.transform.position);
            if(distaceToEnemy < shortestDistance)
            {
                shortestDistance = distaceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if(nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotation.rotation,lookRotation,Time.deltaTime * turnSpeed).eulerAngles;
        partToRotation.rotation = Quaternion.Euler(0f,rotation.y,0f);


        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab,firePoint.position,firePoint.rotation);
        Bullet bullet = bulletObj.GetComponent<Bullet>();

        if (bullet != null)
            bullet.Seek(target);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,range);
    }
}
