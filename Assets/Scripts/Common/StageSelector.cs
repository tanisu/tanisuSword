using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelector : MonoBehaviour
{
    private int stageNumber;

    [SerializeField] GameObject[] stageButton;


    void Start()
    {
        stageNumber = PlayerPrefs.GetInt("stageNumber");
        _checkClearStages();
        SoundManager.I.PlayBGM(BGMSoundData.BGM.TITLE);
    }

    private void _checkClearStages()
    {
        for(int i = 0; i < stageButton.Length; i++)
        {
            Text stageText = stageButton[i].GetComponentInChildren<Text>();
            int stage = stageButton[i].GetComponentInChildren<StageBtn>().stage;
            stageText.text += stage.ToString();
            stageButton[i].GetComponent<Button>().onClick.AddListener(() => { SelectStage(stage); });
            if (stageNumber >= i)
            {
                stageButton[i].GetComponent<Button>().enabled = true;
            }
            else
            {
                stageButton[i].GetComponent<Image>().color = new Color(1,1,1,0.5f);
            }
        }
    }

    private void SelectStage(int stage)
    {
        SceneManager.LoadScene($"stage0{stage}");
    }
}
