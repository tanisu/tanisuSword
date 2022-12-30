using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;



public class UIController : MonoBehaviour
{

    public Slider hpS,expS;
    [SerializeField] GameObject gameOverPanel,stageClearPanel,stageStartPanel;
    [SerializeField] Text currentHpText,maxHpText,levelText ;
    [SerializeField] Button hpButton;
    private void Start()
    {
        gameOverPanel.SetActive(false);
    }
    public void UpdateSlider(int hp)
    {
        if (StageController.I.isStop)
        {
            DOTween.To(() => hpS.value, val => hpS.value = val, hp, 1.0f).SetLink(gameObject).OnComplete(() =>
            {
                _updateCurrentHpText(hp);
                StageController.I.ReScroll();
            });
        }
        else
        {
            _updateCurrentHpText(hp);
            hpS.value = hp;

            
        }
    }

    private void _updateCurrentHpText(int _hp)
    {
        if (_hp <= 0)
        {
            currentHpText.text = "0";
        }
        else
        {
            currentHpText.text = _hp.ToString();
        }
    }

    public void UpdateLevelUI(Dictionary<string,int> _levelParams)
    {
        
        hpS.maxValue = _levelParams["maxHp"];
        hpS.value = _levelParams["currentHp"];
        currentHpText.text = _levelParams["currentHp"].ToString();
        maxHpText.text = $"/ {_levelParams["maxHp"]}";
        levelText.text = $"LV: {_levelParams["level"]}";
        expS.maxValue = _levelParams["nextExp"];
        expS.value = _levelParams["currentExp"];
    }

    public void InitLevelUI(Dictionary<string, int> _levelParams)
    {

        hpS.maxValue = _levelParams["maxHp"];
        hpS.value = _levelParams["maxHp"];
        currentHpText.text = _levelParams["maxHp"].ToString();
        maxHpText.text = $"/ {_levelParams["maxHp"]}";
        levelText.text = $"LV: {_levelParams["level"]}";
        expS.maxValue = _levelParams["nextExp"];
        expS.value = _levelParams["currentExp"];
    }


    public void UpdateExpSlider(int _exp)
    {
        expS.value += _exp;
    }


    public void ViewGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    public void ViewStageClearPanel()
    {
        stageClearPanel.SetActive(true);
    }

    public void HideGameOverPanel()
    {
        gameOverPanel.SetActive(false);
    }

    public void HideStartPanel()
    {
        stageStartPanel.SetActive(false);
    }

    public void HpButtonFalse()
    {
        hpButton.interactable = false;
    }
}
