using System;
using UnityEngine;


public static partial class GameEvent
{ 
    public class GameEffectShowEvent : GameEventArgs<GameEffectShowEvent>
    {
        public Vector3 Pos { get; set; }
        public GameEffectType GameEffectType { get; set; }
        public float Size { get; set; }

        public static GameEffectShowEvent CreateEvent(Vector3 pos, GameEffectType gameEffectType)
        {
            var @event = CreateEvent();
            @event.Pos = pos;
            @event.GameEffectType = gameEffectType;
            return @event;
        }

        public static GameEffectShowEvent CreateEvent(Vector3 pos, GameEffectType gameEffectType,float size)
        {
            var @event = CreateEvent();
            @event.Pos = pos;
            @event.GameEffectType = gameEffectType;
            @event.Size = size;
            return @event;
        }
    }

    public class GameEffectShowWithTextEvent : GameEventArgs<GameEffectShowWithTextEvent>
    {
        public Vector3 Pos { get; set; }
        public GameEffectType GameEffectType { get; set; }
        public string ShowText { get; set; }

        public static GameEffectShowWithTextEvent CreateEvent(Vector3 pos, GameEffectType gameEffectType,string text)
        {
            var @event = CreateEvent();
            @event.Pos = pos;
            @event.GameEffectType = gameEffectType;
            @event.ShowText = text;
            return @event;
        }
    }
}
