using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    private GridPos pos;
    public GridPos Pos { get; private set; }

    public Color hoverColor;
    public Color notEnoughMoneyColor;
    public Color SelectColor = Color.blue;
    private Color startColor;
    private Renderer render;

    private GameObject turret;
    private TurretBlueprint turretBlueprint;
    public TurretBlueprint TurretBlueprint => turretBlueprint;
    private bool isUpgrade = false;
    public bool IsUpgrade => isUpgrade;
    private BuildManager buildManager => BuildManager.instance;
    private GameManager gameManager => GameManager.instance;
    private void Start()
    {
        render = GetComponent<Renderer>();
        startColor = render.material.color;
    }

    public void SetPos(int x,int y)
    {
        pos = new GridPos(x,y);
    }

    public Vector3 GetBuildPos(Vector3 turretOffset)
    {
        return transform.position + turretOffset;
    }

    public void BuildTurret(TurretBlueprint blueprint)
    {
        if (gameManager.Money < blueprint.Cost)
        {
            Debug.Log("½a");
            return;
        }

        gameManager.CostMoney(blueprint.Cost);

        GameObject _turret = Instantiate(blueprint.turretPrefab, GetBuildPos(blueprint.BuildOffset), Quaternion.identity);
        turret = _turret;
        GameObject effect = Instantiate(buildManager.BuildEffect, GetBuildPos(blueprint.BuildOffset), Quaternion.identity);
        Destroy(effect, 5f);
        turretBlueprint = blueprint;
    }

    public void UpgradeTurret()
    {
        if (gameManager.Money < turretBlueprint.Cost)
        {
            Debug.Log("½a");
            return;
        }

        gameManager.CostMoney(turretBlueprint.Cost);

        Destroy(turret);

        GameObject _turret = Instantiate(turretBlueprint.upgradeTurretPrefab, GetBuildPos(turretBlueprint.BuildOffset), Quaternion.identity);
        turret = _turret;
        GameObject effect = Instantiate(buildManager.BuildEffect, GetBuildPos(turretBlueprint.BuildOffset), Quaternion.identity);
        Destroy(effect, 5f);

        isUpgrade = true;
    }

    public void SellTurret()
    {
        gameManager.AddMoney(turretBlueprint.GetSellPirce());
        Destroy(turret);

        GameObject effect = Instantiate(buildManager.SellEffect, GetBuildPos(turretBlueprint.BuildOffset), Quaternion.identity);
        Destroy(effect, 5f);

        turretBlueprint = null;
        isUpgrade = false;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;


        if (turret != null)
        {
            BuildManager.nodeSelected?.Invoke(this);
            return;
        }

        if (!buildManager.CanBuild)
            return;

        BuildTurret(buildManager.TurretTobuild);
    }

    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (!buildManager.CanBuild)
            return;

        if(buildManager.HasMoney)
            render.material.color = hoverColor;
        else
            render.material.color = notEnoughMoneyColor;

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