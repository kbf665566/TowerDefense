using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour
{
    [SerializeField] private SceneFader sceneFader;
    [SerializeField] private string levelName = "Level1";
    public void Retry()
    {
        sceneFader.FadeTo(levelName);
    }

    public void Menu()
    {
        sceneFader.FadeTo(GameSetting.MainMenuName);
    }
}
