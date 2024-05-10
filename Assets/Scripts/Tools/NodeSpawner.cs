using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Color = UnityEngine.Color;

/// <summary>
/// 生成地圖
/// </summary>
public class NodeSpawner : MonoBehaviour
{
    #region Node
    [SerializeField] private Transform nodeObj;
    private List<Transform> nodes = new List<Transform>();
    [SerializeField] private Vector3 nodeSize = Vector3.one;
    #endregion

    #region path
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;
    private List<Transform> startObjList = new List<Transform>();
    private List<Transform> endObjList = new List<Transform>();

    [SerializeField] private Transform wayPointObj;
    private List<Transform> wayPoints = new List<Transform>();

    [SerializeField] private Transform pathObj;
    private List<Transform> pathObjList = new List<Transform>();
    //[FormerlySerializedAs("path")]
    private EnemyPathData[] enemyPathArray;
    private List<Vector2Short> blockGridList = new List<Vector2Short>();
    private List<Vector2Short> enemyPathNodeList = new List<Vector2Short>();
    #endregion


    [SerializeField] private Transform nodeParent;
    [SerializeField] private Transform pathParent;
    [SerializeField] private Transform wayPointParent;
    [SerializeField] private Vector2Short mapSize;
    [SerializeField] private bool showGrid = true;


    public void SetData(MapData mapData)
    {
        DeleteNode();

        mapSize = mapData.MapSize;

        enemyPathArray = new EnemyPathData[mapData.EnemyPathList.Count];
        for (int i = 0; i < enemyPathArray.Length; i++)
        {
            enemyPathArray[i] = new EnemyPathData();
            enemyPathArray[i].Path = new List<EnemyPath>();
        }

        //敵人路徑
        for (int i =0;i<mapData.EnemyPathList.Count;i++)
        {
            for (int j = 0; j < mapData.EnemyPathList[i].Path.Count; j++)
            {
                enemyPathArray[i].Path.Add(mapData.EnemyPathList[i].Path[j]);
            }
        }

        //障礙物
        blockGridList = mapData.GetBlockGridPosList(mapData.BlockGridList);

        SetPath();
        SpawNode();
    }

    List<Vector2Short> tempList = new List<Vector2Short>();
    private void SpawNode()
    {
        nodeObj.localScale = nodeSize;
        if (mapSize.x > 0 && mapSize.y > 0)
        {
            tempList.Clear();
            foreach (var g in enemyPathNodeList)
                tempList.Add(g);
            foreach (var g in blockGridList)
                tempList.Add(g);

            for (int i = 0; i < mapSize.x; i++)
            {
                for (int j = 0; j < mapSize.y; j++)
                {
                    bool canSpawn = true;
                    for (int k = 0; k < tempList.Count; k++)
                    {
                        if (tempList[k].Equals(new Vector2Short(i, j)))
                        {
                            canSpawn = false;
                            tempList.RemoveAt(k);
                            break;
                        }
                    }
                    if (canSpawn)
                    {
                        var tempPos = new Vector3(nodeParent.position.x + i, 0, nodeParent.position.y + j);
                        var tempObj = Instantiate(nodeObj);
                        tempObj.name = "Node("+i+","+j+")";
                        tempObj.SetParent(nodeParent);
                        tempObj.transform.position = tempPos;
                        nodes.Add(tempObj);
                    }
                }
            }

            start.localScale = nodeSize;
            end.localScale = nodeSize;
            for (int i =0;i<enemyPathArray.Length;i++)
            {
                
                var startPos = new Vector3(pathParent.position.x + enemyPathArray[i].Path[0].GridPos.x, nodeSize.y, pathParent.position.y + +enemyPathArray[i].Path[0].GridPos.y);
                var tempstartObj = Instantiate(start);
                tempstartObj.SetParent(pathParent);
                tempstartObj.transform.position = startPos;
                startObjList.Add(tempstartObj);

                
                var endPos = new Vector3(pathParent.position.x + enemyPathArray[i].Path[enemyPathArray[i].Path.Count - 1].GridPos.x, nodeSize.y, pathParent.position.y + enemyPathArray[i].Path[enemyPathArray[i].Path.Count - 1].GridPos.y);
                var tempEndObj = Instantiate(end);
                tempEndObj.SetParent(pathParent);
                tempEndObj.transform.position = endPos;
                endObjList.Add(tempEndObj);
            }
        }
    }


    private void DeleteNode()
    {
        nodes.Clear();
        pathObjList.Clear();
        wayPoints.Clear();
        startObjList.Clear();
        endObjList.Clear();
        blockGridList.Clear();

        for (int i = pathParent.childCount - 1; i >= 0; i--)
            DestroyImmediate(pathParent.GetChild(i).gameObject);
        for (int i = nodeParent.childCount - 1; i >= 0; i--)
            DestroyImmediate(nodeParent.GetChild(i).gameObject);
        for (int i = wayPointParent.childCount - 1; i >= 0; i--)
            DestroyImmediate(wayPointParent.GetChild(i).gameObject);
    }

    private void SetPath()
    {
        for(int i =0;i< enemyPathArray.Length;i++)
        {
            for (int j = 0; j < enemyPathArray[i].Path.Count - 1; j++)
            {
                AddDontSpawnNodes(enemyPathArray[i].Path[j].GridPos, enemyPathArray[i].Path[j + 1].GridPos);
            }
        }

        for (int i = 0; i < enemyPathArray.Length; i++)
        {
            for (int j = 0; j < enemyPathArray[i].Path.Count; j++)
            {
                var tempPos = new Vector3(nodeParent.position.x + enemyPathArray[i].Path[j].GridPos.x, 1.5f, nodeParent.position.z + enemyPathArray[i].Path[j].GridPos.y);
                var tempObj = Instantiate(wayPointObj);
                tempObj.name = "WayPoint " + i + "";
                tempObj.SetParent(wayPointParent);
                tempObj.transform.position = tempPos;
                wayPoints.Add(tempObj);
            }
        }
    }
    
    private void AddDontSpawnNodes(Vector2Short node1, Vector2Short node2)
    {
        if (node1.x < node2.x)
        {
            for (int i = node1.x; i <= node2.x; i++)
            {
                if (node1.y < node2.y)
                {
                    for (int j = node1.y; j <= node2.y; j++)
                    {
                        SpawnEnemyPath(new Vector2Short(i, j));
                    }
                }
                else
                {
                    for (int j = node1.y; j >= node2.y; j--)
                    {
                        SpawnEnemyPath(new Vector2Short(i, j));
                    }
                }
            }
        }
        else
        {
            for (int i = node1.x; i >= node2.x; i--)
            {
                if (node1.y < node2.y)
                {
                    for (int j = node1.y; j <= node2.y; j++)
                    {
                        SpawnEnemyPath(new Vector2Short(i, j));
                    }
                }
                else
                {
                    for (int j = node1.y; j >= node2.y; j--)
                    {
                        SpawnEnemyPath(new Vector2Short(i, j));
                    }
                }
            }
        }
    }

    private void SpawnEnemyPath(Vector2Short node)
    {
        if (!enemyPathNodeList.Contains(node))
        {
            enemyPathNodeList.Add(node);

            var tempObj = Instantiate(pathObj);
            tempObj.localScale = nodeSize;
            tempObj.SetParent(pathParent);
            tempObj.transform.position = new Vector3(node.x, 0, node.y);
            pathObjList.Add(tempObj);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        return;
        if (showGrid)
        {
            Vector3 origin = new Vector3(transform.position.x - nodeSize.x / 2, 3f, transform.position.z - nodeSize.z / 2);
            DebugDrawGrid(origin, mapSize.x, mapSize.y, Color.blue);
            DrawEnemyPath();
        }
    }

    private void DebugDrawGrid(Vector3 origin, int numRows, int numCols, Color color)
    {//在 Scene 裡面畫出對應的格線
        float width = numCols;
        float height = numRows;

        // 水平
        for (int i = 0; i < numRows + 1; i++)
        {
            Vector3 startPos = origin + i * Vector3.forward;
            Vector3 endPos = startPos + width * Vector3.right;
            Debug.DrawLine(startPos, endPos, color);
        }

        // 垂直
        for (int i = 0; i < numCols + 1; i++)
        {
            Vector3 startPos = origin + i *  Vector3.right;
            Vector3 endPos = startPos + height * Vector3.forward;
            Debug.DrawLine(startPos, endPos, color);
        }
    }

    private void DrawEnemyPath()
    {
        for (int i = 0; i < enemyPathArray.Length; i++)
        {
            for (int j = 0; j < enemyPathArray[i].Path.Count - 1; j++)
            {
                Vector3 startPos = new Vector3(nodeParent.position.x + enemyPathArray[i].Path[j].GridPos.x, 3f, nodeParent.position.z + enemyPathArray[i].Path[j].GridPos.y);
                Vector3 endPos = new Vector3(nodeParent.position.x + enemyPathArray[i].Path[j + 1].GridPos.x, 3f, nodeParent.position.z + enemyPathArray[i].Path[j + 1].GridPos.y);
                Debug.DrawLine(startPos, endPos, Color.red);
            }
        }
    }
#endif
}
