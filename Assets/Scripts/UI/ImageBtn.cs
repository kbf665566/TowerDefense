using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ImageBtn : MonoBehaviour,IPointerDownHandler
{
    public UnityEvent clickEvent;
    public void OnPointerDown(PointerEventData eventData)
    {
        clickEvent.Invoke();
    }
}
