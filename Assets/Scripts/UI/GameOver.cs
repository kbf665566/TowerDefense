using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour
{
    private string menuSceneName = "MainMenu";
    [SerializeField] private string levelName = "Level";
    public void Retry()
    {
        SceneManager.LoadScene(levelName);
    }

    public void Menu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
