using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : BossBase
{
    Animator anim;
    [SerializeField] GameObject[] thunders;
    
    [SerializeField]float[] xPos ;
    [SerializeField]float[] yPos ;
    int actionCount = 0;
    bool isThunder;
    Coroutine co;
    new void Start()
    {
        
        
        base.Start();
        anim = GetComponent<Animator>();
    }



    public void StartStaffSE()
    {
        SoundManager.I.PlaySE(SESoundData.SE.STAFFFALL);
    }
    public void EndStaffSE()
    {
        SoundManager.I.PlaySE(SESoundData.SE.STAFFGROUND);
    }



    public void EndShot()
    {


        actionCount++;
        if(actionCount % 2 == 0)
        {
            anim.SetTrigger("Thunder");
        }
        else
        {
            anim.SetTrigger("Default");
        }   
    }

    public void EndWarp()
    {
        anim.SetTrigger("Default");
    }


    public void Warp()
    {
        
        
        Vector2 warpPosition = new Vector2(Random.Range(xPos[0], xPos[1]), Random.Range(yPos[0], yPos[1]));
        
        
        StartCoroutine(_warp(warpPosition));
        
    }

    IEnumerator _warp(Vector2 _pos)
    {
        yield return new WaitForSeconds(0.1f);
        transform.localPosition = _pos;
        anim.SetTrigger("EndWarp");
    }

    public void AttackThunder()
    {
        if (!isThunder)
        {
           co = StartCoroutine(_thunderAttack());
        }
        
        
    }

    IEnumerator _thunderAttack()
    {
        isThunder = true;
        foreach(GameObject thunder in thunders)
        {
            thunder.transform.position = StageController.I.player.transform.position - (Vector3.up * 0.2f);
            thunder.SetActive(true);
            yield return new WaitForSeconds(0.32f);
        }
        
        isThunder = false;
        
        anim.SetTrigger("Warp");
    }


    protected override void DeadStart()
    {
        if(co != null)
        {
            StopCoroutine(co);
        }
        
        base.DeadStart();
        
    }

}
