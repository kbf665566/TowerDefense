using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip menuMusic;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!scene.name.Equals(GameSetting.MainMenuName))
        {
            audioSource.clip = GameManager.instance.NowMapData.MapMusic;
        }
        else
        {
            audioSource.clip = menuMusic;
        }
        audioSource.Play();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
