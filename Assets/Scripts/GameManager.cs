using System;
using TMPro;
using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;
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

    private MapData nowMapData;
    public MapData NowMapData => nowMapData;

    private LevelAllWaves nowWaveData;
    public LevelAllWaves NowWaveData => nowWaveData;

    private bool disableClicked = false;
    public bool DisableClicked => disableClicked;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("有多個GameManager");
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
#if UNITY_EDITOR
        //if (Input.GetKeyDown(KeyCode.E))
        //    onGameOver?.Invoke();
#endif

    }


    public MapData SelectMap(int id)
    {
        nowMapData = MapData.GetData(id);
        nowWaveData = LevelData.GetData(nowMapData.WavesId);
        return nowMapData;
    }
}
