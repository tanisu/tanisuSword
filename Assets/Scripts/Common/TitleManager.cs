using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleManager : MonoBehaviour
{
    [SerializeField] Transform sword, logo;
    [SerializeField] GameObject stageSelectPanel,settingButton;
    
    void Start()
    {
        Sequence sq = DOTween.Sequence();
        sq.Append(sword.DOLocalMoveY(10f, 10f).SetLink(sword.gameObject))
            .Join(logo.DOLocalMoveY(100f, 10f).SetLink(logo.gameObject).OnComplete(() => {
                stageSelectPanel.SetActive(true);
                settingButton.SetActive(true);
            }));
        GetComponent<Button>().onClick.AddListener(() => {
            sq.Kill();
            sword.localPosition = new Vector3(0f, 10f);
            logo.localPosition = new Vector3(0f, 100f);
            stageSelectPanel.SetActive(true);
            settingButton.SetActive(true);
        });
    }


}
