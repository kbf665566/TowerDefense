using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer gameMixer;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    // Start is called before the first frame update
    void Start()
    {
        
    }

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
    }

    public void SetBGMAudio(float amount)
    {
        gameMixer.SetFloat("BGM", amount);
    }

    public void SetTowerAudio(float amount)
    {
        gameMixer.SetFloat("Tower", amount);
    }

    public void SetEnemyAudio(float amount)
    {
        gameMixer.SetFloat("Enemy", amount);
    }

    public void SetUIAudio(float amount)
    {
        gameMixer.SetFloat("UI", amount);
    }

    public void ChangeResolution(int index)
    {
        var fullScreen = Screen.fullScreen;
        if (index == 0)
        {
            Screen.SetResolution(800,600, fullScreen);
        }
        else if (index == 1)
        {
            Screen.SetResolution(1024, 768, fullScreen);
        }
        else if (index == 2)
        {
            Screen.SetResolution(1280, 960, fullScreen);
        }
        else if (index == 3)
        {
            Screen.SetResolution(1920, 1080, fullScreen);
        }
    }
}
