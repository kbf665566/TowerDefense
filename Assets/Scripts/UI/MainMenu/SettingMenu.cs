using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer gameMixer;
    [SerializeField] private TMP_Dropdown resolutionDropdown;

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
        GameManager.GameData.SettingData.MasterAudio = amount;
    }

    public void SetBGMAudio(float amount)
    {
        gameMixer.SetFloat("BGM", amount);
        GameManager.GameData.SettingData.BGMAudio = amount;
    }

    public void SetTowerAudio(float amount)
    {
        gameMixer.SetFloat("Tower", amount);
        GameManager.GameData.SettingData.TowerAudio = amount;
    }

    public void SetEnemyAudio(float amount)
    {
        gameMixer.SetFloat("Enemy", amount);
        GameManager.GameData.SettingData.EnemyAudio = amount;
    }

    public void SetUIAudio(float amount)
    {
        gameMixer.SetFloat("UI", amount);
        GameManager.GameData.SettingData.UIAudio = amount;
    }

    public void ChangeResolution(int index)
    {
        var fullScreen = Screen.fullScreen;
        if (index == 0)
        {
            Screen.SetResolution(800,600, fullScreen);
            GameManager.GameData.SettingData.ScreenWidth = 800;
            GameManager.GameData.SettingData.ScreenHeight = 600;
        }
        else if (index == 1)
        {
            Screen.SetResolution(1024, 768, fullScreen);
            GameManager.GameData.SettingData.ScreenWidth = 1024;
            GameManager.GameData.SettingData.ScreenHeight = 768;
        }
        else if (index == 2)
        {
            Screen.SetResolution(1280, 960, fullScreen);
            GameManager.GameData.SettingData.ScreenWidth = 1280;
            GameManager.GameData.SettingData.ScreenHeight = 960;
        }
        else if (index == 3)
        {
            Screen.SetResolution(1920, 1080, fullScreen);
            GameManager.GameData.SettingData.ScreenWidth = 1920;
            GameManager.GameData.SettingData.ScreenHeight = 1080;
        }
        GameManager.GameData.SettingData.FullScreen = fullScreen;
    }
}
