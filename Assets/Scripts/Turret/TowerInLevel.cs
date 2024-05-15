using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInLevel : MonoBehaviour
{
    private int id;
    public int Id => id;
    protected int uid;
    public int Uid => uid;
    protected string towerName;
    public string Name => towerName;
    protected List<TowerLevelData> towerLevelData;
    protected TowerType towerType;
    public TowerType TowerType => towerType;
    protected int nowLevel;
    public int NowLevel => nowLevel;
    protected TowerData towerData;

    /// <summary>
    /// 建造塔的時候設定資料
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="towerData"></param>
    public virtual void SetTower(int uid,TowerData towerData)
    {
        this.uid = uid;
        id = towerData.Id;
        towerName = towerData.Name;
        nowLevel = 0;
        this.towerData = towerData;
        towerLevelData = towerData.TowerLevelData;
    }

    public virtual void LevelUp()
    {
        nowLevel += 1;
    }

    public virtual void SetLevelData(int level)
    {

    }

    public TowerLevelData GetNextLevelData()
    {
        if (nowLevel + 1 >= towerLevelData.Count)
            return null;

        return towerLevelData[nowLevel + 1];
    }

    public TowerLevelData GetNowLevelData()
    {
        return towerLevelData[nowLevel];
    }

    /// <summary>
    /// 透過輔助塔增加能力
    /// </summary>
    /// <param name="addDamage"></param>
    /// <param name="addRange"></param>
    /// <param name="addFireRate"></param>
    public virtual void TowerGetSupport(float addDamage, float addRange, float addFireRate)
    {

    }

    /// <summary>
    /// 賣掉輔助塔的時候用來重置狀態
    /// </summary>
    public virtual void ResetTowerSupport()
    {

    }

    /// <summary>
    /// 更新塔的能力
    /// </summary>
    public virtual void UpdateTowerState()
    {

    }
}
