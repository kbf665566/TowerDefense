using System;
using UnityEngine;


public static partial class GameEvent
{

    public class EnemyDieEvent : GameEventArgs<EnemyDieEvent>
    {
        public int Uid { get; set; }
        public int Value { get; set; }
        public static EnemyDieEvent CreateEvent(int uid,int value)
        {
            var @event = CreateEvent();
            @event.Uid = uid;
            @event.Value = value;
            return @event;
        }
    }

    public class EnemyEndPathEvent : GameEventArgs<EnemyEndPathEvent>
    {
        public int Uid { get; set; }
        public int Damage { get; set; }
        public static EnemyEndPathEvent CreateEvent(int uid,int damage)
        {
            var @event = CreateEvent();
            @event.Uid = uid;
            @event.Damage = damage;
            return @event;
        }
    }
}
