using System;
using UnityEngine;


public static partial class GameEvent
{

    public class SceneChangeEvent : GameEventArgs<SceneChangeEvent>
    {
        public string SceneName { get; set; }
        public static SceneChangeEvent CreateEvent(string sceneName)
        {
            var @event = CreateEvent();
            @event.SceneName = sceneName;
            return @event;
        }
    }
}
