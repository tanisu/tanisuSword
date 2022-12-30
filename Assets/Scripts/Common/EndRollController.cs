using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EndRollController : MonoBehaviour
{

    [SerializeField] GameObject characterPanel;
    [SerializeField] Transform staffPanel;
    
    void Start()
    {
        SoundManager.I.PlayBGM(BGMSoundData.BGM.TITLE);
        characterPanel.GetComponent<CharacterPanelController>().MoveUp = _moveCharactorPanel;
    }


    void _moveCharactorPanel()
    {
        characterPanel.transform.DOLocalMoveY(1200f, 5f).SetLink(characterPanel).OnComplete(()=> {

            staffPanel.DOLocalMoveY(500f, 15f).SetLink(staffPanel.gameObject);
        });
    }
    
}
