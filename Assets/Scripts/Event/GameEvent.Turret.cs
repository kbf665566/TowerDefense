using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static partial class GameEvent
{
    public class TurretBuildEvent
    {
        public TurretBlueprint bluePrint { get; }
        public TurretBuildEvent(TurretBlueprint turretBlueprint)
        {
            bluePrint = turretBlueprint;
        }
    }

    public class TurretUpgradeEvent
    {
        public TurretUpgradeEvent()
        {
            
        }
    }

    public class TurretSellEvent
    {
        public TurretSellEvent()
        {

        }
    }

}

/*
 public class Publisher
{
    // Declare the delegate (if using non-generic pattern).
    public delegate void SampleEventHandler(object sender, SampleEventArgs e);

    // Declare the event.
    public event SampleEventHandler SampleEvent;

    // Wrap the event in a protected virtual method
    // to enable derived classes to raise the event.
    protected virtual void RaiseSampleEvent()
    {
        // Raise the event in a thread-safe manner using the ?. operator.
        SampleEvent?.Invoke(this, new SampleEventArgs("Hello"));
    }
}
 */