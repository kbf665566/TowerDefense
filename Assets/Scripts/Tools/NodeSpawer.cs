using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSpawer : MonoBehaviour
{
    #region Node
    [SerializeField] private Transform node;
    private List<Transform> nodes = new List<Transform>();
    [SerializeField] private Vector3 nodeSize = Vector3.one;
    [SerializeField] Vector2 nodeOffset = Vector2.zero;

    #endregion
    [SerializeField] private Transform nodeParent;
    [SerializeField] private Vector2 mapSize = Vector2.zero;
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
            for (int i = 0; i < mapSize.x; i++)
            {
                for (int j = 0; j < mapSize.y; j++)
                {
                    var tempPos = new Vector3(nodeParent.position.x + i * nodeOffset.x, 0, nodeParent.position.y + j * nodeOffset.y);
                    var tempObj = Instantiate(node);
                    tempObj.SetParent(nodeParent);
                    tempObj.transform.position = tempPos;
                    nodes.Add(tempObj);
                }
            }

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
    }
}
