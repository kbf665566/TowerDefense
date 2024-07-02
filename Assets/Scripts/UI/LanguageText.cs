using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LanguageText : MonoBehaviour
{
    [SerializeField] private string key;
    [SerializeField] private TextMeshProUGUI text;

    public void ChangeLanguage()
    {
        if (!string.IsNullOrEmpty(key))
        {
            var value = key.GetLanguageValue();
            if (!text.text.Equals(value))
            {
                text.text = key.GetLanguageValue();
                text.font = GameManager.instance.FontAssets[(int)GameSetting.GameLanguage];
            }
        }
    }
}
