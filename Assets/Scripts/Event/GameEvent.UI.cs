using System;
using UnityEngine;


public static partial class GameEvent
{

    public class ShowTipEvent : GameEventArgs<ShowTipEvent>
    {
        public string Title { get; set; }
        public string Information { get; set; }
        public static ShowTipEvent CreateEvent(string title,string information)
        {
            var @event = CreateEvent();
            @event.Title = title;
            @event.Information = information;
            return @event;
        }
    }

    public class HideTipEvent : GameEventArgs<HideTipEvent>
    {
    }
}
