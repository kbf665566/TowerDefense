using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private Image mapIcon;
    private int mapId;
    [SerializeField] private Button btn;
    public Button Btn => btn;

    public void SetButton(Sprite icon,int id)
    {
        mapIcon.sprite = icon;
        mapId = id;
    }
}
