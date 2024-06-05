using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject ui;
    public void Retry()
    {
        EventHelper.SceneChangedEvent.Invoke(this, GameEvent.SceneChangeEvent.CreateEvent(GameManager.instance.NowMapData.MapName));
    }

    public void Menu()
    {
        EventHelper.SceneChangedEvent.Invoke(this, GameEvent.SceneChangeEvent.CreateEvent(GameSetting.MainMenuName));
    }

    private void GameOverEvent(object s,GameEvent.GameOverEvent e)
    {
        ui.SetActive(true);
    }

    private void OnEnable()
    {
        EventHelper.GameOverEvent += GameOverEvent;
    }

    private void OnDisable()
    {
        EventHelper.GameOverEvent -= GameOverEvent;
    }
}
