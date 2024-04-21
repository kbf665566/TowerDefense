using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] public int startMoney = 400;
    private int money;
    public int Money => money;

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
    }

    public void CostMoney(int cost)
    {
        money -= cost;
    }

    public void AddMoney(int add)
    {
        money += add;
    }
}
