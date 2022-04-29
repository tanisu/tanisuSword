using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : BossBase
{
    Animator anim;
    int loopCount = 1;
    new void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
    }

    
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





}
