using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    public int power;
    public void EndThunder()
    {
        gameObject.SetActive(false);
    }
}
