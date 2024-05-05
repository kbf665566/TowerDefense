using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject selectLevelMenu;
    [SerializeField] private GameObject settingMenu;
    [SerializeField] private GameObject exitMenu;
    

    public void ShowSelectLevel()
    {
        selectLevelMenu.SetActive(true);
    }

    public void ShowSetting()
    {
        settingMenu.SetActive(true);
    }

    public void ShowExit()
    {
        exitMenu.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
