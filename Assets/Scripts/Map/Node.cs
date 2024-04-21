using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    private GridPos pos;
    public GridPos Pos { get; private set; }

    public Color hoverColor;
    private Color startColor;
    private Renderer render;

    private GameObject turret;
    private BuildManager buildManager => BuildManager.instance;
    public Vector3 posOffset;
    private void Start()
    {
        render = GetComponent<Renderer>();
        startColor = render.material.color;
    }

    public void SetPos(int x,int y)
    {
        pos = new GridPos(x,y);
    }

    public Vector3 GetBuildPos()
    {
        return transform.position + posOffset;
    }

    public void BuildTurret(GameObject turret)
    {
        this.turret = turret;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!buildManager.CanBuild)
            return;

        if(turret != null)
        {
            Debug.Log("無法建造");
            return;
        }

        buildManager.BuildTurretOn(this);
    }

    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (!buildManager.CanBuild)
            return;

        render.material.color = hoverColor;
    }
    private void OnMouseExit()
    {
        render.material.color = startColor;
    }
}
[Serializable]
public class GridPos
{
    public int x;
    public int y;

    public GridPos(int x,int y)
    {
        this.x = x;
        this.y = y;
    }
    public bool Equal(int x,int y)
    {
        return this.x == x && this.y == y;
    }

    public static GridPos GridPosZero()
    {
        return new GridPos(0,0);
    }
    public static double Distance(GridPos p1,GridPos p2)
    {
        return Math.Sqrt(Math.Pow(p1.x - p2.x, 2) + Math.Pow(p1.y - p2.y, 2));
    }
}