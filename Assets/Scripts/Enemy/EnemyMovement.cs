using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    private Transform target;
    private int wavepointIndex = 0;
    private GameManager gameManager => GameManager.instance;
    private Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        target = gameManager.WayPoints[0];
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * enemy.Speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            GetNextWaypoint();
        }

        enemy.ResetSpeed();
    }

    private void GetNextWaypoint()
    {
        if (wavepointIndex >= gameManager.WayPoints.Length - 1)
        {
            EndPath();
            return;
        }

        wavepointIndex++;
        target = gameManager.WayPoints[wavepointIndex];
    }

    private void EndPath()
    {
        WaveSpawner.EnemiesAlive--;
        gameManager.LostLive(enemy.Damage);
        Destroy(gameObject);
    }
}
