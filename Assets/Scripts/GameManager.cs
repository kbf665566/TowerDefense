using System;
using TMPro;
using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.Audio;
using DG.Tweening;
using UnityEngine.Networking.Types;
/// <summary>
/// 管理整個遊戲
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance;



    public Maps MapData;
    public LevelData LevelData;
    public Towers TowerData;
    public Enemies EnemyData;
    public Languages LanguagesData;

    private MapData nowMapData;
    public MapData NowMapData => nowMapData;

    private LevelAllWaves nowWaveData;
    public LevelAllWaves NowWaveData => nowWaveData;

    private bool disableClicked = false;
    public bool DisableClicked => disableClicked;

    [SerializeField] private AudioMixer gameMixer;

    private PlayerData gameData;
    public PlayerData GameData => gameData;

    private DifficultyType difficulty;
    public DifficultyType Difficulty => difficulty;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("有多個GameManager");
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
        LoadData();
    }

    private void Update()
    {
#if UNITY_EDITOR
        //if (Input.GetKeyDown(KeyCode.E))
        //    onGameOver?.Invoke();
#endif

    }


    public MapData SelectMap(int id,DifficultyType difficulty)
    {
        nowMapData = MapData.GetData(id);
        this.difficulty = difficulty;
        nowWaveData = LevelData.GetData(nowMapData.WavesId);
        return nowMapData;
    }

    private void LoadData()
    {
       var data = SaveLoadHelper.Load();
        if(data == null)
        {
            gameData = new PlayerData();
            gameData.SettingData = new PlayerSettingData
            {
                MasterAudio = 0,
                BGMAudio = 0,
                TowerAudio = 0,
                EnemyAudio = 0,
                UIAudio = 0,
                ScreenWidth = 1920,
                ScreenHeight = 1080,
                FullScreen = false,
                NowLanguage = LanguageType.tw
            };

            gameData.GameLevelData = new PlayerGameLevelData();
            gameData.GameLevelData.GameLevelDatas = new List<GameLevelData>();
            for(int i =0;i<MapData.MapDataList.Count;i++)
            {
                var levelData = new GameLevelData();
                levelData.MapId = MapData.MapDataList[i].Id;
                gameData.GameLevelData.GameLevelDatas.Add(levelData);
            }
        }
        else
        {
            gameData = new PlayerData();
            gameData.SettingData = data.SettingData;
            gameData.GameLevelData = data.GameLevelData;

            gameMixer.SetFloat("Master", data.SettingData.MasterAudio);
            gameMixer.SetFloat("BGM", data.SettingData.BGMAudio);
            gameMixer.SetFloat("Tower", data.SettingData.TowerAudio);
            gameMixer.SetFloat("Enemy", data.SettingData.EnemyAudio);
            gameMixer.SetFloat("UI", data.SettingData.UIAudio);

            gameData.SettingData.NowLanguage = data.SettingData.NowLanguage;

            Screen.SetResolution(data.SettingData.ScreenWidth, data.SettingData.ScreenHeight, data.SettingData.FullScreen);
        }
    }

    private void OnApplicationQuit()
    {
        //SaveLoadHelper.Save(GameData);
    }

    public GameLevelData GetPlayerLevelData(int mapId)
    {
        for(int i =0;i< gameData.GameLevelData.GameLevelDatas.Count;i++)
        {
            if (gameData.GameLevelData.GameLevelDatas[i].MapId == mapId)
                return gameData.GameLevelData.GameLevelDatas[i];
        }

        return null;
    }

    public void SavePlayerWinData()
    {
        for (int i = 0; i < gameData.GameLevelData.GameLevelDatas.Count; i++)
        {
            if (gameData.GameLevelData.GameLevelDatas[i].MapId == nowMapData.Id)
            {
                gameData.GameLevelData.GameLevelDatas[i].LevelDifficulty[(int)difficulty].Win = true;
                break;
            }
        }
    }
}