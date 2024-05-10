using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    private Vector2Short pos;
    public Vector2Short Pos { get; private set; }

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
    private LevelManager levelManager => LevelManager.instance;
    private void Start()
    {
        render = GetComponent<Renderer>();
        startColor = render.material.color;
    }

    public void SetPos(int x,int y)
    {
        pos = new Vector2Short(x,y);
    }

    public Vector3 GetBuildPos(Vector3 turretOffset)
    {
        return transform.position + turretOffset;
    }

    public void BuildTurret(TurretBlueprint blueprint)
    {
        if (levelManager.Money < blueprint.Cost)
        {
            Debug.Log("窮");
            return;
        }

        levelManager.CostMoney(blueprint.Cost);

        GameObject _turret = Instantiate(blueprint.turretPrefab, GetBuildPos(blueprint.BuildOffset), Quaternion.identity);
        turret = _turret;
        GameObject effect = Instantiate(buildManager.BuildEffect, GetBuildPos(blueprint.BuildOffset), Quaternion.identity);
        Destroy(effect, 5f);
        turretBlueprint = blueprint;
    }

    public void UpgradeTurret()
    {
        if (levelManager.Money < turretBlueprint.Cost)
        {
            Debug.Log("窮");
            return;
        }

        levelManager.CostMoney(turretBlueprint.Cost);

        Destroy(turret);

        GameObject _turret = Instantiate(turretBlueprint.upgradeTurretPrefab, GetBuildPos(turretBlueprint.BuildOffset), Quaternion.identity);
        turret = _turret;
        GameObject effect = Instantiate(buildManager.BuildEffect, GetBuildPos(turretBlueprint.BuildOffset), Quaternion.identity);
        Destroy(effect, 5f);

        isUpgrade = true;
    }

    public void SellTurret()
    {
        levelManager.AddMoney(turretBlueprint.GetSellPirce());
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

        EventHelper.NodeSelectedEvent.Invoke(this, GameEvent.NodeSelectEvent.CreateEvent(pos));
    }
}
