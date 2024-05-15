using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEffectManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ShowEffect(object s,GameEvent.GameEffectShowEvent e)
    {

    }

    public void ShowEffect(object s, GameEvent.GameEffectShowWithEnumEvent e)
    {

    }

    private void OnEnable()
    {
        EventHelper.EffectShowEvent += ShowEffect;
        EventHelper.EnumEffectShowEvent += ShowEffect;
    }

    private void OnDisable()
    {
        EventHelper.EffectShowEvent -= ShowEffect;
        EventHelper.EnumEffectShowEvent -= ShowEffect;
    }
}

public enum GameEffectType
{
    Boom,
}