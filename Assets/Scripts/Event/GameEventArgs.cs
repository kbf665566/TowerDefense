using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventArgs<T> : EventArgs where T : class, new()
{
    public static T CreateEvent()
    {
        T @event = new T();
        return @event;
    }
}
