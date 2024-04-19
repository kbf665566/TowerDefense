using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NodeSpawner : MonoBehaviour
{
    #region Node
    [SerializeField] private Transform node;
    private List<Transform> nodes = new List<Transform>();
    [SerializeField] private Vector3 nodeSize = Vector3.one;
    [SerializeField] Vector2 nodeOffset = Vector2.zero;
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;
    private Transform startObj;
    private Transform endObj;
    #endregion

    [SerializeField] private Transform ground;
    private List<Transform> grounds = new List<Transform>();

    [SerializeField] private Transform nodeParent;
    [SerializeField] private GridPos mapSize;
    [SerializeField] private List<GridPos> path = new List<GridPos>();
    private List<GridPos> dontSpawNodeList = new List<GridPos>();
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
            var startPos = new Vector3(nodeParent.position.x + path[0].x * nodeOffset.x, nodeSize.y, nodeParent.position.y + +path[0].y * nodeOffset.y);
            var tempstartObj = Instantiate(start);
            tempstartObj.SetParent(nodeParent);
            tempstartObj.transform.position = startPos;
            nodes.Add(tempstartObj);

            end.localScale = nodeSize;
            var endPos = new Vector3(nodeParent.position.x + path[path.Count - 1].x * nodeOffset.x, nodeSize.y, nodeParent.position.y + +path[path.Count - 1].y * nodeOffset.y);
            var tempEndObj = Instantiate(end);
            tempEndObj.SetParent(nodeParent);
            tempEndObj.transform.position = endPos;
            nodes.Add(tempEndObj);
        }
    }
    [ContextMenu("Delete Nodes")]
    public void DeleteNode()
    {
        if (nodes.Count > 0)
        {
            for (int i = 0; i < nodes.Count; i++)
                DestroyImmediate(nodes[i].gameObject);
        }
        nodes.Clear();

        if (grounds.Count > 0)
        {
            for (int i = 0; i < grounds.Count; i++)
                DestroyImmediate(grounds[i].gameObject);
        }
        grounds.Clear();
        DestroyImmediate(startObj);
        DestroyImmediate(endObj);
    }

    public void SetPath()
    {
        dontSpawNodeList.Clear();
        if (path.Count < 1) return;

        for (int i =0;i<path.Count-1;i++)
        {
            AddDontSpawnNodes(path[i], path[i + 1]);
        }
    }
    
    private void AddDontSpawnNodes(GridPos node1, GridPos node2)
    {
        var tempObj = Instantiate(ground);

        var child = ground.GetChild(0);
        child.localScale = new Vector3(nodeSize.x + (nodeOffset.x - nodeSize.x) , nodeSize.y , nodeSize.z + (nodeOffset.y - nodeSize.z));
        child.localPosition = new Vector3(child.localScale.x / 2,0, child.localScale.z/2);
        var scaleX = Math.Abs(node1.x - node2.x) > 0 ? Math.Abs(node1.x - node2.x) + 1 : 1;
        var scaleZ = Math.Abs(node1.y - node2.y) > 0 ? Math.Abs(node1.y - node2.y) + 1 : 1;

        tempObj.localScale = new Vector3(scaleX, nodeSize.y , scaleZ);
        tempObj.SetParent(nodeParent);
        Vector3 tempPos;
        if (GridPos.Distance(GridPos.GridPosZero(), node1) > GridPos.Distance(GridPos.GridPosZero(), node2))
            tempPos = new Vector3((nodeParent.position.x + node2.x * nodeOffset.x) - nodeOffset.x / 2, 0, (nodeParent.position.y + node2.y * nodeOffset.y) - nodeOffset.y / 2);
        else
            tempPos = new Vector3((nodeParent.position.x + node1.x * nodeOffset.x) - nodeOffset.x / 2, 0, (nodeParent.position.y + node1.y * nodeOffset.y) - nodeOffset.y / 2);

        tempObj.transform.position = tempPos;
        grounds.Add(tempObj);


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
