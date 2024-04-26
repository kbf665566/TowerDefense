using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHelper : MonoBehaviour
{
    public static EventHandler<GameEvent.SceneChangeEventArgs> fuck;
    // Start is called before the first frame update
    void Start()
    {
        fuck += sss;
        fuck += ddd;
        fuck += eee;
        fuck.Invoke(this,GameEvent.SceneChangeEventArgs.CreateEvent(""));
    }
    [ContextMenu("q")]
    private void q()
    {
        fuck.Invoke(this, GameEvent.SceneChangeEventArgs.CreateEvent(""));
    }
    private void sss(object s, GameEvent.SceneChangeEventArgs e)
    {
        Debug.Log("fuck");
    }
    private void ddd(object s, GameEvent.SceneChangeEventArgs e)
    {
        Debug.Log("fuck you");
    }
    private void eee(object s, GameEvent.SceneChangeEventArgs e)
    {
        Debug.Log("faq");
    }


    /*
     * On Other Scripts
    private void Awake()
    {
        EventHelper.fuck += sss;
    }
    private void OnDisable()
    {
        EventHelper.fuck -= sss;
    }
    private void sss(object s, GameEvent.SceneChangeEventArgs e)
    {
        Debug.Log("fuck your mother");
    }
    */
}
