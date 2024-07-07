using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static partial class GameEvent
{
    public class TowerBuildEvent : GameEventArgs<TowerBuildEvent>
    {
        public int Id { get; set; }
        public Vector2Short GridPos { get; set; }
        public static TowerBuildEvent CreateEvent(int id,Vector2Short gridPos)
        {
            var @event = CreateEvent();
            @event.Id = id;
            @event.GridPos = gridPos;
            return @event;
        }
    }

    public class TowerUpgradeEvent : GameEventArgs<TowerUpgradeEvent>
    {
        public int Uid { get; set; }
        public static TowerUpgradeEvent CreateEvent(int uid)
        {
            var @event = CreateEvent();
            @event.Uid = uid;
            return @event;
        }
    }

    public class TowerChangeAttackModeEvent : GameEventArgs<TowerChangeAttackModeEvent>
    {
        public int Uid { get; set; }
        public TowerAttackMode AttackMode { get; set; }
        public static TowerChangeAttackModeEvent CreateEvent(int uid,TowerAttackMode towerAttackMode)
        {
            var @event = CreateEvent();
            @event.Uid = uid;
            @event.AttackMode = towerAttackMode;
            return @event;
        }
    }

    public class TowerSellEvent : GameEventArgs<TowerSellEvent>
    {
        public int Uid { get; set; }
        public static TowerSellEvent CreateEvent(int uid)
        {
            var @event = CreateEvent();
            @event.Uid = uid;
            return @event;
        }
    }

    public class TowerPreviewBuildEvent : GameEventArgs<TowerPreviewBuildEvent>
    {
        public int TowerId { get; set; }
        public float ShootRange { get; set; }
        public Vector2Short Size { get; set; }
        public Vector2Short GridPos { get; set; }
        public static TowerPreviewBuildEvent CreateEvent(int towerId,float shootRange,Vector2Short size, Vector2Short gridPos)
        {
            var @event = CreateEvent();
            @event.TowerId = towerId;
            @event.ShootRange = shootRange;
            @event.Size = size;
            @event.GridPos = gridPos;
            return @event;
        }
    }

    public class MovePreviewBuildEvent : GameEventArgs<MovePreviewBuildEvent>
    {
        public Vector2Short GridPos { get; set; }
        public static MovePreviewBuildEvent CreateEvent(Vector2Short gridPos)
        {
            var @event = CreateEvent();
            @event.GridPos = gridPos;
            return @event;
        }
    }

    public class TowerSelectEvent : GameEventArgs<TowerSelectEvent>
    {
        public int Uid { get; set; }
        public static TowerSelectEvent CreateEvent(int uid)
        {
            var @event = CreateEvent();
            @event.Uid = uid;
            return @event;
        }
    }

    public class TowerCancelPreviewEvent : GameEventArgs<TowerCancelPreviewEvent>
    {

    }

    public class TowerMakeMoneyEvent : GameEventArgs<TowerMakeMoneyEvent>
    {
        public int GetMoney { get; set; }
        public static TowerMakeMoneyEvent CreateEvent(int money)
        {
            var @event = CreateEvent();
            @event.GetMoney = money;
            return @event;
        }
    }
}