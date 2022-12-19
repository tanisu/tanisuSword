using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpController : MonoBehaviour
{
    
    [SerializeField] Transform exitPos;
    [SerializeField] EnemiesController enemies;
    [SerializeField] ObjectPool objectPool;
    GameObject stage;
    

    private void Start()
    {
        stage = GameObject.Find("Stage");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            objectPool.ResetAll();
            stage.transform.position = new Vector3(exitPos.position.x, 0f, 0f);
            exitPos.gameObject.GetComponent<CircleCollider2D>().enabled = true;
            collision.transform.position = exitPos.position;
            SoundManager.I.PlaySE(SESoundData.SE.WARP);
            if (enemies)
            {
                enemies.ResetEnemies();
            }
            
        }
    }

}
