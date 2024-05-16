using System;
using UnityEngine;


public static partial class GameEvent
{ 
    public class GameEffectShowEvent : GameEventArgs<GameEffectShowEvent>
    {
        public Vector3 Pos { get; set; }
        public GameEffectType GameEffectType { get; set; }

        public static GameEffectShowEvent CreateEvent(Vector3 pos, GameEffectType gameEffectType)
        {
            var @event = CreateEvent();
            @event.Pos = pos;
            @event.GameEffectType = gameEffectType;
            return @event;
        }
    }
}
