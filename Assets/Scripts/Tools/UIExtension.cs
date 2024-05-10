using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIExtension
{
    /// <summary>
    /// 計算面板不超過區域
    /// </summary>
    public static void ClampToWindow(RectTransform panelRectTransform, RectTransform parentRectTransform, Vector2 margin)
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
