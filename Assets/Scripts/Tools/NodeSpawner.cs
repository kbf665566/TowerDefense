using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 生成地圖
/// </summary>
public class NodeSpawner : MonoBehaviour
{
    #region Node
    [SerializeField] private Transform node;
    private List<Transform> nodes = new List<Transform>();
    [SerializeField] private Vector3 nodeSize = Vector3.one;
    [SerializeField] Vector2 nodeOffset = Vector2.zero;
    #endregion

    #region path
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;
    private Transform startObj;
    private Transform endObj;

    [SerializeField] private Transform wayPoint;
    private List<Transform> wayPoints = new List<Transform>();

    [SerializeField] private Transform path;
    private List<Transform> pathList = new List<Transform>();
    //[FormerlySerializedAs("path")]
    [SerializeField] private List<GridPos> enemyPath = new List<GridPos>();
    private List<GridPos> dontSpawNodeList = new List<GridPos>();
    #endregion


    [SerializeField] private Transform nodeParent;
    [SerializeField] private Transform pathParent;
    [SerializeField] private Transform wayPointParent;
    [SerializeField] private GridPos mapSize;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    [ContextMenu("Generate Nodes")]
    public void SpawNode()
    {
        node.localScale = nodeSize;
        if (mapSize.x > 0 && mapSize.y > 0)
        {
            DeleteNode();
            SetPath();

            List<GridPos> tempList = new List<GridPos>();
            foreach (var g in dontSpawNodeList)
                tempList.Add(g);

            for (int i = 0; i < mapSize.x; i++)
            {
                for (int j = 0; j < mapSize.y; j++)
                {
                    bool canSpawn = true;
                    for (int k = 0; k < tempList.Count; k++)
                    {
                        if (tempList[k].Equal(i, j))
                        {
                            canSpawn = false;
                            tempList.RemoveAt(k);
                            break;
                        }
                    }
                    if (canSpawn)
                    {
                        var tempPos = new Vector3(nodeParent.position.x + i * nodeOffset.x, 0, nodeParent.position.y + j * nodeOffset.y);
                        var tempObj = Instantiate(node);
                        tempObj.name = "Node("+i+","+j+")";
                        tempObj.SetParent(nodeParent);
                        tempObj.transform.position = tempPos;
                        nodes.Add(tempObj);
                    }
                }
            }

            start.localScale = nodeSize;
            var startPos = new Vector3(pathParent.position.x + enemyPath[0].x * nodeOffset.x, nodeSize.y, pathParent.position.y + +enemyPath[0].y * nodeOffset.y);
            var tempstartObj = Instantiate(start);
            tempstartObj.SetParent(pathParent);
            tempstartObj.transform.position = startPos;
            nodes.Add(tempstartObj);

            end.localScale = nodeSize;
            var endPos = new Vector3(pathParent.position.x + enemyPath[enemyPath.Count - 1].x * nodeOffset.x, nodeSize.y, pathParent.position.y + +enemyPath[enemyPath.Count - 1].y * nodeOffset.y);
            var tempEndObj = Instantiate(end);
            tempEndObj.SetParent(pathParent);
            tempEndObj.transform.position = endPos;
            nodes.Add(tempEndObj);
        }
    }
    [ContextMenu("Delete Nodes")]
    public void DeleteNode()
    {
        nodes.Clear();
        pathList.Clear();
        wayPoints.Clear();


        for (int i = pathParent.childCount - 1; i >= 0; i--)
            DestroyImmediate(pathParent.GetChild(i).gameObject);
        for (int i = nodeParent.childCount - 1; i >= 0; i--)
            DestroyImmediate(nodeParent.GetChild(i).gameObject);
        for (int i = wayPointParent.childCount - 1; i >= 0; i--)
            DestroyImmediate(wayPointParent.GetChild(i).gameObject);
    }

    public void SetPath()
    {
        dontSpawNodeList.Clear();
        if (enemyPath.Count < 1) return;

        for (int i =0;i<enemyPath.Count-1;i++)
        {
            AddDontSpawnNodes(enemyPath[i], enemyPath[i + 1]);
        }

        for (int i = 1; i < enemyPath.Count; i++)
        {
            var tempPos = new Vector3(nodeParent.position.x + enemyPath[i].x * nodeOffset.x, 1.5f, nodeParent.position.y + enemyPath[i].y * nodeOffset.y);
            var tempObj = Instantiate(wayPoint);
            tempObj.name = "WayPoint " + i + "";
            tempObj.SetParent(wayPointParent);
            tempObj.transform.position = tempPos;
            nodes.Add(tempObj);
        }
    }
    
    private void AddDontSpawnNodes(GridPos node1, GridPos node2)
    {
        var tempObj = Instantiate(path);

        var child = path.GetChild(0);
        child.localScale = new Vector3(nodeSize.x + (nodeOffset.x - nodeSize.x) , nodeSize.y , nodeSize.z + (nodeOffset.y - nodeSize.z));
        child.localPosition = new Vector3(child.localScale.x / 2,0, child.localScale.z/2);
        var scaleX = Math.Abs(node1.x - node2.x) > 0 ? Math.Abs(node1.x - node2.x) + 1 : 1;
        var scaleZ = Math.Abs(node1.y - node2.y) > 0 ? Math.Abs(node1.y - node2.y) + 1 : 1;

        tempObj.localScale = new Vector3(scaleX, nodeSize.y , scaleZ);
        tempObj.SetParent(pathParent);
        Vector3 tempPos;
        if (GridPos.Distance(GridPos.GridPosZero(), node1) > GridPos.Distance(GridPos.GridPosZero(), node2))
            tempPos = new Vector3((pathParent.position.x + node2.x * nodeOffset.x) - nodeOffset.x / 2, 0, (pathParent.position.y + node2.y * nodeOffset.y) - nodeOffset.y / 2);
        else
            tempPos = new Vector3((pathParent.position.x + node1.x * nodeOffset.x) - nodeOffset.x / 2, 0, (pathParent.position.y + node1.y * nodeOffset.y) - nodeOffset.y / 2);

        tempObj.transform.position = tempPos;
        pathList.Add(tempObj);


        if (node1.x < node2.x)
        {
            for (int i = node1.x; i <= node2.x; i++)
            {
                if (node1.y < node2.y)
                {
                    for (int j = node1.y; j <= node2.y; j++)
                    {
                        dontSpawNodeList.Add(new GridPos(i, j));
                    }
                }
                else
                {
                    for (int j = node1.y; j >= node2.y; j--)
                    {
                        dontSpawNodeList.Add(new GridPos(i, j));
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
                        dontSpawNodeList.Add(new GridPos(i, j));
                    }
                }
                else
                {
                    for (int j = node1.y; j >= node2.y; j--)
                    {
                        dontSpawNodeList.Add(new GridPos(i, j));
                    }
                }
            }
        }
    }
}
