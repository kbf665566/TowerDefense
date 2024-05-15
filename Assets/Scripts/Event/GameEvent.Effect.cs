using System;
using UnityEngine;


public static partial class GameEvent
{ 

    public class GameEffectShowEvent : GameEventArgs<GameEffectShowEvent>
    {
        public Vector3 Pos { get; set; }
        public GameObject EffectPrefab { get; set; }
        public static GameEffectShowEvent CreateEvent(Vector3 pos, GameObject effectPrefab)
        {
            var @event = CreateEvent();
            @event.Pos = pos;
            @event.EffectPrefab = effectPrefab;
            return @event;
        }
    }

    public class GameEffectShowWithEnumEvent : GameEventArgs<GameEffectShowWithEnumEvent>
    {
        public Vector3 Pos { get; set; }
        public GameEffectType GameEffectType { get; set; }

        public static GameEffectShowWithEnumEvent CreateEvent(Vector3 pos, GameEffectType gameEffectType)
        {
            var @event = CreateEvent();
            @event.Pos = pos;
            @event.GameEffectType = gameEffectType;
            return @event;
        }
    }
}
