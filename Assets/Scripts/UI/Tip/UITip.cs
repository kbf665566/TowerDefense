using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI informationText;
    [SerializeField] private RectTransform rect;
    [SerializeField] private GameObject context;
    private RectTransform parentRect;
    //邊界
    private Vector2 margin = new Vector2(50, 50);
    void Start()
    {
        context.SetActive(false);
        if (parentRect == null)
            transform.parent.GetComponent<RectTransform>();
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
        UIExtension.ClampToWindow(rect, parentRect, margin);
        context.SetActive(true);
    }

    
}
