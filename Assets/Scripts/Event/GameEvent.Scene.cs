using System;
using UnityEngine;


public static partial class GameEvent
{

    public class SceneChangeEventArgs : GameEventArgs<SceneChangeEventArgs>
    {
        public string SceneName { get; set; }
        public static SceneChangeEventArgs CreateEvent(string sceneName)
        {
            var @event = CreateEvent();
            @event.SceneName = sceneName;
            return @event;
        }
    }
}
