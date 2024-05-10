using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    private GameManager gameManager => GameManager.instance;

    #region LevelButton
    private List<LevelButton> levelBtns = new List<LevelButton>();
    [SerializeField] private LevelButton levelBtnPrefab;
    [SerializeField] private Transform btnScrollView;
    private bool btnSpawnFinish = false;
    #endregion

    [SerializeField] private Image mapIcon;
    [SerializeField] private TextMeshProUGUI mapNameText;
    [SerializeField] private TextMeshProUGUI mapInformationText;

    private int nowSelectId = 0;
    private void Start()
    {
        int levelReached = PlayerPrefs.GetInt("levelReached",1);

        if (!btnSpawnFinish)
        {
            for (int i = 0; i < gameManager.MapData.MapDataList.Count; i++)
            {
                var levelBtn = Instantiate(levelBtnPrefab);
                levelBtn.transform.SetParent(btnScrollView, false);
                levelBtn.SetButton(gameManager.MapData.MapDataList[i].MapIcon, gameManager.MapData.MapDataList[i].Id);
                int i2 = i;
                //在lambda運算式下，用for迴圈的i會變成只用最終結果
                levelBtn.Btn.onClick.AddListener(() =>SelectLevel(gameManager.MapData.MapDataList[i2].Id));
                levelBtns.Add(levelBtn);
            }
            btnSpawnFinish = true;
        }

        //for (int i = 0; i < levelBtns.Length; i++)
        //{
        //    if(i + 1 > levelReached)
        //    levelBtns[i].interactable = false;
        //}

        SelectLevel(gameManager.MapData.MapDataList[0].Id);
    }

    public void SelectLevel(int mapId)
    {
        var mapData = gameManager.MapData.GetData(mapId);
        if(mapData != null)
        {
            mapIcon.sprite = mapData.MapIcon;
            mapNameText.text = mapData.MapName;
            mapInformationText.text = "起始生命：" + mapData.StartLive +" \n起始資源："+mapData.StartMoney;
            nowSelectId = mapId;
        }
    }

    public void StartLevel()
    {
        var mapData = gameManager.SelectMap(nowSelectId);
        EventHelper.SceneChangedEvent.Invoke(this, GameEvent.SceneChangeEvent.CreateEvent(mapData.SceneName));
    }

    public void ClearData()
    {
        PlayerPrefs.DeleteKey("levelReached");
    }
}
