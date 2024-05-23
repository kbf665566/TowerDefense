using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.WSA;

public class Enemy : MonoBehaviour
{
    private GameManager gameManager => GameManager.instance;

    private int uid = 0;

    private float speed;
    private float startSpeed = 10f;
    private int damage = 1;
    private float hp = 100;
    private int value = 50;

    private float rotationSpeed = 10f;

    private bool immuneStun;
    private bool immuneSlow;

    private bool isDead = false;
    public bool IsDead => isDead;

    private IObjectPool<Enemy> objectPool;
    public IObjectPool<Enemy> ObjectPool { set => objectPool = value; }

    private EnemyData enemyData;

    private Transform target;
    private int wavepointIndex = 0;
    private Transform[] waypoints;

    private void Start()
    {
        speed = startSpeed;
    }

    void Update()
    {
        if (isDead)
            return;
        
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (dir != Vector3.zero)
        {
            Quaternion wantedRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, wantedRot, rotationSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, target.position) <= 0.25f)
        {
            GetNextWaypoint();
        }

        //ResetSpeed();
    }

    public void SetEnemy(EnemyData enemyData)
    {
        this.enemyData = enemyData;
        hp = enemyData.HP;
        value = enemyData.Value;
        
        startSpeed = enemyData.Speed;
        speed = enemyData.Speed;
        
        immuneSlow = enemyData.ImmuneSlow;
        immuneStun= enemyData.ImmuneStun;

        damage = enemyData.Damage;
    }

    public void ReSwpawn(Transform[] waypoints,int uid)
    {
        hp = enemyData.HP;
        speed = enemyData.Speed;
        this.waypoints = waypoints;
        target = waypoints[0];
        wavepointIndex = 0;
        isDead = false;
        this.uid = uid;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        hp -= amount;
        if (hp <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        EventHelper.EffectShowEvent.Invoke(this,GameEvent.GameEffectShowEvent.CreateEvent(transform.position,enemyData.DeadParticle));

        //發送死亡事件
        EventHelper.EnemyDiedEvent.Invoke(this,GameEvent.EnemyDieEvent.CreateEvent(uid,value));
        objectPool.Release(this);
    }

    public void Slow(float pct)
    {
        speed = startSpeed * (1f - pct);
    }

    public void ResetSpeed()
    {
        speed = startSpeed;
    }

    private void GetNextWaypoint()
    {
        if (wavepointIndex >= waypoints.Length - 1)
        {
            EndPath();
            return;
        }

        wavepointIndex++;
        target = waypoints[wavepointIndex];
    }

    private void EndPath()
    {
        objectPool.Release(this);
        EventHelper.EnemyEndPathEvent.Invoke(this, GameEvent.EnemyEndPathEvent.CreateEvent(uid,damage));
    }
}
