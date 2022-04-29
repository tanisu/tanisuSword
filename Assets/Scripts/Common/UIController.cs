using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;



public class UIController : MonoBehaviour
{

    public Slider s;
    [SerializeField] GameObject gameOverPanel,stageClearPanel,stageStartPanel;
    [SerializeField] Text testText;

    private void Start()
    {
        gameOverPanel.SetActive(false);
    }
    public void UpdateSlider(int hp)
    {
        if (StageController.I.isStop)
        {
            DOTween.To(()=>s.value,val=>s.value = val,hp,1.0f).OnComplete(()=>{
                StageController.I.ReScroll();
            });
        }
        else
        {
            s.value = hp;
        }
        
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

    public void TestText()
    {
        testText.text = "stage";
    }
}
