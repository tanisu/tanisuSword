using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorRandomize : MonoBehaviour
{
    Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

   void Start()
    {
        anim.Play("Base Layer.DragonFire", 0, Random.value);
    }
}
