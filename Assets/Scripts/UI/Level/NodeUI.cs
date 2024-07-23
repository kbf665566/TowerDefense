using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class NodeUI : MonoBehaviour
{
    [SerializeField] GameObject cancelClickBtn;

    [SerializeField] GameObject towerInfoGroup;
    [SerializeField] TextMeshProUGUI towerNameText;
    [SerializeField] Image towerIcon;
    [SerializeField] VerticalLayoutGroup layoutGroup;

    [Header("AttackMode")]
    [SerializeField] private GameObject attackModeMenu;
    [SerializeField] private TextMeshProUGUI attackModeText;
    private int nowAttackModeIndex = 0;
    private TowerAttackMode nowCanSelectAttackMode;
    private TowerAttackMode nowSelectTowerAttackMode;
    private Array attackModeEnum = Enum.GetValues(typeof(TowerAttackMode));

    [Header("LockOn")]
    [SerializeField] private GameObject lockonMenu;
    [SerializeField] private GameObject lockonImage;
    private bool inSelectLockOn;
    private Vector3 lockonPos;
    private Vector3 originLockonPos;
    private RaycastHit raycastHit;
    private LayerMask environmentLayer => GameSetting.EnvironmentLayer;

    [Header("LevelInfo")]
    [SerializeField] private RectTransform levelInfoMenu;
    [SerializeField] private TextMeshProUGUI levelInfoText;

    [Header("Upgrade/Sell")]
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [SerializeField] private TextMeshProUGUI sellPriceText;
    [SerializeField] private Button upgradeBtn;

    private int selectTowerUid;

    private BuildManager buildManager => BuildManager.instance;
    private GameManager gameManager => GameManager.instance;
    private LevelManager levelManager => LevelManager.instance;

    private Camera mainCamera;

    private void Awake()
    {
        cancelClickBtn.SetActive(false);
        lockonImage.SetActive(false);
        towerInfoGroup.SetActive(false);

        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!inSelectLockOn)
            return;

        lockonImage.transform.position = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit, 1000f, environmentLayer))
            {
                lockonPos = raycastHit.point;
#if UNITY_EDITOR
                Debug.Log("LockOnPos:"+ raycastHit.point);
#endif
                lockonPos.y = 0;
                lockonPos.x = lockonPos.x < 0 ? 0 : lockonPos.x > gameManager.NowMapData.MapSize.x ? gameManager.NowMapData.MapSize.x : lockonPos.x;
                lockonPos.z = lockonPos.z < 0 ? 0 : lockonPos.z > gameManager.NowMapData.MapSize.y ? gameManager.NowMapData.MapSize.y : lockonPos.z;

                EventHelper.TowerSelectedSpecificPointEvent.Invoke(this, GameEvent.TowerSelectSpecificPointEvent.CreateEvent(selectTowerUid, lockonPos));
                inSelectLockOn = false;
                towerInfoGroup.SetActive(true);
                cancelClickBtn.SetActive(true);
            }
        }
        else if (Input.GetMouseButton(1))
        {
            CancelSelectLockOn();
        }
    }

    public void SelectTower(object s, GameEvent.TowerSelectEvent e)
    {
        var tower = buildManager.GetTower(e.Uid);
        selectTowerUid = e.Uid;
        if (tower != null)
        {
            towerIcon.sprite = tower.TowerData.TowerIcon;
            towerNameText.text = tower.TowerData.Name.GetLanguageValue();
            var nextData = tower.GetNextLevelData();
           
            if(nextData != null) 
            {
                //還能升級
                upgradeCostText.text = "$" + nextData.BuildUpgradeCost;
                upgradeBtn.interactable = levelManager.Money >= nextData.BuildUpgradeCost;
                levelInfoText.text = tower.TowerData.GetTowerLevelInfo(tower.NowLevel, true);
                levelInfoText.ForceMeshUpdate(true);
            }
            else
            {
                //升滿了
                upgradeCostText.text = "Done".GetLanguageValue();
            }
            upgradeBtn.interactable = nextData != null;
            levelInfoMenu.gameObject.SetActive(nextData != null);
            sellPriceText.text = "$" + tower.GetNowLevelData().SoldPrice;

            if (tower.TowerType == TowerType.Normal)
            {
                nowCanSelectAttackMode = tower.TowerData.AttackMode;
                nowSelectTowerAttackMode = tower.NowAttackMode;
                nowAttackModeIndex = (int)tower.NowAttackMode;
                attackModeText.text = tower.NowAttackMode.ToString().GetLanguageValue();
            }
            attackModeMenu.SetActive(tower.TowerType == TowerType.Normal);

            if (nowCanSelectAttackMode.HasFlag(TowerAttackMode.SpecificPoint) && tower is IAttackTower attackTower)
            {
                var imagePos = mainCamera.WorldToScreenPoint(attackTower.LockOnPos);
                lockonImage.transform.position = imagePos;
                originLockonPos = imagePos;
            }

            lockonImage.SetActive(tower.NowAttackMode == TowerAttackMode.SpecificPoint);
            lockonMenu.SetActive(tower.NowAttackMode == TowerAttackMode.SpecificPoint);

            StartCoroutine(WaitShowUI());
        }
    }

    /// <summary>
    /// 切換塔的攻擊模式
    /// </summary>
    public void PrevAttackMode()
    {
        do
        {
            nowAttackModeIndex >>= 1;
            if (nowAttackModeIndex <= 0)
                nowAttackModeIndex = (int)attackModeEnum.GetValue(attackModeEnum.Length - 1);
        } while (!nowCanSelectAttackMode.HasFlag((TowerAttackMode)nowAttackModeIndex));

        nowSelectTowerAttackMode = (TowerAttackMode)nowAttackModeIndex;
        attackModeText.text = nowSelectTowerAttackMode.ToString().GetLanguageValue();

        lockonImage.SetActive(nowSelectTowerAttackMode == TowerAttackMode.SpecificPoint);
        lockonMenu.SetActive(nowSelectTowerAttackMode == TowerAttackMode.SpecificPoint);
        EventHelper.TowerChangedAttackModeEvent.Invoke(this,GameEvent.TowerChangeAttackModeEvent.CreateEvent(selectTowerUid, nowSelectTowerAttackMode));
    }
    /// <summary>
    /// 切換塔的攻擊模式
    /// </summary>
    public void NextAttackMode()
    {
        do
        {
            nowAttackModeIndex <<= 1;
            if(nowAttackModeIndex > (int)attackModeEnum.GetValue(attackModeEnum.Length - 1))
                nowAttackModeIndex = 1;
        } while (!nowCanSelectAttackMode.HasFlag((TowerAttackMode)nowAttackModeIndex));


        nowSelectTowerAttackMode = (TowerAttackMode)nowAttackModeIndex;
        attackModeText.text = nowSelectTowerAttackMode.ToString().GetLanguageValue();

        lockonImage.SetActive(nowSelectTowerAttackMode == TowerAttackMode.SpecificPoint);
        lockonMenu.SetActive(nowSelectTowerAttackMode == TowerAttackMode.SpecificPoint);
        EventHelper.TowerChangedAttackModeEvent.Invoke(this, GameEvent.TowerChangeAttackModeEvent.CreateEvent(selectTowerUid, nowSelectTowerAttackMode));
    }


    /// <summary>
    /// 避免還沒出現就被關掉
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitShowUI()
    {
        layoutGroup.enabled = false;
        yield return new WaitForEndOfFrame();
        cancelClickBtn.SetActive(true);
        if (levelInfoMenu.gameObject.activeSelf)
            levelInfoMenu.sizeDelta = new Vector2(levelInfoMenu.sizeDelta.x, levelInfoText.renderedHeight + 55f);
        layoutGroup.enabled = true;
        towerInfoGroup.SetActive(true);
    }


    public void Hide()
    {
        selectTowerUid = 0;
        cancelClickBtn.SetActive(false);
        towerInfoGroup.SetActive(false);
        lockonImage.SetActive(false);
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
        if (inSelectLockOn)
            return;

        Hide();
        EventHelper.NodeCancelSelectedEvent.Invoke(this, GameEvent.NodeCancelSelectEvent.CreateEvent());
    }

    public void SelectLockOn()
    {
        inSelectLockOn = true;
        towerInfoGroup.SetActive(false);
        cancelClickBtn.SetActive(false);
    }

    public void CancelSelectLockOn()
    {
        inSelectLockOn = false;
        towerInfoGroup.SetActive(true);
        cancelClickBtn.SetActive(true);
        lockonImage.transform.position = originLockonPos;
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
