using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpExitController : MonoBehaviour
{
    [SerializeField] EnemiesController enemies;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (enemies)
            {
                enemies.ShowEnemies();
            }
            GetComponent<CircleCollider2D>().enabled = false;
        }

    }
}
