using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;



public class UIController : MonoBehaviour
{

    public Slider s;
    
    
    public void UpdateSlider(int hp)
    {
        if (StageController.I.isStop)
        {
            DOTween.To(()=>s.value,val=>s.value = val,hp,1.0f).OnComplete(()=>{
                StageController.I.isStop = false;
                StageController.I.ReScroll();
            });
        }
        else
        {
            s.value = hp;
        }
        
    }
}
