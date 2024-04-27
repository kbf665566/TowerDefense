using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "TowerDefense/Tower Data")]
public class Towers : ScriptableObject
{
    public List<TowerData> TowerList = new List<TowerData>();
}
