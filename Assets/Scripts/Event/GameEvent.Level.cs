using System;
using UnityEngine;


public static partial class GameEvent
{

    public class GameOverEvent : EventArgs
    {
        public static GameOverEvent CreatEvent()
        {
            var @event = CreatEvent();
            return @event;
        }
    }

    public class LevelStartEvent : EventArgs
    {
        public static LevelStartEvent CreatEvent()
        {
            var @event = CreatEvent();
            return @event;
        }
    }

    public class GameWinEvent : EventArgs
    {
        public static GameWinEvent CreatEvent()
        {
            var @event = CreatEvent();
            return @event;
        }
    }
}
