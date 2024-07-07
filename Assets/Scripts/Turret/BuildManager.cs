using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    private GameManager gameManager => GameManager.instance;
    private LevelManager levelManager => LevelManager.instance;

    private Dictionary<int, TowerInLevel> nowTowers = new Dictionary<int, TowerInLevel>();
    [SerializeField] private Transform towerParent;

    private int selectTowerId;

    private bool nowPreviewBuild = false;
    public bool NowPreviewBuild => nowPreviewBuild;
    private float tempShootRange;
    private Vector2Short tempSize;

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

    #region 畫塔的範圍
    [SerializeField] private LineRenderer LineDrawer;
    [Range(6, 60)] 
    float ThetaScale = 0.01f;//改變形狀
    private int Size;
    private float Theta = 0f;
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

        LineDrawer.loop = true;

        LineDrawer.startColor = Color.red;
        LineDrawer.endColor = Color.red;
        LineDrawer.startWidth = 0.1f;
        LineDrawer.endWidth = 0.1f;
    }

    private void Update()
    {
        if(nowPreviewBuild && Input.GetMouseButton(1))
        {
            CancelBuild();
        }
    }

    public void PreviewBuildTower(object s, GameEvent.TowerPreviewBuildEvent e)
    {
        nowPreviewBuild = true;
        if (e.ShootRange != 0)
        {
            selectTowerId = e.TowerId;
            tempShootRange = e.ShootRange;
            tempSize = e.Size;
            DrawTowerRange(GridExtension.GetCenterGrid(e.GridPos, e.Size).ToWorldPos(), e.ShootRange);
        }
    }

    public void MovePreviewBuild(object s, GameEvent.MovePreviewBuildEvent e)
    {
        DrawTowerRange(GridExtension.GetCenterGrid(e.GridPos, tempSize).ToWorldPos(), tempShootRange);
    }

    public void SelectTower(object s, GameEvent.TowerSelectEvent e)
    {
        var tower = GetTower(e.Uid);
        if (tower.TowerType != TowerType.Money)
        {
            var range = ((ITowerRange)tower).GetShootRange();
            DrawTowerRange(tower.transform.position, range);
#if UNITY_EDITOR
            EditorGUIUtility.PingObject(tower);
            Selection.activeGameObject = tower.gameObject;
#endif
        }
    }

    public void CancelSelectTower(object s, GameEvent.NodeCancelSelectEvent e)
    {
        StopDrawTowerRange();
    }

    public void SelectNode(object s, GameEvent.NodeSelectEvent e)
    {
#if UNITY_EDITOR
        var gridState = GetGridState(e.GridPos);
        Debug.Log("gridpos:" + e.GridPos + " state:" + gridState);
#endif

        if (!nowPreviewBuild)
            return;

        if (CheckCanBuild(tempSize, e.GridPos))
            EventHelper.TowerBuiltEvent.Invoke(this, GameEvent.TowerBuildEvent.CreateEvent(selectTowerId, e.GridPos));
        else
            CancelBuild();
    }

    public void BuildTower(object s,GameEvent.TowerBuildEvent e)
    {
        LineDrawer.enabled = false;
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

                if (towerData.towerType == TowerType.Normal)
                {
                    if (towerData.IsUseBullet)
                        ((NormalTower)newTower).SetTower(uid, towerData, e.GridPos);
                    else
                        ((NoBulletTower)newTower).SetTower(uid, towerData, e.GridPos);
                }
                else if (towerData.towerType == TowerType.AOE)
                    ((AOETower)newTower).SetTower(uid, towerData, e.GridPos);
                else if (towerData.towerType == TowerType.Support)
                    ((SupportTower)newTower).SetTower(uid, towerData, e.GridPos);
                else if (towerData.towerType == TowerType.Money)
                    ((MoneyTower)newTower).SetTower(uid, towerData, e.GridPos);

                gridManager.PlaceTower(uid, e.GridPos, towerData.TowerSize);
                nowTowers.Add(uid, newTower);

                levelManager.CostMoney(towerData.TowerLevelData[0].BuildUpgradeCost);


                if (newTower.TowerType == TowerType.Support)
                    ((SupportTower)newTower).SupportOtherTower();

                //發出建造特效事件
                EventHelper.EffectShowEvent.Invoke(this, GameEvent.GameEffectShowEvent.CreateEvent(worldPos, towerData.BuildParticle));
            }
        }

        selectTowerId = -1;
        nowPreviewBuild = false;
        StopDrawTowerRange();
    }

    public void CancelBuild()
    {
        selectTowerId = -1;
        nowPreviewBuild = false;
        StopDrawTowerRange();
        EventHelper.TowerCanceledPreviewEvent.Invoke(this, GameEvent.TowerCancelPreviewEvent.CreateEvent());
    }

    public void SellTower(object s, GameEvent.TowerSellEvent e)
    {
        LineDrawer.enabled = false;
        if (nowTowers.TryGetValue(e.Uid, out var tower))
        {
            EventHelper.EffectShowEvent.Invoke(this, GameEvent.GameEffectShowEvent.CreateEvent(tower.GridPos.CalculateCenterPos(tower.TowerData.TowerSize), tower.TowerData.RemoveParticle));
            levelManager.AddMoney(tower.GetNowLevelData().SoldPrice);


            gridManager.RemoveTower(tower.GridPos, tower.TowerData.TowerSize);
            nowTowers.Remove(e.Uid);

            if (tower.TowerType == TowerType.Support)
            {
                ((SupportTower)tower).SellToResetSupport();
                foreach(var obj in nowTowers)
                {
                    if(obj.Value.TowerType == TowerType.Support)
                    {
                        ((SupportTower)obj.Value).SupportOtherTower();
                    }
                }
            }

            if (towerObjectPools.TryGetValue(tower.Id, out var pool))
                pool.Release(tower);
        }
    }

    public void UpgradeTower(object s, GameEvent.TowerUpgradeEvent e)
    {
        LineDrawer.enabled = false;
        if (nowTowers.TryGetValue(e.Uid,out var tower))
        {
            tower.LevelUp();
            levelManager.CostMoney(tower.GetNowLevelData().BuildUpgradeCost);
        }
    }

    public void ChangeTowerAttackMode(object s, GameEvent.TowerChangeAttackModeEvent e)
    {
        if (nowTowers.TryGetValue(e.Uid, out var tower))
        {
            if (tower.TowerType == TowerType.Normal || tower.TowerType == TowerType.AOE)
                ((IAttackTower)tower).ChangeAttackMode(e.AttackMode);
        }
    }

    #region Check/Get
    public bool CheckCanBuild(Vector2Short size,Vector2Short pos)
    {
        return gridManager.CheckCanBuild(pos,size);
    }

    public int GetTowerUid(Vector2Short pos)
    {
        return gridManager.GetGridTowerUid(pos);
    }

    public GridState GetGridState(Vector2Short gridPos)
    {
        var gridState = gridManager.GetGridState(gridPos.x, gridPos.y);
        return gridState;
    }
    public GridState GetGridState(int x, int y)
    {
        if (gridManager.IsExistGrid(new Vector2Short(x, y)) == false)
            return GridState.Block;

        var gridState = gridManager.GetGridState(x, y);
        return gridState;
    }

    public TowerInLevel GetTower(int uid)
    {
        if (nowTowers.TryGetValue(uid, out var tower))
            return tower;

        return null;
    }
    #endregion

    private void DrawTowerRange(Vector3 towerPos,float towerShootRange)
    {
        LineDrawer.enabled = true;
        Theta = 0f;
        Size = (int)((1f / ThetaScale) + 1f);
        LineDrawer.positionCount = Size;

        for (int i = 0; i < Size; i++)
        {
            Theta += (2.0f * Mathf.PI * ThetaScale);
            float x = towerShootRange * Mathf.Cos(Theta);
            float y = towerShootRange * Mathf.Sin(Theta);
            LineDrawer.SetPosition(i, new Vector3(towerPos.x + x, towerPos.y + 0.5f, towerPos.z + y));
        }
    }

    private void StopDrawTowerRange()
    {
        LineDrawer.enabled = false;
    }

    private void OnDisable()
    {
        EventHelper.NodeSelectedEvent -= SelectNode;
        EventHelper.TowerBuiltEvent -= BuildTower;
        EventHelper.TowerSoldEvent -= SellTower;
        EventHelper.TowerUpgradedEvent -= UpgradeTower;
        EventHelper.TowerSelectedEvent -= SelectTower;
        EventHelper.TowerPreviewBuiltEvent -= PreviewBuildTower;
        EventHelper.MovePreviewBuiltEvent -= MovePreviewBuild;
        EventHelper.NodeCancelSelectedEvent -= CancelSelectTower;
        EventHelper.TowerChangedAttackModeEvent -= ChangeTowerAttackMode;
    }

    private void OnEnable()
    {
        EventHelper.NodeSelectedEvent += SelectNode;
        EventHelper.TowerBuiltEvent += BuildTower;
        EventHelper.TowerSoldEvent += SellTower;
        EventHelper.TowerUpgradedEvent += UpgradeTower;
        EventHelper.TowerSelectedEvent += SelectTower;
        EventHelper.TowerPreviewBuiltEvent += PreviewBuildTower;
        EventHelper.MovePreviewBuiltEvent += MovePreviewBuild;
        EventHelper.NodeCancelSelectedEvent += CancelSelectTower;
        EventHelper.TowerChangedAttackModeEvent += ChangeTowerAttackMode;
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
        newTower.transform.SetParent(towerParent);
       
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
