using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private GridPos pos;
    public GridPos Pos { get; private set; }

    public Color hoverColor;
    private Color startColor;
    private Renderer render;

    private GameObject turret;
    private void Start()
    {
        render = GetComponent<Renderer>();
        startColor = render.material.color;
    }

    public void SetPos(int x,int y)
    {
        pos = new GridPos(x,y);
    }

    private void OnMouseDown()
    {
        if(turret != null)
        {
            return;
        }
        GameObject turretToBuild = BuildManager.instance.TurretTobuild;
        turret = Instantiate(turretToBuild,new Vector3(transform.position.x -0.5f, transform.position.y + 1f, transform.position.z),transform.rotation);
    }

    private void OnMouseEnter()
    {
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