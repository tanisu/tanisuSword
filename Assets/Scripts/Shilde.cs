using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shilde : Actor
{
    public bool isHit;
    [SerializeField] int MaxHp;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("House"))
        {
            Debug.Log("house");
            return;
        }
        if (collision.CompareTag("Enemy") || collision.CompareTag("EnemyBullet"))
        {
            hp--;
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<Actor>().HideFromStage();
            }
            else
            {
                collision.GetComponent<BulletController>().HideFromStage();
            }
            isHit = true;
            
            if (hp <= 0)
            {
                StageController.I.PlayerHasShilde();
                gameObject.SetActive(false);
                hp = MaxHp;
            }
        }
    }
}
