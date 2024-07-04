using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NodeStateDisplayer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer nodeStatePrefab;
    [SerializeField] private int maxAmount = 16;
    [SerializeField] private SpriteRenderer[] stateDisplayers;
    private BuildManager buildManager => BuildManager.instance;

    private Color RedColor = new Color(1,0,0,0.6f);
    private Color WhiteColor = new Color(1,1,1,0.6f);
    private Color GreenColor = new Color(0, 1, 0, 0.6f);

    private Vector2Short tempSize;
    public void SelectNode(object s, GameEvent.NodeSelectEvent e)
    {
        stateDisplayers[0].color = GreenColor;

        stateDisplayers[0].transform.localPosition = new Vector3(e.GridPos.x, 0.7f, e.GridPos.y);
        stateDisplayers[0].gameObject.SetActive(true);
    }

    public void StartPreviewBuildTower(object s,GameEvent.TowerPreviewBuildEvent e)
    {
        int index = 0;
        tempSize = e.Size;
        for (int x = e.GridPos.x; x < e.GridPos.x + e.Size.x; x++)
        {
            for (int y = e.GridPos.y; y < e.GridPos.y + e.Size.y; y++)
            {
                stateDisplayers[index].color = GreenColor;
                stateDisplayers[index].transform.localPosition = new Vector3(x, 0.7f, y);
                stateDisplayers[index].gameObject.SetActive(true);
                index++;
            }
        }
    }
    public void MovePreviewBuildTower(object s, GameEvent.MovePreviewBuildEvent e)
    {
        var index = 0;
        for (int x = e.GridPos.x; x < e.GridPos.x + tempSize.x; x++)
        {
            for (int y = e.GridPos.y; y < e.GridPos.y + tempSize.y; y++)
            {
                var gridState = buildManager.GetGridState(x, y);
                if (gridState == GridState.Building || gridState == GridState.EnemyPath || gridState == GridState.Block)
                    stateDisplayers[index].color = RedColor;
                else if (gridState == GridState.Empty)
                    stateDisplayers[index].color = GreenColor;

                stateDisplayers[index].transform.localPosition = new Vector3(x, 0.7f, y);
                index++;
            }
        }
    }

    public void SelectTower(object s, GameEvent.TowerSelectEvent e)
    {
        var tower = buildManager.GetTower(e.Uid);
        if (tower == null)
            return;

        var size = tower.TowerData.TowerSize;
        var gridPos = tower.GridPos;

        int index = 0;
        for (int x = gridPos.x; x < gridPos.x + size.x; x++)
        {
            for (int y = gridPos.y; y < gridPos.y + size.y; y++)
            {
                stateDisplayers[index].color = GreenColor;
                stateDisplayers[index].transform.localPosition = new Vector3(x, 0.7f, y);
                stateDisplayers[index].gameObject.SetActive(true);
                index++;
            }
        }
    }

    public void HideState()
    {
        for (int i = 0; i < stateDisplayers.Length; i++)
        {
            stateDisplayers[i].gameObject.SetActive(false);
        }
    }

    public void CancelSelectNode(object s,GameEvent.NodeCancelSelectEvent e)
    {
        HideState();
    }

    public void TowerBuilt(object s, GameEvent.TowerBuildEvent e)
    {
        HideState();
    }

    public void TowerUpgraded(object s, GameEvent.TowerUpgradeEvent e)
    {
        HideState();
    }

    public void TowerSold(object s, GameEvent.TowerSellEvent e)
    {
        HideState();
    }

    public void TowerCanceledPreview(object s, GameEvent.TowerCancelPreviewEvent e)
    {
        HideState();
    }

    [ContextMenu("Spawn")]
    private void SpawnPrefab()
    {
        stateDisplayers = new SpriteRenderer[maxAmount];

        for (int i = 0; i < stateDisplayers.Length; i++)
        {
            SpriteRenderer obj = (SpriteRenderer)PrefabUtility.InstantiatePrefab(nodeStatePrefab);
            obj.transform.SetParent(transform, false);
            obj.gameObject.name = obj.name + " " + (i + 1);
            obj.gameObject.SetActive(false);
            stateDisplayers[i] = obj;
        }
    }

    private void OnEnable()
    {
        EventHelper.TowerPreviewBuiltEvent += StartPreviewBuildTower;
        EventHelper.MovePreviewBuiltEvent += MovePreviewBuildTower;
        EventHelper.NodeSelectedEvent += SelectNode;
        EventHelper.NodeCancelSelectedEvent += CancelSelectNode;
        EventHelper.TowerBuiltEvent += TowerBuilt;
        EventHelper.TowerUpgradedEvent += TowerUpgraded;
        EventHelper.TowerSoldEvent += TowerSold;
        EventHelper.TowerSelectedEvent += SelectTower;
        EventHelper.TowerCanceledPreviewEvent += TowerCanceledPreview;
    }

    private void OnDisable()
    {
        EventHelper.TowerPreviewBuiltEvent -= StartPreviewBuildTower;
        EventHelper.MovePreviewBuiltEvent -= MovePreviewBuildTower;
        EventHelper.NodeSelectedEvent -= SelectNode;
        EventHelper.NodeCancelSelectedEvent -= CancelSelectNode;
        EventHelper.TowerBuiltEvent -= TowerBuilt;
        EventHelper.TowerUpgradedEvent -= TowerUpgraded;
        EventHelper.TowerSoldEvent -= TowerSold;
        EventHelper.TowerSelectedEvent -= SelectTower;
        EventHelper.TowerCanceledPreviewEvent -= TowerCanceledPreview;
    }
}
