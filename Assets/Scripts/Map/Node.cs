﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    private Vector2Short pos;
    public Vector2Short Pos => pos;


    private void Start()
    {
        pos = new Vector2Short((short)transform.localPosition.x, (short)transform.localPosition.z);
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        var uid = BuildManager.instance.GetTowerUid(pos);
        if (uid != 0)
            EventHelper.TowerSelectedEvent.Invoke(this, GameEvent.TowerSelectEvent.CreateEvent(uid));
        else
            EventHelper.NodeSelectedEvent.Invoke(this, GameEvent.NodeSelectEvent.CreateEvent(pos));
    }
}
