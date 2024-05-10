using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "TowerDefense/Tower Data")]
public class Towers : ScriptableObject
{
    public List<TowerData> TowerList = new List<TowerData>();

    public TowerData GetData(int id)
    {
        for (int i = 0; i < TowerList.Count; i++)
        {
            if (TowerList[i].Id == id)
                return TowerList[i];
        }
        return null;
    }
}
