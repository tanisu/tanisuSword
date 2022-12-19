using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GirlController : MonoBehaviour
{
    Animator anim;
    public UnityAction endChange;


    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ChangeAnim()
    {
        StartCoroutine(_startAnim());
        
    }

    public void KubiSe()
    {
        SoundManager.I.PlaySE(SESoundData.SE.STAFFGROUND);
    }

    public void BodySe()
    {
        SoundManager.I.PlaySE(SESoundData.SE.ATTACK);
    }
    
    IEnumerator _startAnim()
    {
        SoundManager.I.StopBGM();
        yield return new WaitForSeconds(1f);
        
        anim.SetBool("GirlChange", true);
    }

    public void EndChange()
    {
        endChange?.Invoke();
    }
}
