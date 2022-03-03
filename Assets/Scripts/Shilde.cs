using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shilde : Actor
{
    public bool isHit;
    [SerializeField] int MaxHp;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("EnemyBullet"))
        {
            SoundManager.I.PlaySE(SESoundData.SE.GUARD);
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
        if (collision.CompareTag("Boss"))
        {
            hp = 0;
            StageController.I.PlayerHasShilde();
            gameObject.SetActive(false);
            hp = MaxHp;
        }
    }

    public void Recovery()
    {
        hp = MaxHp;
    }
}
