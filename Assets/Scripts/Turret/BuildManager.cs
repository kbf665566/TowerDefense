using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEditor.PlayerSettings;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    private GameManager gameManager => GameManager.instance;
    private LevelManager levelManager => LevelManager.instance;

    private Dictionary<int, TowerInLevel> nowTowers = new Dictionary<int, TowerInLevel>();
    [SerializeField] private Transform towerParent;

    private GridManager gridManager;

#if UNITY_EDITOR
    private GridDebugger gridDebugger;
    public GridDebugger GridDebugger => gridDebugger;
#endif

    #region 物件池
    private int nowSelectTowerId = -1;
    private Dictionary<int, IObjectPool<TowerInLevel>> towerObjectPools;
    private int defaultCapacity = 20;
    private int maxCapacity = 100;
    private bool collectionCheck = true;
    #endregion

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("有多個BuildManager");
            return;
        }
        instance = this;

#if UNITY_EDITOR
        if(gridDebugger == null)
            gridDebugger = FindObjectOfType<GridDebugger>();
#endif

        var mapData = gameManager.NowMapData;
        gridManager = new GridManager(Vector2Short.Zero, mapData.MapSize, mapData.GetBlockGridPosList(mapData.BlockGridList), mapData.GetEnemyPathPosList(mapData.EnemyPathList));

#if UNITY_EDITOR
        gridDebugger.SetUp();
#endif
    }

    private void Start()
    {
        towerObjectPools = new Dictionary<int, IObjectPool<TowerInLevel>>();
        var towerList = gameManager.TowerData.TowerList;
        for (int i = 0;i< towerList.Count;i++)
        {
            towerObjectPools.Add(towerList[i].Id, new ObjectPool<TowerInLevel>(CreateTower,
            OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
            collectionCheck, defaultCapacity, maxCapacity));
        }
    }


    public GridState GetGridState(Vector2Short gridPos)
    {
        var gridState = gridManager.GetGridState(gridPos.x,gridPos.y);
        return gridState;
    }
    public GridState GetGridState(int x,int y)
    {
        if (gridManager.IsExistGrid(new Vector2Short(x, y)) == false)
            return GridState.Block;

        var gridState = gridManager.GetGridState(x, y);
        return gridState;
    }

    public void BuildTower(object s,GameEvent.TowerBuildEvent e)
    {
        nowSelectTowerId = e.Id;
        var towerData = gameManager.TowerData.GetData(e.Id);
        var uid = GenerateUid();
        if(towerData != null)
        {
            TowerInLevel newTower = null;
            var worldPos = e.GridPos.ToWorldPos() + new Vector3(0, .5f, 0);

            if (towerObjectPools.TryGetValue(e.Id, out var pool))
            {
                newTower = pool.Get();

                newTower.transform.SetPositionAndRotation(worldPos, Quaternion.identity);
                newTower.transform.SetParent(towerParent);

                if (towerData.towerType == TowerType.Normal)
                    ((NormalTower)newTower).SetTower(uid, towerData, e.GridPos);
                else if (towerData.towerType == TowerType.AOE)
                    ((AOETower)newTower).SetTower(uid, towerData, e.GridPos);
                else if (towerData.towerType == TowerType.Support)
                    ((SupportTower)newTower).SetTower(uid, towerData, e.GridPos);
                else if (towerData.towerType == TowerType.Money)
                    ((MoneyTower)newTower).SetTower(uid, towerData, e.GridPos);

                gridManager.PlaceTower(uid, e.GridPos, towerData.TowerSize);
                nowTowers.Add(uid, newTower);

                levelManager.CostMoney(towerData.TowerLevelData[0].BuildUpgradeCost);

                //發出建造特效事件
                EventHelper.EffectShowEvent.Invoke(this, GameEvent.GameEffectShowEvent.CreateEvent(worldPos, towerData.BuildParticle));

#if UNITY_EDITOR
                gridDebugger.ChangeColor(e.GridPos, towerData.TowerSize, GridState.Building);
#endif
            }
        }
    }

    public void SellTower(object s, GameEvent.TowerSellEvent e)
    {
        if (nowTowers.TryGetValue(e.Uid, out var tower))
        {
            EventHelper.EffectShowEvent.Invoke(this, GameEvent.GameEffectShowEvent.CreateEvent(tower.GridPos.CalculateCenterPos(tower.TowerData.TowerSize), tower.TowerData.RemoveParticle));
            levelManager.AddMoney(tower.GetNowLevelData().SoldPrice);

            gridManager.RemoveTower(tower.GridPos,tower.TowerData.TowerSize);

            nowTowers.Remove(e.Uid);

            if (towerObjectPools.TryGetValue(tower.Id, out var pool))
                pool.Release(tower);
        }
    }

    public void UpgradeTower(object s, GameEvent.TowerUpgradeEvent e)
    {
        if(nowTowers.TryGetValue(e.Uid,out var tower))
        {
            tower.LevelUp();
            levelManager.CostMoney(tower.GetNowLevelData().BuildUpgradeCost);
        }
    }

    public bool CheckCanBuild(Vector2Short size,Vector2Short pos)
    {
        return gridManager.CheckCanBuild(pos,size);
    }

    public int GetTowerUid(Vector2Short pos)
    {
        return gridManager.GetGridTowerUid(pos);
    }

    public TowerInLevel GetTower(int uid)
    {
        if (nowTowers.TryGetValue(uid, out var tower))
            return tower;

        return null;
    }

    public void DeselectNode()
    {

    }

    private void OnDisable()
    {
        EventHelper.TowerBuiltEvent -= BuildTower;
        EventHelper.TowerSoldEvent -= SellTower;
        EventHelper.TowerUpgradedEvent -= UpgradeTower;
    }

    private void OnEnable()
    {
        EventHelper.TowerBuiltEvent += BuildTower;
        EventHelper.TowerSoldEvent += SellTower;
        EventHelper.TowerUpgradedEvent += UpgradeTower;
    }

    private int GenerateUid()
    {
        int uid = 1;
        while (nowTowers.ContainsKey(uid)) uid += 1;
        return uid;
    }

    #region  物件池
    // 物件池中的物件不夠時，建立新的物件去填充物件池
    private TowerInLevel CreateTower()
    {
        var towerData = gameManager.TowerData.GetData(nowSelectTowerId);
        TowerInLevel newTower = Instantiate(towerData.TowerPrefab);
       
        if (towerObjectPools.TryGetValue(nowSelectTowerId, out var pool))
            newTower.ObjectPool = pool;

        return newTower;
    }

    // 將物件放回物件池
    private void OnReleaseToPool(TowerInLevel pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }

    // 從物件池中取出物件
    private void OnGetFromPool(TowerInLevel pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }

    // 當超出物件池的上限時，將物件Destroy
    private void OnDestroyPooledObject(TowerInLevel pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }

    #endregion
}
