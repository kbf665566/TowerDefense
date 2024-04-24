using UnityEngine;


public static partial class GameEvent
{
    public class NodeSelectEvent
    {
        public Node Node { get; }
        public NodeSelectEvent(Node node)
        {
            Node = node;
        }
    }

    public class NodeCancelSelectEvent
    {
        public NodeCancelSelectEvent()
        {

        }
    }
}
