using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    private Node target;
    [SerializeField] private Vector3 offset = new Vector3(0,.5f,0);

    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [SerializeField] private TextMeshProUGUI sellPriceText;
    [SerializeField] private Button upgradeBtn;

    [SerializeField] private GameObject menu;

    [SerializeField] private GameObject upgradeAndSellMenu;

    [SerializeField] private GameObject buildMenu;
    [SerializeField] private RectTransform buildMenuRect;
    [SerializeField] private BuildMenuItem buildMenuItemObj;
    [SerializeField] private Transform buildMenuItemParent;
    private int nowPageIndex = 0;

    [SerializeField] private GameObject confirmMenu;
    private Vector3 buildMenuOriginPos;

    private BuildManager buildManager => BuildManager.instance;
    private GameManager gameManager => GameManager.instance;

    private void Awake()
    {
        upgradeAndSellMenu.SetActive(false);
        buildMenu.SetActive(false);
        menu.SetActive(false);
        confirmMenu.SetActive(false);
        buildMenuOriginPos = buildMenu.transform.position;

        var towerList = gameManager.TowerData.TowerList;
        for (int i =0;i< towerList.Count; i++)
        {
            var buildBtn = Instantiate(buildMenuItemObj);
            buildBtn.transform.SetParent(buildMenuItemParent,false);
            int i2 = i;
            buildBtn.SetItem(towerList[i].Id, towerList[i].TowerIcon, towerList[i].TowerLevelData[0].BuildUpgradeCost,() => PrereviwBuildTower(towerList[i2].Id));
        }
    }

    public void SetTarget(Node targetNode)
    {
        if (target == targetNode)
        {
            target = null;
            return;
        }

        target = targetNode;
        transform.position = target.GetBuildPos(offset);
        if (!target.IsUpgrade)
        {
            upgradeCostText.text = "$" + target.TurretBlueprint.UpgradeCost;
            upgradeBtn.interactable = true;
        }
        else
        {
            upgradeCostText.text = "Done";
            upgradeBtn.interactable = false;
        }
       
        sellPriceText.text = "$" + target.TurretBlueprint.GetSellPirce();
       
        //ui.SetActive(true);
    }

    public void SelectNode(object s,GameEvent.NodeSelectEvent e)
    {
       var gridState = buildManager.GetGridState(e.GridPos);
        menu.transform.position = new Vector3(e.GridPos.x,5f,e.GridPos.y);
        if (gridState == GridState.Building)
        {
            upgradeAndSellMenu.SetActive(true);
        }
        else if(gridState == GridState.Empty)
        {
            buildMenu.SetActive(true);
        }

    }

    public void PrereviwBuildTower(int towerId)
    {
        var towerData = gameManager.TowerData.GetData(towerId);
        if(towerData != null)
        {

        }
    }

    public void Hide()
    {
        //ui.SetActive(false);
    }

    public void Upgrade()
    {
        target.UpgradeTurret();
        BuildManager.instance.DeselectNode();
    }

    public void Sell()
    {
        target.SellTurret();
        BuildManager.instance.DeselectNode();
        Hide();
    }

    public void CancelClick()
    {
        upgradeAndSellMenu.SetActive(false);
        buildMenu.SetActive(false);
        menu.SetActive(false);
        confirmMenu.SetActive(false);
        EventHelper.NodeCancelSelectedEvent.Invoke(this, GameEvent.NodeCancelSelectEvent.CreateEvent());
    }

    private void OnDisable()
    {
        BuildManager.nodeSelected -= SetTarget;
        BuildManager.nodeCancelSelected -= Hide;
    }
    private void OnEnable()
    {
        EventHelper.NodeSelectedEvent += SelectNode;
    }
}
