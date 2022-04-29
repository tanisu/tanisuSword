using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager I;
    public float currentShootInterval;
    public float currentSpeed;
    public bool currentHasShilde;

    private void Awake()
    {
        if (I == null)
        {
            I = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetParams()
    {
        currentHasShilde = false;
        currentShootInterval = 0;
        currentSpeed = 0;
    }
}
