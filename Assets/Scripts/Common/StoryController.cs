using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
public class StoryController : MonoBehaviour
{
    [SerializeField] Button nextButton, titleButton;
    [SerializeField] Transform storiesTf;
    public  Ease easeType;
    const float SCROLLY = 660f;
    float fadeValue = 0.1f;
    float fadeDuration = 1.3f;
    
    public UnityAction ToTitle;
    void Start()
    {
        nextButton.onClick.AddListener(() =>  scrollStory());
        titleButton.onClick.AddListener(()=> returnTitle());
        nextButton.GetComponent<CanvasGroup>().DOFade(fadeValue, fadeDuration).SetEase(easeType).SetLoops(-1, LoopType.Yoyo).SetLink(nextButton.gameObject);
        titleButton.GetComponent<CanvasGroup>().DOFade(fadeValue, fadeDuration).SetEase(easeType).SetLoops(-1, LoopType.Yoyo).SetLink(nextButton.gameObject);
    }

    public void ShowButtons()
    {
        nextButton.gameObject.SetActive(true);
        titleButton.gameObject.SetActive(true);
    }

    void scrollStory()
    {
        if (storiesTf.localPosition.y > 1000) return;
        nextButton.gameObject.SetActive(false);
        titleButton.gameObject.SetActive(false);
        storiesTf.DOLocalMoveY(storiesTf.localPosition.y + SCROLLY,8f).OnComplete(()=> {
            if(storiesTf.localPosition.y < 1000)
            {
                nextButton.gameObject.SetActive(true);
            }
            
            titleButton.gameObject.SetActive(true);
        });
    }
    void returnTitle()
    {
        ToTitle?.Invoke();
    }

    public void ResetStoryPos()
    {
        storiesTf.localPosition = Vector3.zero;
        nextButton.gameObject.SetActive(false);
        titleButton.gameObject.SetActive(false);
    }
    
}
