using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GridDebugger : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] Vector2Short Grid;

        [Header("Grid State")]
        [SerializeField]
        Vector2Short gridPos;
        [SerializeField]
        GridData gridData;
        [HideInInspector]
        public TextMesh[,] debugTexts;

        private int size => GridExtension.GridUnitSize;


        public List<Transform> childs = new List<Transform>();
        private BuildManager buildManager => BuildManager.instance;

        public void Create(Vector2Short gridSize)
        {
            Grid = gridSize;
            debugTexts = new TextMesh[Grid.x, Grid.y];
            childs.Clear();
            var debugTextTemnplate = transform.GetChild(0).GetComponent<TextMesh>();
            var originCount = transform.childCount;
            for (int y = 0; y < Grid.y; y++)
            {
                for (int x = 0; x < Grid.x; x++)
                {
                    var index = y * Grid.x + x;
                    TextMesh textMesh;
                    if (index + 1 < originCount)
                        textMesh = transform.GetChild(index + 1).GetComponent<TextMesh>();
                    else
                    {
                        textMesh = Instantiate(debugTextTemnplate, transform);
                        textMesh.GetComponent<MeshRenderer>().enabled = true;
                    }

                    textMesh.text = $"({x}, {y})";
                    textMesh.name = textMesh.text;
                    textMesh.transform.localPosition = new Vector3(x * size, 0, y * size);
                    //Debug.Log("Pos:" + textMesh.transform.localPosition);

                    debugTexts[x, y] = textMesh;
                    childs.Add(textMesh.transform);
                }
            }
            Debug.Log($"GridSize: {gridSize}, Child Count: {transform.childCount},  Index: {gridSize.x * gridSize.y}");
            debugTextTemnplate.GetComponent<MeshRenderer>().enabled = false;
        }




        public void ChangeColor(int x, int y, GridState gridState)
        {
            var grid = childs.Find(z => z.name.Equals($"({x}, {y})"));
            if (grid != null)
            {
                if (gridState == GridState.Building)
                    grid.GetComponent<TextMesh>().color = Color.blue;
                else if (gridState == GridState.Empty)
                    grid.GetComponent<TextMesh>().color = Color.white;
                else if (gridState == GridState.EnemyPath)
                    grid.GetComponent<TextMesh>().color = Color.red;
                else if (gridState == GridState.Block)
                    grid.GetComponent<TextMesh>().color = Color.black;
                else if (gridState == GridState.Preview)
                    grid.GetComponent<TextMesh>().color = Color.green;
            }
        }

        private void OnEnable()
        {
            if (childs.Count != 0)
            {

                for (int x = 0; x < GameManager.instance.NowMapData.MapSize.x; x++)
                {
                    for (int y = 0; y < GameManager.instance.NowMapData.MapSize.y; y++)
                    {
                        var grid = childs.Find(z => z.name.Equals($"({x}, {y})"));
                        if (grid != null)
                        {
                            var gridState = buildManager.GetGridState(x, y);
                            if (gridState == GridState.Building)
                                grid.GetComponent<TextMesh>().color = Color.blue;
                            else if (gridState == GridState.Empty)
                                grid.GetComponent<TextMesh>().color = Color.white;
                            else if (gridState == GridState.EnemyPath)
                                grid.GetComponent<TextMesh>().color = Color.red;
                            else if (gridState == GridState.Block)
                                grid.GetComponent<TextMesh>().color = Color.black;
                            else if (gridState == GridState.Preview)
                                grid.GetComponent<TextMesh>().color = Color.green;
                        }
                    }
                }
            }
        }

        [ContextMenu("ShowGird")]
        public void ShowGrid()
        {
            foreach (var child in childs)
                child.gameObject.SetActive(true);
        }

        [ContextMenu("HideGird")]
        public void HideGrid()
        {
            foreach (var child in childs)
                child.gameObject.SetActive(false);
        }

    }
#endif
}