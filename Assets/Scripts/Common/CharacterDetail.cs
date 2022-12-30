using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterDetail : MonoBehaviour
{

    [SerializeField] Text charactorName;
    [SerializeField] Image charctorImage;
    [SerializeField] Animator animator;

    public void ChangeCharactor(string _name ,Sprite _sprite,RuntimeAnimatorController anim)
    {
        charactorName.text = _name;
        charctorImage.sprite = _sprite;
        animator.runtimeAnimatorController = anim;
    }
}
