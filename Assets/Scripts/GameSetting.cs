

using UnityEngine;

public class GameSetting
{
    public const string MainMenuName = "MainMenu";
    public static LayerMask EnemyLayer = 1 << LayerMask.NameToLayer("Enemy");
    public const float EasyMoneyRatio = 1f;
    public const float NormalMoneyRatio = 0.75f;
    public const float HardMoneyRatio = 0.5f;

    public const float NormalLiveRatio = 0.5f;
    public const int HardLive = 1;

    public static LanguageType GameLanguage = LanguageType.tw;
}
