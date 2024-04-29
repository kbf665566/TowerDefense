using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager
{
    private Vector2Short startGridPos;
    public Vector2Short StartGridPos => startGridPos;
    private Vector2Short endGridPos;
    public Vector2Short GridSize { get; private set; }
    private HashSet<GridData> emptyGridList;

    /// <summary> �x�s���� </summary>
    private GridData[,] gridMap;
    private Vector2Short tempGridPos;

    public GridManager(Vector2Short startGridPos, Vector2Short mapSize)
    {
        this.startGridPos = startGridPos;
        GridMapInitialize(mapSize);
        endGridPos = startGridPos + mapSize;

        tempGridPos = new Vector2Short();
    }


    private void GridMapInitialize(Vector2Short gridSize)
    {
        GridSize = gridSize;
        gridMap = new GridData[GridSize.x, GridSize.y];
        emptyGridList = new HashSet<GridData>();
        for (short x = 0; x < GridSize.x; x++)
        {
            for (short y = 0; y < GridSize.y; y++)
            {
                var data = new GridData();
                data.GridPos = new Vector2Short(x, y) + StartGridPos;
                data.GridState = GridState.Empty;
                gridMap[x, y] = data;
                emptyGridList.Add(data);
            }
        }
    }

    public void Dispose()
    {
        gridMap = null;
        emptyGridList = null;
    }

    public void ResetGrid()
    {
        //���m �����T
        for (short x = 0; x < GridSize.x; x++)
        {
            for (short y = 0; y < GridSize.y; y++)
            {
                if (gridMap[x, y].GridState == GridState.Block)
                    continue;
                gridMap[x, y].Clear();
            }
        }
    }

    /// <summary>
    /// �]�w���檬�A
    /// </summary>
    /// <param name="gridPos"></param>
    /// <param name="gridState"></param>
    public void SetGridState(Vector2Short gridPos, GridState gridState)
    {
        // ������
        var gridData = GetGridData(gridPos);
        // �]�w���檬�A
        gridData.GridState = gridState;
    }

    /// <summary>
    /// ���o������
    /// </summary>
    /// <param name="gridPos"></param>
    /// <returns></returns>
    private GridData GetGridData(Vector2Short gridPos)
    {
        gridPos -= startGridPos;
        return gridMap[gridPos.x, gridPos.y];
    }

    public long GetGridTowerUid(Vector2Short gridPos)
    {
        var gridData = GetGridData(gridPos);
        return gridData != null ? gridData.TowerUid : 0;
    }

    /// <summary>
    /// �O�_�i�ؿv
    /// </summary>
    /// <param name="pos">�ˬd��m</param>
    /// <param name="unitSize">�ˬd�d��</param>
    /// <returns></returns>
    public bool CheckCanBuild(Vector2Short pos, Vector2Short unitSize)
    {
        GridData gridData;
        for (short x = pos.x; x < pos.x + unitSize.x; x++)
        {
            for (short y = pos.y; y < pos.y + unitSize.y; y++)
            {
                tempGridPos.SetPos(x, y);
                gridData = GetGridData(tempGridPos);
                if (gridData == null)
                    return false;

                //�ˬd�O�_����ê���Ψ�L��
                if (gridData.CanBuild == false)
                    return false;
            }
        }
        return true;
    }


    /// <summary>
    /// ��m��
    /// </summary>
    /// <param name="towerUid"></param>
    /// <param name="towerPos"></param>
    /// <param name="towerSize"></param>
    /// <returns></returns>
    public bool PlaceTower(int towerUid,Vector2Short towerPos,Vector2Short towerSize)
    {
        if (!CheckCanBuild(towerPos, towerSize))
            return false;
        UpdateGridInfo(towerUid, towerPos, towerSize, GridState.Building);
        return true;
    }

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="anchorGridPos"></param>
    /// <param name="unitSize"></param>
    public void RemoveTower(Vector2Short anchorGridPos, Vector2Short unitSize)
    {
        RemoveGridInfo(anchorGridPos, unitSize);
    }

    /// <summary>
    /// �]�w��ê�a��
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="unitSize"></param>
    public void SetBlockGrid(Vector2Short pos, Vector2Short unitSize)
    {
        UpdateGridInfo(0, pos, unitSize, GridState.Block);
    }


    /// <summary>
    /// ��s����
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="anchorGridPos"></param>
    /// <param name="size"></param>
    /// <param name="gridState"></param>
    private void UpdateGridInfo(int uid, Vector2Short anchorGridPos, Vector2Short size, GridState gridState)
    {
        for (short x = 0; x < size.x; x++)
        {
            for (short y = 0; y < size.y; y++)
            {
                tempGridPos.SetPos(x, y);
                tempGridPos = tempGridPos + anchorGridPos;
                var grid = GetGridData(tempGridPos);
                if (grid == null)
                    continue;

                if (grid.GridState == GridState.Block) continue;

                if (grid.CanBuild == false) continue;

                grid.TowerUid = uid;
                grid.GridState = gridState;
                UpdateEmptyList(grid);
                continue;

            }
        }
    }

    private void RemoveGridInfo(Vector2Short anchorGridPos, Vector2Short size)
    {
        for (short x = -1; x < size.x + 1; x++)
        {
            for (short y = -1; y < size.y + 1; y++)
            {
                tempGridPos.SetPos(x, y);
                tempGridPos = tempGridPos + anchorGridPos;
                var grid = GetGridData(tempGridPos);
                if (grid == null)
                    continue;

                if (grid.GridState == GridState.Block) continue;

                if (x < 0 || y < 0 || x == size.x || y == size.y)
                {
                    if (grid.TowerUid == 0)
                    {
                        grid.GridState = GridState.Empty;
                        UpdateEmptyList(grid);
                    }
                    continue;
                }


                grid.TowerUid = 0;
                grid.GridState = GridState.Empty;
                UpdateEmptyList(grid);
            }
        }
    }


    private void UpdateEmptyList(GridData grid)
    {
        if (grid.GridState == GridState.Empty)
        {
            if (emptyGridList.Contains(grid) == false)
                emptyGridList.Add(grid);
        }
        else
        {
            if (emptyGridList.Contains(grid) == true)
                emptyGridList.Remove(grid);
        }
    }

    public bool IsEmptyGrid(Vector2Short gridPos)
    {
        var grid = GetGridData(gridPos);
        if (grid == null)
            return false;
        
        return emptyGridList.Contains(grid);
    }
}
