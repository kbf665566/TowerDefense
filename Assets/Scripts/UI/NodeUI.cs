using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(0,2.5f,0);
    [SerializeField] private Vector2 margin = new Vector2(50,50);

    [SerializeField] private Vector3 tipOffset = new Vector3(400f,0,0);

    [SerializeField] private Vector3 nodeContainsOffset = new Vector3(0,0,4f);

    [SerializeField] private RectTransform rect;
    private RectTransform parentRect;

    [SerializeField] GameObject cancelClickBtn;

    [Header("Upgrade/Sell")]
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [SerializeField] private TextMeshProUGUI sellPriceText;
    [SerializeField] private Button upgradeBtn;
    [SerializeField] private GameObject upgradeAndSellMenu;

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
        cancelClickBtn.SetActive(false);
        confirmMenu.SetActive(false);

        parentRect = transform.GetTopParent().GetComponent<RectTransform>();
    }


    public void SelectTower(object s, GameEvent.TowerSelectEvent e)
    {
        var tower = buildManager.GetTower(e.Uid);
        selectTowerUid = e.Uid;
        if (tower != null)
        {
            var worldPos = GridExtension.GetCenterGrid(tower.GridPos, tower.TowerData.TowerSize).ToWorldPos() + offset;
            ClameWindow(worldPos, tower.GridPos);

            var nextData = tower.GetNextLevelData();
           
            if(nextData != null) 
            {
                //還能升級
                upgradeCostText.text = "$" + nextData.BuildUpgradeCost;
                upgradeBtn.interactable = levelManager.Money >= nextData.BuildUpgradeCost;
                var towerName = tower.TowerData.Name.GetLanguageValue() + " " + "Upgrade".GetLanguageValue();
                var towerInfo = tower.TowerData.TowerInformation.GetLanguageValue() + tower.TowerData.GetTowerLevelInfo(tower.NowLevel, true);
                EventHelper.TipShowEvent.Invoke(this, GameEvent.ShowTipEvent.CreateEvent(transform.position - tipOffset, towerName, towerInfo));
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

    private void ClameWindow(Vector3 worldPos,Vector2Short gridPos)
    {
        transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPos);
        var nodePos = RectTransformUtility.WorldToScreenPoint(Camera.main, gridPos.ToWorldPosCorner());

        UIExtension.ClampToWindow(rect, parentRect, margin);

        //判斷有沒有跟Node重疊到
        bool contains = RectTransformUtility.RectangleContainsScreenPoint(rect, nodePos);
        if (contains)
        {
            var newPos = worldPos - nodeContainsOffset;
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


    public void Hide()
    {
        EventHelper.TipHideEvent.Invoke(this,GameEvent.HideTipEvent.CreateEvent());
        selectTowerUid = 0;
        upgradeAndSellMenu.SetActive(false);
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
        EventHelper.TowerSelectedEvent -= SelectTower;
    }
    private void OnEnable()
    {
        EventHelper.TowerSelectedEvent += SelectTower;
    }
}
