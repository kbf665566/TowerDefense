using System;
using TMPro;
using UnityEngine;

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

    private bool gameIsOver = false;
    [SerializeField] GameObject gameOverUI;
    public delegate void OnGameOver();
    public static OnGameOver onGameOver;


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
        ResetGame();
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
        money = startMoney;
        moneyText.text = money.ToString();
        live = startLive;
        liveText.text = live.ToString();
        gameIsOver = false;
        gameOverUI.SetActive(false);
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

    private void EndGame()
    {
        gameIsOver = true;
        gameOverUI.SetActive(true);
    }

    [ContextMenu("SetWayPoint")]
    public void SetWayPoint()
    {
        wayPoints = new Transform[wayPointParent.childCount];
        for (int i = 0; i < wayPoints.Length; i++)
            wayPoints[i] = wayPointParent.GetChild(i);
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
