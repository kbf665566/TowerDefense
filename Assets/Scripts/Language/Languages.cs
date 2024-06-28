using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "TowerDefense/Language Data")]
public class Languages : ScriptableObject
{
    public List<LanguageData> LanguageDataList = new List<LanguageData>();


    public string GetLanguageValue(string key)
    {
        for(int i =0;i<LanguageDataList.Count;i++)
        {
            if (LanguageDataList[i].Key.Equals(key))
                return LanguageDataList[i].LanguageString[(int)GameSetting.GameLanguage];
        }

        return "";
    }
}