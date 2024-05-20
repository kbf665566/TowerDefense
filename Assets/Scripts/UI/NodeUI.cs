using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(0,.5f,0);
    [SerializeField] private Vector2 margin = new Vector2(50,50);

    [SerializeField] private RectTransform rect;
    private RectTransform parentRect;

    [SerializeField] GameObject cancelClickBtn;

    [Header("Upgrade/Sell")]
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [SerializeField] private TextMeshProUGUI sellPriceText;
    [SerializeField] private Button upgradeBtn;
    [SerializeField] private GameObject upgradeAndSellMenu;


    [Header("Build")]
    [SerializeField] private GameObject buildMenu;
    [SerializeField] private BuildMenuItem buildMenuItemObj;
    [SerializeField] private Transform buildMenuItemParent;

    private int selectTowerId;
    private Vector2Short selectGridPos;
    private int selectTowerUid;

    [Header("Confirm")]
    [SerializeField] private GameObject confirmMenu;
    [SerializeField] private Button yesToBuildBtn;

    private BuildManager buildManager => BuildManager.instance;
    private GameManager gameManager => GameManager.instance;
    private LevelManager levelManager => LevelManager.instance;

    private void Awake()
    {
        upgradeAndSellMenu.SetActive(false);
        buildMenu.SetActive(false);
        cancelClickBtn.SetActive(false);
        confirmMenu.SetActive(false);

        parentRect = transform.GetTopParent().GetComponent<RectTransform>();

        var towerList = gameManager.TowerData.TowerList;
        for (int i =0;i< towerList.Count; i++)
        {
            var buildBtn = Instantiate(buildMenuItemObj);
            buildBtn.transform.SetParent(buildMenuItemParent,false);
            int i2 = i;
            buildBtn.SetItem(towerList[i].Id, towerList[i].TowerIcon, towerList[i].TowerLevelData[0].BuildUpgradeCost,() => PrereviwBuildTower(towerList[i2].Id));
        }
    }


    public void SelectTower(object s, GameEvent.TowerSelectEvent e)
    {
        var tower = buildManager.GetTower(e.Uid);
        selectTowerUid = e.Uid;
        if (tower != null)
        {
            var worldPos = GridExtension.GetCenterGrid(tower.GridPos, tower.TowerData.TowerSize).ToWorldPos() + new Vector3(0, 2.5f, 0);
            ClameWindow(worldPos, tower.GridPos);

            var nextData = tower.GetNextLevelData();
           
            if(nextData != null) 
            {
                //還能升級
                upgradeCostText.text = "$" + nextData.BuildUpgradeCost;
                upgradeBtn.interactable = levelManager.Money >= nextData.BuildUpgradeCost;
            }
            else
            {
                //升滿了
                upgradeCostText.text = "Done";
                upgradeBtn.interactable = false;
            }
            sellPriceText.text = "$" + tower.GetNowLevelData().SoldPrice;

            upgradeAndSellMenu.SetActive(true);
            StartCoroutine(WaitShowUI());
        }
    }

    public void SelectNode(object s,GameEvent.NodeSelectEvent e)
    {
        selectGridPos = e.GridPos;

#if UNITY_EDITOR
        var gridState = buildManager.GetGridState(e.GridPos);
        Debug.Log("gridpos:" + e.GridPos + " state:" + gridState);
#endif
        var worldPos = e.GridPos.ToWorldPos() + new Vector3(0, 2.5f, 0);
        ClameWindow(worldPos, e.GridPos);

        buildMenu.SetActive(true);


        StartCoroutine(WaitShowUI());
    }
    private void ClameWindow(Vector3 worldPos,Vector2Short gridPos)
    {
        transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPos);
        var nodePos = RectTransformUtility.WorldToScreenPoint(Camera.main, gridPos.ToWorldPosCorner());

        UIExtension.ClampToWindow(rect, parentRect, margin);

        //判斷有沒有跟Node重疊到
        bool contains = RectTransformUtility.RectangleContainsScreenPoint(rect, nodePos);
        if (contains)
        {
            var newPos = worldPos - new Vector3(0, 0, 6f);
            transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, newPos);
        }
    }

    /// <summary>
    /// 避免還沒出現就被關掉
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitShowUI()
    {
        yield return null;
        cancelClickBtn.SetActive(true);
    }

    public void PrereviwBuildTower(int towerId)
    {
        var towerData = gameManager.TowerData.GetData(towerId);
        if (towerData != null)
        {
            selectTowerId = towerId;
            EventHelper.TowerPreviewBuiltEvent.Invoke(this, GameEvent.TowerPreviewBuildEvent.CreateEvent(towerData.TowerSize, selectGridPos));
            EventHelper.TipHideEvent.Invoke(this, GameEvent.HideTipEvent.CreateEvent());
            yesToBuildBtn.interactable = buildManager.CheckCanBuild(towerData.TowerSize, selectGridPos);
            buildMenu.SetActive(false);
            confirmMenu.SetActive(true);
        }
    }

    public void YesToBuild()
    {
        EventHelper.TowerBuiltEvent.Invoke(this,GameEvent.TowerBuildEvent.CreateEvent(selectTowerId, selectGridPos));
        confirmMenu.SetActive(false);
        cancelClickBtn.SetActive(false);
        selectTowerId = -1;
        selectGridPos = Vector2Short.Hide;
    }

    public void CancelBuild()
    {
        EventHelper.TowerCanceledPreviewEvent.Invoke(this, GameEvent.TowerCancelPreviewEvent.CreateEvent(selectGridPos));
        confirmMenu.SetActive(false);
        buildMenu.SetActive(true);
    }

    public void Hide()
    {
        selectTowerId = -1;
        selectGridPos = Vector2Short.Hide;
        selectTowerUid = 0;
        upgradeAndSellMenu.SetActive(false);
        buildMenu.SetActive(false);
        cancelClickBtn.SetActive(false);
        confirmMenu.SetActive(false);
    }

    public void Upgrade()
    {
        EventHelper.TowerUpgradedEvent.Invoke(this, GameEvent.TowerUpgradeEvent.CreateEvent(selectTowerUid));
        Hide();
    }

    public void Sell()
    {
        EventHelper.TowerSoldEvent.Invoke(this, GameEvent.TowerSellEvent.CreateEvent(selectTowerUid));
        Hide();
    }

    public void CancelClick()
    {
        Hide();
        EventHelper.NodeCancelSelectedEvent.Invoke(this, GameEvent.NodeCancelSelectEvent.CreateEvent());
    }

    private void OnDisable()
    {
        EventHelper.NodeSelectedEvent -= SelectNode;
        EventHelper.TowerSelectedEvent -= SelectTower;
    }
    private void OnEnable()
    {
        EventHelper.NodeSelectedEvent += SelectNode;
        EventHelper.TowerSelectedEvent += SelectTower;
    }
}
