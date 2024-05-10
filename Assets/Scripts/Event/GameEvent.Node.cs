using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public static partial class GameEvent
{
    public class NodeSelectEvent : GameEventArgs<NodeSelectEvent>
    {
        public Vector2Short GridPos { get; set; }
        public static NodeSelectEvent CreateEvent(Vector2Short GridPos)
        {
            var @event = CreateEvent();
            @event.GridPos = GridPos;
            return @event;
        }
    }

    public class NodeCancelSelectEvent : GameEventArgs<NodeCancelSelectEvent>
    {

    }
}
