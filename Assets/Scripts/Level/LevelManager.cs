using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

/// <summary>
/// 管理關卡
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] private Transform[] wayPoints1;
    [SerializeField] private Transform[] wayPoints2;
    public Transform[] WayPoints1 => wayPoints1;
    public Transform[] WayPoints2 => wayPoints2;

    #region Money
    [SerializeField] private int startMoney = 400;
    private int money;
    public int Money => money;
    [SerializeField] private TextMeshProUGUI moneyText;
    #endregion

    #region Live
    [SerializeField] private int startLive = 20;
    private int live;
    public int Live => live;
    [SerializeField] private TextMeshProUGUI liveText;
    #endregion
    private GameManager gameManager => GameManager.instance;

    #region Wave
    private int nowWave = 0;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private Button nextWaveBtn;
    private bool nowWaveEnd = true;
    public bool NextWaveEnd => nowWaveEnd;
    #endregion
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("有多個LevelManager");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        SetLevelSetting(gameManager.NowMapData.StartMoney,gameManager.NowMapData.StartLive);
        nextWaveBtn.onClick.AddListener(NextWave);
    }

    public void SetLevelSetting(int startMoney,int startLive)
    {
        this.startLive = startLive;
        this.startMoney = startMoney;

        ResetLevel();
    }

    public void CostMoney(int cost)
    {
        money -= cost;
        moneyText.text = money.ToString();
    }

    public void AddMoney(int add)
    {
        money += add;
        moneyText.text = money.ToString();
    }

    public void AddLive(int add)
    {
        live += add;
        liveText.text = live.ToString();
    }

    private void ResetLevel()
    {
        nowWave = 0;
        waveText.text = nowWave.ToString() + " / " + gameManager.NowWaveData.WaveList.Count;
        money = startMoney;
        moneyText.text = money.ToString();
        live = startLive;
        liveText.text = live.ToString();
        nowWaveEnd = true;
        nextWaveBtn.interactable = true;
    }

    public void NextWave()
    {
        nextWaveBtn.interactable = false;
        nowWaveEnd = false;
        nowWave++;
        waveText.text = nowWave.ToString() + " / " + gameManager.NowWaveData.WaveList.Count;

        EventHelper.NextWaveStartedEvent.Invoke(this,GameEvent.NextWaveStartEvent.CreateEvent(nowWave));
    }

    private void WaveEnd(object s, GameEvent.WaveEndEvent e)
    {
        if (nowWave == gameManager.NowWaveData.WaveList.Count)
        {
            WinTheGame();
            EventHelper.GameWonEvent.Invoke(this, GameEvent.GameWinEvent.CreateEvent());
        }
        else
            nextWaveBtn.interactable = true;

        nowWaveEnd = true;
    }

    private void EnemyDied(object s,GameEvent.EnemyDieEvent e)
    {
        money += e.Value;
        moneyText.text = money.ToString();
    }

    private void EnemyEndPath(object s, GameEvent.EnemyEndPathEvent e)
    {
        live = live - e.Damage <= 0 ? 0 : live - e.Damage;
        liveText.text = live.ToString();
        if (live <= 0)
            EventHelper.GameOverEvent.Invoke(this,GameEvent.GameOverEvent.CreateEvent());
    }

    private void MakeMoney(object s, GameEvent.TowerMakeMoneyEvent e)
    {
        money += e.GetMoney;
        moneyText.text = money.ToString();
    }


    private void WinTheGame()
    {
        //記錄獲勝的關卡
        // PlayerPrefs.SetInt("levelReached", levelToUnlock);
    }

    private void OnEnable()
    {
        EventHelper.WaveEndEvent += WaveEnd;
        EventHelper.EnemyDiedEvent += EnemyDied;
        EventHelper.EnemyEndPathEvent += EnemyEndPath;
        EventHelper.TowerModeMoneyEvent += MakeMoney;
    }

    private void OnDisable()
    {
        EventHelper.WaveEndEvent -= WaveEnd;
        EventHelper.EnemyDiedEvent -= EnemyDied;
        EventHelper.EnemyEndPathEvent -= EnemyEndPath;
        EventHelper.TowerModeMoneyEvent -= MakeMoney;
    }

#if UNITY_EDITOR
    public void SetUI(Transform levelUICanvas,Transform map,int enemyPathCount)
    {
        var uiContext = levelUICanvas.Find("LevelPlayerStatus").Find("Context");
        moneyText = uiContext.Find("Money").Find("Money").GetComponent<TextMeshProUGUI>();
        liveText = uiContext.Find("Live").Find("LiveCount").GetComponent<TextMeshProUGUI>();
        waveText = uiContext.Find("Wave").Find("WaveCount").GetComponent<TextMeshProUGUI>();
        nextWaveBtn = levelUICanvas.Find("NextWaveBtn").GetComponent<Button>();

        var waypointparent = map.Find("Waypoints0");
        wayPoints1 = new Transform[waypointparent.childCount];
        for (int i = 0; i < waypointparent.childCount; i++)
            wayPoints1[i] = waypointparent.GetChild(i);

        if(enemyPathCount == 2)
        {
            var waypoint2parent = map.Find("Waypoints1");
            wayPoints2 = new Transform[waypoint2parent.childCount];
            for (int i = 0; i < waypoint2parent.childCount; i++)
                wayPoints2[i] = waypoint2parent.GetChild(i);
        }

    }
#endif
}
