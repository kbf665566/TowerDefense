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
    private DifficultyType nowSelectDifficulty = DifficultyType.Easy;
    private MapData tempMapData;
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

        SelectLevel(gameManager.MapData.MapDataList[0].Id);
    }

    public void SelectLevel(int mapId)
    {
        tempMapData = gameManager.MapData.GetData(mapId);
        nowSelectId = mapId;
        if (tempMapData != null)
            UpdataMapInformation(tempMapData.StartLive, tempMapData.StartMoney);
    }

    private void UpdataMapInformation(int startLive, int startMoney)
    {
        mapIcon.sprite = tempMapData.MapIcon;
        mapNameText.text = tempMapData.MapName;
        mapInformationText.text = "SartLive".GetLanguageValue() + startLive + " \n" + "StartMoney".GetLanguageValue() + startMoney;
    }

    public void SelectEasyDifficulty(bool e)
    {
        nowSelectDifficulty = DifficultyType.Easy;
        UpdataMapInformation(tempMapData.StartLive, (int)(tempMapData.StartMoney * GameSetting.EasyMoneyRatio));
    }

    public void SelectNormalDifficulty(bool n)
    {
        nowSelectDifficulty = DifficultyType.Normal;
        UpdataMapInformation((int)(tempMapData.StartLive * GameSetting.NormalLiveRatio), (int)(tempMapData.StartMoney * GameSetting.NormalMoneyRatio));
    }

    public void SelectHardDifficulty(bool h)
    {
        nowSelectDifficulty = DifficultyType.Hard;
        UpdataMapInformation(GameSetting.HardLive, (int)(tempMapData.StartMoney * GameSetting.HardMoneyRatio));
    }

    public void StartLevel()
    {
        var mapData = gameManager.SelectMap(nowSelectId,nowSelectDifficulty);
        EventHelper.SceneChangedEvent.Invoke(this, GameEvent.SceneChangeEvent.CreateEvent(mapData.SceneName));
    }

    public void ClearData()
    {
        PlayerPrefs.DeleteKey("levelReached");
    }
}
