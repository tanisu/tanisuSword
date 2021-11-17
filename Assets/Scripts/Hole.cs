using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    [SerializeField] GameObject childHole;
    public void ViewChild()
    {
        childHole.SetActive(true);
    }
}
