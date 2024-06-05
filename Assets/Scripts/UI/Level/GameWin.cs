using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWin : MonoBehaviour
{
    [SerializeField] private GameObject ui;
    public void Menu()
    {
        EventHelper.SceneChangedEvent.Invoke(this, GameEvent.SceneChangeEvent.CreateEvent(GameSetting.MainMenuName));
    }

    private void GameWinEvent(object s, GameEvent.GameWinEvent e)
    {
        ui.SetActive(true);
    }

    private void OnEnable()
    {
        EventHelper.GameWonEvent += GameWinEvent;
    }

    private void OnDisable()
    {
        EventHelper.GameWonEvent -= GameWinEvent;
    }
}
