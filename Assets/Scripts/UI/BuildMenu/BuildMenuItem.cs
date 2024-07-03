using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class BuildMenuItem : MonoBehaviour,IPointerExitHandler,IPointerMoveHandler
{
    [SerializeField] private Image towerImage;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button btn;
    private int towerId;
    private TowerData towerData;
    private float interval = 5;

    private string towerName;
    private string towerInfo;

    public void SetItem(int id,Sprite towerIcon,int cost, UnityAction clickAction)
    {
        towerId = id;
        towerData = GameManager.instance.TowerData.GetData(towerId);
        towerName = towerData.Name.GetLanguageValue();
        towerInfo = towerData.TowerInformation.GetLanguageValue() + towerData.GetTowerLevelInfo(0);
        towerImage.sprite = towerIcon;
        costText.text = cost.ToString();
        btn.onClick.AddListener(clickAction);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //發出隱藏Tip事件
        EventHelper.TipHideEvent.Invoke(this, GameEvent.HideTipEvent.CreateEvent());
    }

    public void SetCost(int cost)
    {
        costText.text = "$" + cost;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        //減低更新位置的頻率
        if (Time.frameCount % interval == 0)
        {
            EventHelper.TipShowEvent.Invoke(this, GameEvent.ShowTipEvent.CreateEvent(towerName, towerInfo));
        }
    }

    private void OnEnable()
    {
        if (towerData != null)
            btn.interactable = LevelManager.instance.Money >= towerData.TowerLevelData[0].BuildUpgradeCost;
    }
}
