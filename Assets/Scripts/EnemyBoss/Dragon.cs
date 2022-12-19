using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    public void StartFire()
    {
        SoundManager.I.PlaySE(SESoundData.SE.DRAGON);
    }
}
