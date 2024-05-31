using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using DG.Tweening;
using TMPro;

public class GameEffectWithText : MonoBehaviour,IGameEffect
{
    private IObjectPool<IGameEffect> objectPool;
    [SerializeField] private TextMeshPro textMesh;
    private Color originColor;
    [SerializeField] private float duration = 1.5f;
    private WaitForSeconds waitSecond = new WaitForSeconds(1.5f);
    public void StartEffect(string text)
    {
        originColor = textMesh.color;
        textMesh.text = text;
        transform.DOMoveY(5f, duration);
        textMesh.DOColor(new Color(originColor.r, originColor.g, originColor.b,0),duration);
        StartCoroutine(StartHide());
    }

    private IEnumerator StartHide()
    {
        yield return waitSecond;
        Hide();
    }

    public void Hide()
    {
        textMesh.color = originColor;
        objectPool.Release(this);
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void SetObjectPool(IObjectPool<IGameEffect> objectPool)
    {
        this.objectPool = objectPool; ;
    }
}
