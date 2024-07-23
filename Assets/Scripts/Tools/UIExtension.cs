using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIExtension
{
    /// <summary>
    /// 計算面板不超過區域
    /// </summary>
    public static void ClampToWindow(RectTransform panelRectTransform, RectTransform parentRectTransform, Vector2 margin)
    {
        var parentRect = parentRectTransform.rect;
        var panelRect = panelRectTransform.rect;
        Vector2 minPosition = parentRect.min + margin - panelRect.min;
        Vector2 maxPosition = parentRect.max - margin - panelRect.max;

        Vector3 pos = panelRectTransform.localPosition;
        pos.x = Mathf.Clamp(panelRectTransform.localPosition.x, minPosition.x, maxPosition.x);
        pos.y = Mathf.Clamp(panelRectTransform.localPosition.y, minPosition.y, maxPosition.y);

        panelRectTransform.localPosition = pos;
    }

    public static Transform GetTopParent(this Transform transform)
    {
        var temp = transform.parent;
        while(temp.parent != null)
        {
            temp = temp.parent;
        }
        return temp;
    }

    public static string GetLanguageValue(this string key)
    {
        return GameManager.instance.LanguagesData.GetLanguageValue(key);
    }

    public static string GetTowerLevelInfo(this TowerData towerData, int nowLevel, bool isUpdate = false)
    {
        var info = string.Empty;
        var levelData = towerData.TowerLevelData[nowLevel];
        TowerLevelData nextData = (nowLevel + 1) >= towerData.TowerLevelData.Count ? null : towerData.TowerLevelData[nowLevel + 1];
        var addValue = string.Empty;

        if (levelData.Damage != 0)
        {
            addValue = nextData != null && isUpdate ? "<color=#ff0000>  + " + Math.Round((nextData.Damage - levelData.Damage),3) +"</color>" : string.Empty;
            info += "Damage".GetLanguageValue() + ": " + levelData.Damage + addValue + "\n";
        }

        if (levelData.ShootRange != 0)
        {
            addValue = nextData != null && isUpdate ? "<color=#ff0000>  + " + Math.Round((nextData.ShootRange - levelData.ShootRange),3) + "</color>" : string.Empty;
            info += "ShootRange".GetLanguageValue() + ": " + levelData.ShootRange + addValue + "\n";
        }

        if (levelData.FireRate != 0 && towerData.towerType != TowerType.Money)
        {
            var dps = levelData.Damage / levelData.FireRate;
            addValue = nextData != null && isUpdate ? "<color=#ff0000>  + " + Math.Round(nextData.Damage / nextData.FireRate - dps) + "</color>" : string.Empty;
            info += "DPS: " + Math.Round(dps, 2) + addValue + "\n";
        }
        else if (towerData.towerType == TowerType.Money)
        {
            var dps = levelData.GetMoney / levelData.FireRate;
            addValue = nextData != null && isUpdate ? "<color=#ff0000>  + " + Math.Round(nextData.GetMoney / nextData.FireRate - dps) + "</color>" : string.Empty;
            info += "DPS: " + Math.Round(dps, 2) + addValue + "\n";
        }

        if (levelData.BulletExplosionRadius != 0)
        {
            addValue = nextData != null && isUpdate ? "<color=#ff0000>  + " + Math.Round((nextData.BulletExplosionRadius - levelData.BulletExplosionRadius),3) + "</color>" : string.Empty;
            info += "BulletExplosionRadius".GetLanguageValue() + ": " + levelData.BulletExplosionRadius + addValue + "\n";
        }

        if (levelData.BuffAddDamage != 0)
        {
            addValue = nextData != null && isUpdate ? "<color=#ff0000>  + " + Math.Round((nextData.BuffAddDamage - levelData.BuffAddDamage),3) + "</color>" : string.Empty;
            info += "BuffAddDamage".GetLanguageValue() + ": " + levelData.BuffAddDamage + addValue + "\n";
        }

        if (levelData.BuffAddRange != 0)
        {
            addValue = nextData != null && isUpdate ? "<color=#ff0000>  + " + Math.Round((nextData.BuffAddRange - levelData.BuffAddRange),3) + "</color>" : string.Empty;
            info += "BuffAddRange".GetLanguageValue() + ": " + levelData.BuffAddRange + addValue + "\n";
        }

        if (levelData.BuffAddFireRate != 0)
        {
            addValue = nextData != null && isUpdate ? "<color=#ff0000>  + " + Math.Round((nextData.BuffAddFireRate - levelData.BuffAddFireRate),3) + "</color>" : string.Empty;
            info += "BuffAddFireRate".GetLanguageValue() + ": " + levelData.BuffAddFireRate + addValue + "\n";
        }

        if (levelData.GetMoney != 0)
        {
            addValue = nextData != null && isUpdate ? "<color=#ff0000>  + " + (nextData.GetMoney - levelData.GetMoney) + "</color>" : string.Empty;
            info += "GetMoney".GetLanguageValue() + ": " + levelData.GetMoney + addValue + "\n";
        }

        if (levelData.SlowAmount != 0)
        {
            addValue = nextData != null && isUpdate ? "<color=#ff0000>  + " + Math.Round((nextData.SlowAmount - levelData.SlowAmount),3) + "</color>" : string.Empty;
            info += "SlowAmount".GetLanguageValue() + ": " + levelData.SlowAmount + addValue + "\n";
        }
        if (levelData.SlowDuration != 0)
        {
            addValue = nextData != null && isUpdate ? "<color=#ff0000>  + " + Math.Round((nextData.SlowDuration - levelData.SlowDuration),3) + "</color>" : string.Empty;
            info += "SlowDuration".GetLanguageValue() + ": " + levelData.SlowDuration + addValue + "\n";
        }

        if (levelData.StunProbability != 0)
        {
            addValue = nextData != null && isUpdate ? "<color=#ff0000>  + " + Math.Round((nextData.StunProbability - levelData.StunProbability),3) + "</color>" : string.Empty;
            info += "StunProbability".GetLanguageValue() + ": " + levelData.StunProbability + addValue + "\n";
        }
        if (levelData.StunDuration != 0)
        {
            addValue = nextData != null && isUpdate ? "<color=#ff0000>  + " + Math.Round((nextData.StunDuration - levelData.StunDuration),3) + "</color>" : string.Empty;
            info += "StunDuration".GetLanguageValue() + ": " + levelData.StunDuration + addValue + "\n";
        }

        return info;
    }
}
