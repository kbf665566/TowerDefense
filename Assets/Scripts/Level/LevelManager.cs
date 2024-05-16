using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GameManager;

/// <summary>
/// 管理關卡
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    private Transform[][] wayPoints;
    public Transform[][] WayPoints => wayPoints;


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
    }

    public void SetLevelSetting(int startMoney,int startLive)
    {
        this.startLive = startLive;
        this.startMoney = startMoney;

        money = startMoney;
        live = startLive;

        moneyText.text = money.ToString();
        liveText.text = live.ToString();
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

    public void LostLive(int lost)
    {
        live = live - lost <= 0 ? 0 : live - lost;
        liveText.text = live.ToString();
        if (live <= 0)
            onGameOver?.Invoke();
    }

    public void AddLive(int add)
    {
        live += add;
        liveText.text = live.ToString();
    }

    private void ResetLevel()
    {
        money = startMoney;
        moneyText.text = money.ToString();
        live = startLive;
        liveText.text = live.ToString();
    }


#if UNITY_EDITOR
    public void SetUI(Transform levelUICanvas,Transform map,int enemyPathCount)
    {
        var uiContext = levelUICanvas.Find("LevelPlayerStatus").Find("Context");
        moneyText = uiContext.Find("Money").Find("Money").GetComponent<TextMeshProUGUI>();
        liveText = uiContext.Find("Live").Find("LiveCount").GetComponent<TextMeshProUGUI>();

        wayPoints = new Transform[enemyPathCount][];
        for(int i =0;i<enemyPathCount;i++)
        {
            var waypointparent = map.Find("Waypoints" + i);
            wayPoints[i] = new Transform[waypointparent.childCount];
            for (int j = 0;j<waypointparent.childCount;j++)
            {
                wayPoints[i][j] = waypointparent.GetChild(j);
            }
        }
    }
#endif
}
