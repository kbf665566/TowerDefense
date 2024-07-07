using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    private GameManager gameManager => GameManager.instance;
    private LevelManager levelManager => LevelManager.instance;

    private Dictionary<int, IObjectPool<Enemy>> enemyObjectPools;
    private int defaultCapacity = 20;
    private int maxCapacity = 200;
    private bool collectionCheck = true;
    private int nowSelectEnemyId;

    private int totalEnemyAmount = 0;
    private int nowDiedEnemyAmount = 0;

    private LevelAllWaves waves;
    private List<WaveEnemy> nowWaveEnemies = new List<WaveEnemy>();
    private int nowWave = 0;

    private bool startSpawn = false;
    private float spawnTimeCount = 0;
    private int nowSpawnIndex = 0;
    private int spawnEnemyCount = 0;

    private float nowLevelTime = 0;

    private Dictionary<int, Enemy> nowEnemies = new Dictionary<int, Enemy>();

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("有多個EnemyManager");
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

        enemyObjectPools = new Dictionary<int, IObjectPool<Enemy>>();
        for (int i = 0; i < GameManager.instance.EnemyData.EnemyList.Count; i++)
            enemyObjectPools.Add(GameManager.instance.EnemyData.EnemyList[i].Id, new ObjectPool<Enemy>(CreateEnemy,
            OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
            collectionCheck, defaultCapacity, maxCapacity));

        waves = gameManager.NowWaveData;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (startSpawn == false)
            return;

        if (spawnTimeCount >= nowWaveEnemies[nowSpawnIndex].SpawnInterval)
        {
            if(nowWaveEnemies[nowSpawnIndex].SpawnInAllPath == true && gameManager.NowMapData.EnemyPathList.Count == 2)
            {
                SpawnEnemy(nowWaveEnemies[nowSpawnIndex].EnemyID, 0);
                SpawnEnemy(nowWaveEnemies[nowSpawnIndex].EnemyID, 1);
            }
            else if(nowWaveEnemies[nowSpawnIndex].SpawnInAllPath == false)
                SpawnEnemy(nowWaveEnemies[nowSpawnIndex].EnemyID, nowWaveEnemies[nowSpawnIndex].PathId);


            spawnEnemyCount++;

            if(spawnEnemyCount >= nowWaveEnemies[nowSpawnIndex].SpawnAmount)
            {
                spawnEnemyCount = 0;
                nowSpawnIndex++;
                if (nowSpawnIndex >= nowWaveEnemies.Count)
                    startSpawn = false;
            }

            spawnTimeCount = 0;
        }

        nowLevelTime += Time.fixedDeltaTime;
        spawnTimeCount += Time.fixedDeltaTime;
    }

    private void NextWaveStart(object s,GameEvent.NextWaveStartEvent e)
    {
        nowWaveEnemies.Clear();
        totalEnemyAmount = 0;
        nowDiedEnemyAmount = 0;
        nowWave = e.NowWave;

        nowLevelTime = 0;

        //計算這波敵人總數
        for (int i = 0;i < waves.WaveList[nowWave - 1].WaveEnemyList.Count;i++)
        {
            nowWaveEnemies.Add(waves.WaveList[nowWave - 1].WaveEnemyList[i]);
            totalEnemyAmount += waves.WaveList[nowWave - 1].WaveEnemyList[i].SpawnAmount;
            if(waves.WaveList[nowWave - 1].WaveEnemyList[i].SpawnInAllPath == true && gameManager.NowMapData.EnemyPathList.Count == 2)
                totalEnemyAmount += waves.WaveList[nowWave - 1].WaveEnemyList[i].SpawnAmount;
        }

        startSpawn = true;
        nowSpawnIndex = 0;
        spawnTimeCount = nowWaveEnemies[nowSpawnIndex].SpawnInterval;
        spawnEnemyCount = 0;
    }

    private void SpawnEnemy(int enemyId,int pathId)
    {
        nowSelectEnemyId = enemyId;
        if (enemyObjectPools.TryGetValue(enemyId,out var pool))
        {
            var newEnemy = pool.Get();
            var wayPoints = pathId == 0 ? levelManager.WayPoints1 : levelManager.WayPoints2;
            var uid = GenerateUid();
            newEnemy.ReSwpawn(wayPoints,uid,nowLevelTime);
            newEnemy.transform.SetPositionAndRotation(wayPoints[0].transform.position,Quaternion.identity);
            nowEnemies.Add(uid,newEnemy);
        }
    }

    private void EnemyDied(object s, GameEvent.EnemyDieEvent e)
    {
        EnemyDestroyed(e.Uid);
    }

    private void EnemyEndPath(object s, GameEvent.EnemyEndPathEvent e)
    {
        EnemyDestroyed(e.Uid);
    }

    private void EnemyDestroyed(int uid)
    {
        nowEnemies.Remove(uid);
        nowDiedEnemyAmount++;

        if (nowDiedEnemyAmount >= totalEnemyAmount)
        {
            EventHelper.WaveEndEvent.Invoke(this, GameEvent.WaveEndEvent.CreateEvent());
        }
    }
    /// <summary>
    /// 找離最近的敵人
    /// </summary>
    /// <param name="towerPos"></param>
    /// <param name="towerShootRange"></param>
    /// <returns></returns>
    public Enemy FindNearestEnemy(Vector3 towerPos,float towerShootRange)
    {
        float shortestDistance = Mathf.Infinity;
        Enemy nearestEnemy = null;

        foreach (var enemy in nowEnemies)
        {
            float distaceToEnemy = Vector3.Distance(towerPos, enemy.Value.transform.localPosition);
            if (distaceToEnemy < shortestDistance && distaceToEnemy <= towerShootRange)
            {
                shortestDistance = distaceToEnemy;
                nearestEnemy = enemy.Value;
            }
        }

        return nearestEnemy;
    }
    /// <summary>
    /// 找血量最多的敵人
    /// </summary>
    /// <param name="towerPos"></param>
    /// <param name="towerShootRange"></param>
    /// <returns></returns>
    public Enemy FindHighestHPEnemy(Vector3 towerPos, float towerShootRange)
    {
        float highestHP = 0;
        Enemy highestHPEnemy = null;

        foreach (var enemy in nowEnemies)
        {
            float distaceToEnemy = Vector3.Distance(towerPos, enemy.Value.transform.localPosition);
            if (distaceToEnemy <= towerShootRange && enemy.Value.HP > highestHP)
            {
                highestHP = enemy.Value.HP;
                highestHPEnemy = enemy.Value;
            }
        }

        return highestHPEnemy;
    }
    /// <summary>
    /// 找最弱的敵人
    /// </summary>
    /// <param name="towerPos"></param>
    /// <param name="towerShootRange"></param>
    /// <returns></returns>
    public Enemy FindWeakestEnemy(Vector3 towerPos, float towerShootRange)
    {
        float weakestHP = Mathf.Infinity;
        Enemy weakestPEnemy = null;

        foreach (var enemy in nowEnemies)
        {
            float distaceToEnemy = Vector3.Distance(towerPos, enemy.Value.transform.localPosition);
            if (distaceToEnemy <= towerShootRange && enemy.Value.HP < weakestHP)
            {
                weakestHP = enemy.Value.HP;
                weakestPEnemy = enemy.Value;
            }
        }

        return weakestPEnemy;
    }

    /// <summary>
    /// 找第一個的敵人
    /// </summary>
    /// <param name="towerPos"></param>
    /// <param name="towerShootRange"></param>
    /// <returns></returns>
    public Enemy FindFirstEnemy(Vector3 towerPos, float towerShootRange)
    {
        float firstSpawnTime = Mathf.Infinity;
        Enemy firstEnemy = null;

        foreach (var enemy in nowEnemies)
        {
            float distaceToEnemy = Vector3.Distance(towerPos, enemy.Value.transform.localPosition);
            if (distaceToEnemy <= towerShootRange && enemy.Value.SpawnTime < firstSpawnTime)
            {
                firstSpawnTime = enemy.Value.SpawnTime;
                firstEnemy = enemy.Value;
            }
        }

        return firstEnemy;
    }

    public int GetNowEnemyAmount()
    {
        return nowEnemies.Count;
    }

    private void OnEnable()
    {
        EventHelper.NextWaveStartedEvent += NextWaveStart;
        EventHelper.EnemyEndPathEvent += EnemyEndPath;
        EventHelper.EnemyDiedEvent += EnemyDied;
    }

    private void OnDisable()
    {
        EventHelper.NextWaveStartedEvent -= NextWaveStart;
        EventHelper.EnemyEndPathEvent -= EnemyEndPath;
        EventHelper.EnemyDiedEvent -= EnemyDied;
    }

    private int GenerateUid()
    {
        int uid = 1;
        while (nowEnemies.ContainsKey(uid)) uid += 1;
        return uid;
    }

    #region  物件池
    // 物件池中的物件不夠時，建立新的物件去填充物件池
    private Enemy CreateEnemy()
    {
        var enemyData = gameManager.EnemyData.GetData(nowSelectEnemyId);
        var obj = Instantiate(enemyData.EnemyPrefab);
        Enemy newEnemy = obj.GetComponent<Enemy>();
        newEnemy.transform.SetParent(transform);
        if (enemyObjectPools.TryGetValue(nowSelectEnemyId, out var pool))
        {
            newEnemy.SetEnemy(enemyData);
            newEnemy.ObjectPool = pool;
        }

        return newEnemy;
    }

    // 將物件放回物件池
    private void OnReleaseToPool(Enemy pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }

    // 從物件池中取出物件
    private void OnGetFromPool(Enemy pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }

    // 當超出物件池的上限時，將物件Destroy
    private void OnDestroyPooledObject(Enemy pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }

    #endregion
}
