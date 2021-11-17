using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockWrapper : MonoBehaviour
{
    [SerializeField] GameObject block;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            PoolContent poolObj = collision.gameObject.GetComponent<PoolContent>();
            poolObj.HideFromStage();
            block.SetActive(true);
        }
    }
}
