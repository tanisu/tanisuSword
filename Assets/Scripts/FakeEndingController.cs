using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FakeEndingController : MonoBehaviour
{

    Camera mainCam;
    [SerializeField] float camSize, yPos, dulation;
    [SerializeField] GameObject player,girl;
    [SerializeField] SpriteRenderer bgColor;
    

    void Start()
    {
        SoundManager.I.PlayBGM(BGMSoundData.BGM.FAKEEND);
        mainCam = Camera.main;
        mainCam.DOOrthoSize(camSize, dulation);
        mainCam.transform.DOLocalMoveY(yPos, dulation).OnComplete(() => { 
            
        });
        player.transform.DOLocalMoveY(-0.5f, dulation);
        player.transform.DOScale(3.5f, dulation);
    }

    
   
}
