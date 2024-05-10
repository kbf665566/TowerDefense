using System;
using TMPro;
using UnityEngine;
using System.IO;
/// <summary>
/// 管理整個遊戲
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    private bool gameIsOver = false;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject gameWinUI;
    public delegate void OnGameOver();
    public static OnGameOver onGameOver;

    public Maps MapData;
    public LevelData LevelData;
    public Towers TowerData;
    public Enemies EnemyData;

    private MapData nowMapData;
    public MapData NowMapData => nowMapData;

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

    // Start is called before the first frame update
    void Start()
    {
       
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.E))
            onGameOver?.Invoke();
#endif
    }

    private void ResetGame()
    {
        gameIsOver = false;
        gameOverUI.SetActive(false);
    }


    private void EndGame()
    {
        gameIsOver = true;
        gameOverUI.SetActive(true);
    }

    private void WinGame()
    {
        gameIsOver = true;
        gameWinUI.SetActive(true);
    }

    public MapData SelectMap(int id)
    {
        nowMapData = MapData.GetData(id);
        return nowMapData;
    }

    private void OnEnable()
    {
        onGameOver += EndGame;
    }
    private void OnDisable()
    {
        onGameOver -= EndGame;
    }
}
