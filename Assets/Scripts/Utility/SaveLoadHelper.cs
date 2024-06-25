using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadHelper
{
    public static void Save(PlayerData playerData)
    {
        string saveData = JsonUtility.ToJson(playerData);

        StreamWriter file = new StreamWriter(Path.Combine(Application.streamingAssetsPath,"GameData.sav"));
        file.Write(saveData);
        file.Close();
    }

    public static PlayerData Load()
    {
        if (File.Exists(Path.Combine(Application.streamingAssetsPath, "GameData.sav")) == false)
            return null;

        StreamReader file = new StreamReader(Path.Combine(Application.streamingAssetsPath, "GameData.sav"));
        string fileString = file.ReadToEnd();
        file.Close();

        PlayerData playerData = JsonUtility.FromJson<PlayerData>(fileString);
        return playerData;
    }
}
