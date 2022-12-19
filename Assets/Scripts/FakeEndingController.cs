using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FakeEndingController : MonoBehaviour
{

    Camera mainCam;
    [SerializeField] float camSize, yPos, dulation;
    [SerializeField] GameObject player,LastBoss;
    [SerializeField] GirlController girl;
    [SerializeField] SpriteRenderer bgColor, girlmap;
    [SerializeField] Sprite girlBody;
    Tween tw;

    void Start()
    {
        girl.endChange = _endZoom;
        SoundManager.I.LoopSwitch();
        SoundManager.I.PlayBGM(BGMSoundData.BGM.FAKEEND);
        mainCam = Camera.main;
        tw =  mainCam.DOOrthoSize(camSize, dulation).SetLink(mainCam.gameObject);
        tw = mainCam.transform.DOLocalMoveY(yPos, dulation).OnComplete(() => {
            girl.ChangeAnim();
        }).SetLink(mainCam.gameObject);
        player.transform.DOLocalMoveY(-0.5f, dulation);
        player.transform.DOScale(3.5f, dulation);
    }

    private void _endZoom()
    {

        StartCoroutine(_viewGame());
    }
    
   IEnumerator _viewGame()
    {
        tw.Kill();
        StageController.I.LastBattle();
        yield return new WaitForSeconds(0.3f);
        mainCam.transform.localPosition = new Vector3(0, 0, -10f);
        mainCam.orthographicSize = 5f;
        girlmap.sprite = girlBody;
        girlmap.transform.Rotate(0,0,-90f);
        
        SoundManager.I.PlayBGM(BGMSoundData.BGM.LASTBOSS);
        SoundManager.I.LoopSwitch();
        StageController.I.canShoot = true;
        LastBoss.SetActive(true);
        gameObject.SetActive(false);
    }

}
