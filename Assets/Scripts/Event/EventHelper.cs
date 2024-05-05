using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHelper : MonoBehaviour
{
    public static EventHandler<GameEvent.SceneChangeEventArgs> test;
    // Start is called before the first frame update
    void Start()
    {
        test += sss;
        test += ddd;
        test += eee;
        test.Invoke(this,GameEvent.SceneChangeEventArgs.CreateEvent(""));
    }
    [ContextMenu("q")]
    private void q()
    {
        test.Invoke(this, GameEvent.SceneChangeEventArgs.CreateEvent(""));
    }
    private void sss(object s, GameEvent.SceneChangeEventArgs e)
    {
        Debug.Log("test sss");
    }
    private void ddd(object s, GameEvent.SceneChangeEventArgs e)
    {
        Debug.Log("test ddd");
    }
    private void eee(object s, GameEvent.SceneChangeEventArgs e)
    {
        Debug.Log("test eee");
    }


    /*
     * On Other Scripts
    private void Awake()
    {
        EventHelper.test += sss;
    }
    private void OnDisable()
    {
        EventHelper.test -= sss;
    }
    private void sss(object s, GameEvent.SceneChangeEventArgs e)
    {
        Debug.Log("test sss over");
    }
    */
}
