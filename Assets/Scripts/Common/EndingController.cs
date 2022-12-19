using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class EndingController : MonoBehaviour
{
    Camera cam;
    [SerializeField]float firstWait,duration;
    [SerializeField] EndingPlayer endingPlayer;
    Tween tw;

    void Start()
    {
        cam = Camera.main;
        StartCoroutine(_startEnding());
        endingPlayer.ToStaffRoll = _toStaffRoll;
        SoundManager.I.PlayBGM(BGMSoundData.BGM.SADENDING);
    }

    IEnumerator _startEnding()
    {
        yield return new WaitForSeconds(firstWait);
        Sequence sq = DOTween.Sequence();
        tw = sq.Append(cam.DOOrthoSize(2f, duration))
            .Join(cam.transform.DOMoveY(1.7f,duration))
            .OnComplete(()=> {
                endingPlayer.StartAnim();
            });
    }

    private void _toStaffRoll()
    {
        StartCoroutine(_staffRoll());
    }

    IEnumerator _staffRoll()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("StaffRoll");
    }
}
