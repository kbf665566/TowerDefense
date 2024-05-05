using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private Transform levelBtnParent;
    [SerializeField] private Button[] levelBtns;

    private void Start()
    {
        int levelReached = PlayerPrefs.GetInt("levelReached",1);

        for (int i = 0; i < levelBtns.Length; i++)
        {
            if(i + 1 > levelReached)
            levelBtns[i].interactable = false;
        }
    }

    public void Select()
    {

    }

    public void StartLevel()
    {
        //Call SceneEvent
    }

    public void ClearData()
    {
        PlayerPrefs.DeleteKey("levelReached");
    }

    [ContextMenu("SetBtn")]
    public void SetBtn()
    {
        levelBtns = levelBtnParent.GetComponentsInChildren<Button>();
    }
}
