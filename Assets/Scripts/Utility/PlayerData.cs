using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData 
{
    public PlayerSettingData SettingData;
    public PlayerGameLevelData GameLevelData;
}

[Serializable]
public class PlayerSettingData
{
    public float MasterAudio;
    public float BGMAudio;
    public float TowerAudio;
    public float EnemyAudio;
    public float UIAudio;
    public int ScreenWidth;
    public int ScreenHeight;
    public bool FullScreen;
}

[Serializable]
public class PlayerGameLevelData
{
    public List<GameLevelData> GameLevelDatas;
}

[Serializable]
public class GameLevelData
{
    public int LevelId;
    public LevelDifficultyData[] LevelDifficulty = new LevelDifficultyData[]
    { new LevelDifficultyData() {Difficulty = DifficultyType.Easy , Win = false },
    new LevelDifficultyData() {Difficulty = DifficultyType.Normal , Win = false },
    new LevelDifficultyData() {Difficulty = DifficultyType.Hard , Win = false }};
}


[Serializable]
public class LevelDifficultyData
{
    public DifficultyType Difficulty;
    public bool Win;
}

[Serializable]
public enum DifficultyType
{
    Easy = 0,
    Normal = 1,
    Hard = 2,
}