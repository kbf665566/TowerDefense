using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LanguageData
{
    public int Id;
    public string Key;
    public string[] LanguageString = new string[] {"tw","en"};
}

[Serializable]
public enum LanguageType
{
    tw = 0,
    en = 1,
}