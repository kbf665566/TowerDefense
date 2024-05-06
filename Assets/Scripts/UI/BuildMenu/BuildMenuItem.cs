using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class BuildMenuItem : MonoBehaviour,IPointerExitHandler,IPointerMoveHandler
{
    [SerializeField] private Image turretImage;
    [SerializeField] private TextMeshProUGUI costText;
    private float interval = 10;
    private void Start()
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //發出隱藏Tip事件
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
            //發出顯示Tip事件
        }
    }
}
