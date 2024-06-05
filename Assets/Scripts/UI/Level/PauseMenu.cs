using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public GameObject ui;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        ui.SetActive(!ui.activeSelf);
        if (ui.activeSelf)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    public void Retry()
    {
        Toggle();
        EventHelper.SceneChangedEvent.Invoke(this,GameEvent.SceneChangeEvent.CreateEvent(GameManager.instance.NowMapData.MapName));
    }

    public void Menu()
    {
        Toggle();
        EventHelper.SceneChangedEvent.Invoke(this, GameEvent.SceneChangeEvent.CreateEvent(GameSetting.MainMenuName));
    }
}
