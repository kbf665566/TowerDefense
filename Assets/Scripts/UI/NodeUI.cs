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

    [Header("Confirm")]
    [SerializeField] private GameObject confirmMenu;
    [SerializeField] private Button yesToBuildBtn;

    private BuildManager buildManager => BuildManager.instance;
    private GameManager gameManager => GameManager.instance;


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

    private void Update()
    {
    }

    public void SetTarget(Node targetNode)
    {
        //if (target == targetNode)
        //{
        //    target = null;
        //    return;
        //}

        //target = targetNode;
        //transform.position = target.GetBuildPos(offset);
        //if (!target.IsUpgrade)
        //{
        //    upgradeCostText.text = "$" + target.TurretBlueprint.UpgradeCost;
        //    upgradeBtn.interactable = true;
        //}
        //else
        //{
        //    upgradeCostText.text = "Done";
        //    upgradeBtn.interactable = false;
        //}
       
        //sellPriceText.text = "$" + target.TurretBlueprint.GetSellPirce();
       
        //ui.SetActive(true);
    }

    public void SelectNode(object s,GameEvent.NodeSelectEvent e)
    {

        selectGridPos = e.GridPos;
        var gridState = buildManager.GetGridState(e.GridPos);

#if UNITY_EDITOR
        Debug.Log("gridpos:" + e.GridPos + " state:" + gridState);
#endif

        var worldPos = e.GridPos.ToWorldPosCorner() + new Vector3(0,2.5f,0);
        transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPos);
        var nodePos = RectTransformUtility.WorldToScreenPoint(Camera.main, e.GridPos.ToWorldPosCorner());

        UIExtension.ClampToWindow(rect, parentRect, margin);

        //判斷有沒有跟Node重疊到
        bool contains = RectTransformUtility.RectangleContainsScreenPoint(rect, nodePos);
        if(contains)
        {
            var newPos = worldPos - new Vector3(0,0,6f);
            transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, newPos);
        }


        if (gridState == GridState.Building)
        {
            //取得塔

            upgradeAndSellMenu.SetActive(true);
        }
        else if(gridState == GridState.Empty)
        {
            
            buildMenu.SetActive(true);
        }

       

        StartCoroutine(WaitShowUI());
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
        EventHelper.TowerCanceledPreviewEvent.Invoke(this, GameEvent.TowerCancelPreviewEvent.CreateEvent());
        confirmMenu.SetActive(false);
        buildMenu.SetActive(true);
    }

    public void Hide()
    {
        
    }

    public void Upgrade()
    {
        //target.UpgradeTurret();
        BuildManager.instance.DeselectNode();
    }

    public void Sell()
    {
        BuildManager.instance.DeselectNode();
        Hide();
    }

    public void CancelClick()
    {
        selectTowerId = -1;
        selectGridPos = Vector2Short.Hide;
        upgradeAndSellMenu.SetActive(false);
        buildMenu.SetActive(false);
        cancelClickBtn.SetActive(false);
        confirmMenu.SetActive(false);
        EventHelper.NodeCancelSelectedEvent.Invoke(this, GameEvent.NodeCancelSelectEvent.CreateEvent());
    }

    private void OnDisable()
    {
        EventHelper.NodeSelectedEvent -= SelectNode;
    }
    private void OnEnable()
    {
        EventHelper.NodeSelectedEvent += SelectNode;
    }
}
