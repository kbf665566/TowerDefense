using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI informationText;
    [SerializeField] private RectTransform rect;
    //邊界
    private Vector2 margin = new Vector2(50, 50);
    void Start()
    {
       gameObject.SetActive(false);
    }

    public void SetTip(Vector3 targetPos,string title,string information)
    {
        transform.position = targetPos;
        if (!(titleText.text.Equals(title) && informationText.text.Equals(information)))
        {
            titleText.text = title;
            informationText.text = information;
            informationText.ForceMeshUpdate(true);
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, informationText.renderedHeight + 30f);
        }
        ClampToWindow(rect,transform.parent.GetComponent<RectTransform>());
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 計算面板不超過區域
    /// </summary>
    private void ClampToWindow(RectTransform panelRectTransform, RectTransform parentRectTransform)
    {
        var parentRect = parentRectTransform.rect;
        var panelRect = panelRectTransform.rect;
        Vector2 minPosition = parentRect.min + margin - panelRect.min;
        Vector2 maxPosition = parentRect.max - margin - panelRect.max;

        Vector3 pos = panelRectTransform.localPosition;
        pos.x = Mathf.Clamp(panelRectTransform.localPosition.x, minPosition.x, maxPosition.x);
        pos.y = Mathf.Clamp(panelRectTransform.localPosition.y, minPosition.y, maxPosition.y);

        panelRectTransform.localPosition = pos;
    }
}
