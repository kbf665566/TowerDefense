using System;
using UnityEngine;


public static partial class GameEvent
{

    public class GameOverEvent : GameEventArgs<GameOverEvent>
    {
        public static GameOverEvent CreateEvent()
        {
            var @event = CreateEvent();
            return @event;
        }
    }

    public class LevelStartEvent : GameEventArgs<LevelStartEvent>
    {
        public static LevelStartEvent CreateEvent()
        {
            var @event = CreateEvent();
            return @event;
        }
    }

    public class NextWaveStartEvent : GameEventArgs<NextWaveStartEvent>
    {
        public int NowWave { get; set; }
        public static NextWaveStartEvent CreateEvent(int nowWave)
        {
            var @event = CreateEvent();
            @event.NowWave = nowWave;
            return @event;
        }
    }

    public class WaveEndEvent : GameEventArgs<WaveEndEvent>
    {

    }

    public class GameWinEvent : GameEventArgs<GameWinEvent>
    {
        public static GameWinEvent CreatEvent()
        {
            var @event = CreatEvent();
            return @event;
        }
    }

}

