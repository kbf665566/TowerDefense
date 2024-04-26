using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public static partial class GameEvent
{
    public class NodeSelectEventArgs : GameEventArgs<NodeSelectEventArgs>
    {
        public Node Node { get; set; }
        public static NodeSelectEventArgs CreateEvent(Node node)
        {
            var @event = CreateEvent();
            @event.Node = node;
            return @event;
        }
    }

    public class NodeCancelSelectEvent : GameEventArgs<NodeCancelSelectEvent>
    {

    }
}
