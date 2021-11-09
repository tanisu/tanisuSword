using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChips : MonoBehaviour
{
    [SerializeField] int hp;
    [SerializeField] bool breakObj;
    [SerializeField] Sprite brokenSprite = default;
    BoxCollider2D bc2d;
    SpriteRenderer sp;
    void Start()
    {
        bc2d = GetComponent<BoxCollider2D>();
        sp = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            PoolContent pool = collision.GetComponent<PoolContent>();
            
            pool.HideFromStage();
            if (breakObj)
            {
                hp -= collision.GetComponent<BulletController>().power;
                
                if (hp <= 0)
                {
                    bc2d.enabled = false;
                    
                    gameObject.SetActive(false);
                }
                else
                {
                    sp.sprite = brokenSprite;
                }
            }

        }
    }
}
