using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI informationText;
    [SerializeField] private RectTransform rect;
    [SerializeField] private GameObject context;
    private RectTransform parentRect;
    private WaitForEndOfFrame waitFrame;
    //邊界
    private Vector2 margin = new Vector2(50, 50);
    void Start()
    {
        context.SetActive(false);
        if (parentRect == null)
            parentRect = transform.GetTopParent().GetComponent<RectTransform>();
    }

    public void ShowTip(object s,GameEvent.ShowTipEvent e)
    {
        SetTip(Input.mousePosition,e.Title,e.Information);
    }

    public void HideTip(object s, GameEvent.HideTipEvent e)
    {
        context.SetActive(false);
    }

    public void SetTip(Vector3 targetPos,string title,string information)
    {
        transform.position = targetPos;
        if (!(titleText.text.Equals(title) && informationText.text.Equals(information)))
        {
            titleText.text = title;
            informationText.text = information;
            informationText.ForceMeshUpdate(true);
            StartCoroutine(UpdateUI());
        }

        UIExtension.ClampToWindow(rect, parentRect, margin);
        context.SetActive(true);
    }
    private IEnumerator UpdateUI()
    {
        yield return waitFrame;
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, informationText.renderedHeight + 30f);
    }

    private void OnEnable()
    {
        EventHelper.TipShowEvent += ShowTip;
        EventHelper.TipHideEvent += HideTip;
    }

    private void OnDisable()
    {
        EventHelper.TipShowEvent -= ShowTip;
        EventHelper.TipHideEvent -= HideTip;
    }
}
