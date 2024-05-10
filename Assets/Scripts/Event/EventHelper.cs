using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHelper : MonoBehaviour
{
    public static EventHandler<GameEvent.SceneChangeEvent> SceneChangedEvent;

    public static EventHandler<GameEvent.TowerBuildEvent> TowerBuiltEvent;
    public static EventHandler<GameEvent.TowerSellEvent> TowerSoldEvent;
    public static EventHandler<GameEvent.TowerUpgradeEvent> TowerUpgradedEvent;

    public static EventHandler<GameEvent.NodeSelectEvent> NodeSelectedEvent;
    public static EventHandler<GameEvent.NodeCancelSelectEvent> NodeCancelSelectedEvent;

    public static EventHandler<GameEvent.GameOverEvent> GameOverEvent;
    public static EventHandler<GameEvent.LevelStartEvent> LevelStartedEvent;
    public static EventHandler<GameEvent.GameWinEvent> GameWonEvent;

    public static EventHandler<GameEvent.ShowTipEvent> TipShowEvent;
    public static EventHandler<GameEvent.HideTipEvent> TipHideEvent;

    // Start is called before the first frame update
    void Start()
    {
        //SceneChangedEvent += sss;
       // SceneChangedEvent.Invoke(this,GameEvent.SceneChangeEventArgs.CreateEvent(""));
    }

    private void q()
    {
        //SceneChangedEvent.Invoke(this, GameEvent.SceneChangeEventArgs.CreateEvent(""));
    }
    private void sss(object s, GameEvent.SceneChangeEvent e)
    {
       // Debug.Log("test sss");
    }

    /*
     * On Other Scripts
    private void Awake()
    {
        EventHelper.test += sss;
    }
    private void OnDisable()
    {
        EventHelper.test -= sss;
    }
    private void sss(object s, GameEvent.SceneChangeEventArgs e)
    {
        Debug.Log("test sss over");
    }
    */
}
