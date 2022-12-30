using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleManager : MonoBehaviour
{
    [SerializeField] Transform sword, logo,startObj,storyObj;
    [SerializeField] GameObject stageSelectPanel,settingButton;
    [SerializeField]StoryController story;
    [SerializeField] Button storyButton;
    bool isStory;
    void Start()
    {
        story.ToTitle = _toTitle;
        Sequence sq = DOTween.Sequence();
        sq.Append(sword.DOLocalMoveY(10f, 10f).SetLink(sword.gameObject))
            .Join(logo.DOLocalMoveY(200f, 10f).SetLink(logo.gameObject).OnComplete(() => {
                _showUI();
            }));
        GetComponent<Button>().onClick.AddListener(() => {
            sq.Kill();
            sword.localPosition = new Vector3(0f, 10f);
            logo.localPosition = new Vector3(0f, 200f);
                _showUI();
        });
        storyButton.onClick.AddListener(() => _moveToStory());
        //AdmobController.I.ShowBanner();
    }

    

    void _showUI()
    {
        stageSelectPanel.SetActive(true);
        settingButton.SetActive(true);
        storyButton.gameObject.SetActive(true);
        logo.GetComponent<Image>().DOColor(new Color(0.92f, 0.03f, 0.05f), 0.83f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutBounce).SetLink(logo.gameObject);
    }

    void _moveToStory()
    {
        startObj.DOLocalMoveX(360f, 3f).SetLink(startObj.gameObject).OnComplete(() => {
            story.ShowButtons();
        });
    }



    void _toTitle()
    {
        startObj.DOLocalMoveX(0, 3f).SetLink(startObj.gameObject).OnComplete(()=> story.ResetStoryPos());
    }
}
