using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// 切換語言時將同一群組的文字自動調整成同樣大小
/// </summary>
public class TMPTextGroup : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI[] texts;
    [SerializeField] protected LanguageText[] languageTexts;

    private bool firstShow = true;
    private LanguageType textLanguage = LanguageType.tw;

    private void TextAutoSize()
    {
        if (firstShow == false && textLanguage == GameSetting.GameLanguage)
            return;

        for (int i = 0; i < languageTexts.Length; i++)
            languageTexts[i].ChangeLanguage();

        if (texts.Length > 1)
        {
            int candidateIndex = 0;
            float maxPreferredWidth = 0;

            for (int i = 0; i < texts.Length; i++)
            {
                float preferredWidth = texts[i].preferredWidth;
                if (preferredWidth > maxPreferredWidth)
                {
                    maxPreferredWidth = preferredWidth;
                    candidateIndex = i;
                }
            }

            texts[candidateIndex].enableAutoSizing = true;
            texts[candidateIndex].ForceMeshUpdate();
            float optimumPointSize = texts[candidateIndex].fontSize;

            texts[candidateIndex].enableAutoSizing = false;

            for (int i = 0; i < texts.Length; i++)
                texts[i].fontSize = optimumPointSize;
        }
        else
        {
            texts[0].enableAutoSizing = true;
            texts[0].ForceMeshUpdate();
            texts[0].enableAutoSizing = false;
        }
        textLanguage = GameSetting.GameLanguage;
        firstShow = false;
    }

    private void LanguageChanged(object s, GameEvent.ChangeLanguageEvent e)
    {
        TextAutoSize();
    }

    private void OnEnable()
    {
        TextAutoSize();
        EventHelper.LanguageChangedEvent += LanguageChanged;
    }

    private void OnDisable()
    {
        EventHelper.LanguageChangedEvent -= LanguageChanged;
    }

    public void SetLanguageText()
    {
        languageTexts = new LanguageText[texts.Length];
        for (int i = 0; i < languageTexts.Length; i++)
            languageTexts[i] = texts[i].GetComponent<LanguageText>();
    }
}
