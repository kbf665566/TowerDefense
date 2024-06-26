using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer gameMixer;
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private GameManager gameManager => GameManager.instance;
    public void OnEnable()
    {
        if(Screen.width == 800 && Screen.height == 600)
        {
            resolutionDropdown.value = 0;
        }
        else if (Screen.width == 1024 && Screen.height == 768)
        {
            resolutionDropdown.value = 1;
        }
        else if (Screen.width == 1280 && Screen.height == 960)
        {
            resolutionDropdown.value = 2;
        }
        else if (Screen.width == 1920 && Screen.height == 1080)
        {
            resolutionDropdown.value = 3;
        }
    }

    public void SetMasterAudio(float amount)
    {
        gameMixer.SetFloat("Master",amount);
        gameManager.GameData.SettingData.MasterAudio = amount;
    }

    public void SetBGMAudio(float amount)
    {
        gameMixer.SetFloat("BGM", amount);
        gameManager.GameData.SettingData.BGMAudio = amount;
    }

    public void SetTowerAudio(float amount)
    {
        gameMixer.SetFloat("Tower", amount);
        gameManager.GameData.SettingData.TowerAudio = amount;
    }

    public void SetEnemyAudio(float amount)
    {
        gameMixer.SetFloat("Enemy", amount);
        gameManager.GameData.SettingData.EnemyAudio = amount;
    }

    public void SetUIAudio(float amount)
    {
        gameMixer.SetFloat("UI", amount);
        gameManager.GameData.SettingData.UIAudio = amount;
    }

    public void ChangeResolution(int index)
    {
        var fullScreen = Screen.fullScreen;
        if (index == 0)
        {
            Screen.SetResolution(800,600, fullScreen);
            gameManager.GameData.SettingData.ScreenWidth = 800;
            gameManager.GameData.SettingData.ScreenHeight = 600;
        }
        else if (index == 1)
        {
            Screen.SetResolution(1024, 768, fullScreen);
            gameManager.GameData.SettingData.ScreenWidth = 1024;
            gameManager.GameData.SettingData.ScreenHeight = 768;
        }
        else if (index == 2)
        {
            Screen.SetResolution(1280, 960, fullScreen);
            gameManager.GameData.SettingData.ScreenWidth = 1280;
            gameManager.GameData.SettingData.ScreenHeight = 960;
        }
        else if (index == 3)
        {
            Screen.SetResolution(1920, 1080, fullScreen);
            gameManager.GameData.SettingData.ScreenWidth = 1920;
            gameManager.GameData.SettingData.ScreenHeight = 1080;
        }
        gameManager.GameData.SettingData.FullScreen = fullScreen;
    }
}
