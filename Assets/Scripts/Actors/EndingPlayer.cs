using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class EndingPlayer : MonoBehaviour
{
    Animator anim;
    [SerializeField] GameObject tanisSword,cover;

    public UnityAction ToStaffRoll;
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.enabled = false;
    }

    public void StartAnim()
    {
        anim.enabled = true;
    }

    public void GetSword()
    {
        SoundManager.I.PlaySE(SESoundData.SE.ATTACK);
        tanisSword.SetActive(false);
    }

    public void ViewCover()
    {
        cover.SetActive(true);
    }

    public void DeadMe()
    {
        SoundManager.I.StopBGM();
        SoundManager.I.PlaySE(SESoundData.SE.DAMAGE);
        ToStaffRoll?.Invoke();
    }

}
