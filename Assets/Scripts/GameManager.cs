using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] Transform wayPointParent;
    private Transform[] wayPoints;
    public Transform[] WayPoints
    { get 
        {
            if (wayPoints == null)
            {
                wayPoints = new Transform[wayPointParent.childCount];
                for (int i = 0; i < wayPoints.Length; i++)
                    wayPoints[i] = wayPointParent.GetChild(i);
            }
            return wayPoints;
        } 
    }

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
            Debug.LogError("¦³¦h­ÓGameManager");
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        money = startMoney;
        moneyText.text = money.ToString();
        live = startLive;
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
            EndGame();
    }

    public void AddLive(int add)
    {
        live += add;
        liveText.text = live.ToString();
    }

    private void EndGame()
    {

    }

    [ContextMenu("SetWayPoint")]
    public void SetWayPoint()
    {
        wayPoints = new Transform[wayPointParent.childCount];
        for (int i = 0; i < wayPoints.Length; i++)
            wayPoints[i] = wayPointParent.GetChild(i);
    }
}
