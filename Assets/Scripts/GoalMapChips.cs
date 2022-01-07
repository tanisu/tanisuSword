using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalMapChips : MonoBehaviour
{
    Animator anim;
    BoxCollider2D bc2d;

    private void Start()
    {
        anim = GetComponent<Animator>();
        bc2d = GetComponent<BoxCollider2D>();
    }

    public void ViewGoal()
    {
        anim.SetBool("isBossDead",true);
        bc2d.isTrigger = true;
    }

}
