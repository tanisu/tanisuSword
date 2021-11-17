using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleWrapper : MonoBehaviour
{
    [SerializeField] GameObject hole;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hole.SetActive(true);
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
