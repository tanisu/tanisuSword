using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : MonoBehaviour
{
    Animator anim;
    int loopCount = 1;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "DemonWalkAnim")
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && loopCount % 3 != 0) {
                
                anim.SetBool("isDef", true);
                loopCount++;
                
            }
            else if(loopCount % 3 == 0)
            {
                anim.SetBool("isAtc", true);
                loopCount++;
            }
        } 
    }

    public void StartWalk()
    {
        anim.SetBool("isDef",false);
        anim.SetBool("isAtc", false);
    }

    void DeadStart()
    {
        
        SoundManager.I.PlaySE(SESoundData.SE.BOSS_EXPLOSION);
    }

    void Dead()
    {
        PlayerPrefs.SetInt("stageNumber", 1);
        
        StageController.I.BossDead();
    }
}
