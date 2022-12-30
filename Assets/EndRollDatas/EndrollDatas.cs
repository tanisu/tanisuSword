using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "EndrollData")]
public class EndrollDatas : ScriptableObject
{
    public string characterName;
    public Sprite characterSprite;
    public RuntimeAnimatorController anim;
}
