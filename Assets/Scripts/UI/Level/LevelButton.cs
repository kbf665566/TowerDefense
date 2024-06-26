using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private Image mapIcon;
    [SerializeField] private Image trophyIcon;
    [SerializeField] private Sprite[] trophySprites;
    private int mapId;
    [SerializeField] private Button btn;
    public Button Btn => btn;

    public void SetButton(Sprite icon,int id)
    {
        mapIcon.sprite = icon;
        mapId = id;
        SetTrophy();
    }

    private void SetTrophy()
    {
        var data = GameManager.instance.GetPlayerLevelData(mapId);
        if (data == null)
        {
            trophyIcon.gameObject.SetActive(false);
            return;
        }
        bool winMap = false;
        for (int i = (int)DifficultyType.Hard; i >= (int)DifficultyType.Easy; i--)
        {
            if (data.LevelDifficulty[i].Win == true)
            {
                trophyIcon.sprite = trophySprites[i];
                winMap = true;
                break;
            }
        }

        trophyIcon.gameObject.SetActive(winMap);
    }
}
