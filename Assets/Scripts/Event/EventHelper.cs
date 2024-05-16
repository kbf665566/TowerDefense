using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHelper : MonoBehaviour
{
    public static EventHandler<GameEvent.SceneChangeEvent> SceneChangedEvent;

    public static EventHandler<GameEvent.TowerBuildEvent> TowerBuiltEvent;
    public static EventHandler<GameEvent.TowerSelectEvent> TowerSelectedEvent;
    public static EventHandler<GameEvent.TowerSellEvent> TowerSoldEvent;
    public static EventHandler<GameEvent.TowerUpgradeEvent> TowerUpgradedEvent;
    public static EventHandler<GameEvent.TowerPreviewBuildEvent> TowerPreviewBuiltEvent;
    public static EventHandler<GameEvent.TowerCancelPreviewEvent> TowerCanceledPreviewEvent;

    public static EventHandler<GameEvent.NodeSelectEvent> NodeSelectedEvent;
    public static EventHandler<GameEvent.NodeCancelSelectEvent> NodeCancelSelectedEvent;

    public static EventHandler<GameEvent.GameOverEvent> GameOverEvent;
    public static EventHandler<GameEvent.LevelStartEvent> LevelStartedEvent;
    public static EventHandler<GameEvent.GameWinEvent> GameWonEvent;

    public static EventHandler<GameEvent.ShowTipEvent> TipShowEvent;
    public static EventHandler<GameEvent.HideTipEvent> TipHideEvent;


    public static EventHandler<GameEvent.GameEffectShowEvent> EffectShowEvent;
}
