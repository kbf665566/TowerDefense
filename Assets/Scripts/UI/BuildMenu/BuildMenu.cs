using UnityEngine;

public class BuildMenu : MonoBehaviour
{
    [SerializeField] private BuildMenuItem buildMenuItemObj;
    [SerializeField] private Transform buildMenuItemParent;

    private GameManager gameManager => GameManager.instance;

    void Start()
    {
        var towerList = gameManager.TowerData.TowerList;
        for (int i = 0; i < towerList.Count; i++)
        {
            var buildBtn = Instantiate(buildMenuItemObj);
            buildBtn.transform.SetParent(buildMenuItemParent, false);
            int i2 = i;
            buildBtn.SetItem(towerList[i].Id, towerList[i].TowerIcon, towerList[i].TowerLevelData[0].BuildUpgradeCost, () => PrereviwBuildTower(towerList[i2].Id));
        }
    }

    public void PrereviwBuildTower(int towerId)
    {
        var towerData = gameManager.TowerData.GetData(towerId);
        if (towerData != null)
        {
            EventHelper.TowerPreviewBuiltEvent.Invoke(this, GameEvent.TowerPreviewBuildEvent.CreateEvent(towerId,towerData.TowerLevelData[0].ShootRange, towerData.TowerSize, Vector2Short.Hide));
            EventHelper.TipHideEvent.Invoke(this, GameEvent.HideTipEvent.CreateEvent());
        }
    }
}
