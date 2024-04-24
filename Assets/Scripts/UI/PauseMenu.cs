using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public GameObject ui;
    [SerializeField] private string levelName = "Level1";
    [SerializeField] private SceneFader sceneFader;
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
        sceneFader.FadeTo(levelName);
    }

    public void Menu()
    {
        Toggle();
        sceneFader.FadeTo(GameSetting.MainMenuName);
    }
}
