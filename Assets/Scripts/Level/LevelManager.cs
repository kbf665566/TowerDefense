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

    [SerializeField] Transform wayPointParent;
    private Transform[] wayPoints;
    public Transform[] WayPoints => wayPoints;


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

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("有多個GameManager");
            return;
        }
        instance = this;
    }

    public void SetLevelSetting(int startMoney,int startLive)
    {
        this.startLive = startLive;
        this.startMoney = startMoney;
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
}
