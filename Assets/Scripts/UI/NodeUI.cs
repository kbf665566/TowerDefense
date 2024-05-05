using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class NodeUI : MonoBehaviour
{
    private Node target;
    [SerializeField] private Vector3 offset = new Vector3(0,.5f,0);
    [SerializeField] private GameObject ui;

    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [SerializeField] private TextMeshProUGUI sellPriceText;
    [SerializeField] private Button upgradeBtn;
    private void Awake()
    {
        ui.SetActive(false);
    }
    private void Start()
    {
        BuildManager.nodeSelected += SetTarget;
        BuildManager.nodeCancelSelected += Hide;
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
       
        ui.SetActive(true);
    }

    public void Hide()
    {
        ui.SetActive(false);
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

    private void OnDisable()
    {
        BuildManager.nodeSelected -= SetTarget;
        BuildManager.nodeCancelSelected -= Hide;
    }
}
