using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalMapChips : MonoBehaviour
{
    [SerializeField] bool hasAnim;
    Animator anim;//�A�j���[�V�������邩�ǂ����B�����ꍇ��boxcollider2d�͍ŏ��I�t
    BoxCollider2D bc2d;

    private void Start()
    {
        if (hasAnim)
        {
            anim = GetComponent<Animator>();
        }
        
        bc2d = GetComponent<BoxCollider2D>();
        if (!hasAnim)
        {
            bc2d.enabled = false;
        }
    }

    public void ViewGoal()
    {
        SoundManager.I.StopBGM();
        if (hasAnim)
        {
            anim.SetBool("isBossDead", true);
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = true;
            bc2d.enabled = true;
        }


        bc2d.isTrigger = true;
    }

}
