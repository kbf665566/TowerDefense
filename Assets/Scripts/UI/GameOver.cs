using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour
{
    [SerializeField] private SceneFader sceneFader;
    [SerializeField] private string levelName = "Level1";
    public void Retry()
    {
        sceneFader.FadeTo(levelName);
    }

    public void Menu()
    {
        sceneFader.FadeTo(GameSetting.MainMenuName);
    }

    //IEnumerator AnimationText()
    //{
    //    roundText.text = "0";
    //    int round = 0;
    //    yield return new WaitForSeconds(0.7f);

    //    while(round < 20)
    //    {
    //        round++;
    //        roundText.text = round.ToString();
    //        yield return new WaitForSeconds(0.05f);
    //    }
    //}
}
