using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Actor
{
    private ObjectPool bulletPool;
    private float interval = 0;//‚±‚êˆÈ‰º‚Ì‚Æ‚«‚µ‚©”­ŽË‚Å‚«‚È‚¢
    
    private SpriteRenderer sp;
    [SerializeField] float shootInterval;//”­ŽËŠÔŠu
    [SerializeField] float minShootInterval;
    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] float moveLimitY = 4.5f;
    [SerializeField] float moveLimitX = 2.5f;

    
    [SerializeField] int maxHp;
    [SerializeField] float maxSpeed;
    [SerializeField] float speed;
    public bool isDead;
    
    void Start()
    {
        bulletPool = StageController.I.playerBulletPool;
        sp = GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        interval -= Time.deltaTime;
    }

    public void Move(Vector3 _moveVec)
    {
        if (StageController.I.isStop)
        {
            return;
        }
        transform.Translate(_moveVec * speed * Time.deltaTime);
        Vector3 nowPos = transform.localPosition;
        nowPos.x = Mathf.Clamp(nowPos.x, -moveLimitX, moveLimitX);
        nowPos.y = Mathf.Clamp(nowPos.y, -moveLimitY, moveLimitY);
        transform.localPosition = nowPos;
    }

    public void Shot()
    {
        if(interval <= 0)
        {
            PoolContent bullet = bulletPool.Launch(transform.position + Vector3.up * 0.1f);
            if (bullet != null) {
                bullet.GetComponent<BulletController>().Setting(Mathf.PI / 2);
                bullet.GetComponent<BulletController>().speed = bulletSpeed;
                bullet.GetComponent<BulletController>().power = power;
            } 
            interval = shootInterval;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Enemy") || collision.CompareTag("EnemyBullet"))
        {
            int damage = collision.CompareTag("Enemy") ? collision.gameObject.GetComponent<Enemy>().power : collision.gameObject.GetComponent<BulletController>().power;
            DelHp(damage,sp);
            StageController.I.UpdateHp(hp);
            
            if(hp <= 0)
            {
                isDead = true;
            }
        }
        if (collision.CompareTag("House"))
        {
            sp.sortingOrder = -1;
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            transform.position = collision.gameObject.transform.position;
            
            StageController.I.stopScroll();
            StageController.I.UpdateHp(maxHp);
            StartCoroutine(_showMe());
        }
        if (collision.CompareTag("Item"))
        {
            Items i = collision.GetComponent<Items>();
            switch (collision.name)
            {
                
                case "Boots":
                    if(speed < maxSpeed)
                    {
                        speed += i.powerPoint;
                    }
                break;
                case "Sword":
                    if(shootInterval > minShootInterval)
                    {
                        shootInterval -= i.powerPoint;
                    }
                break;
            }

            Destroy(collision.gameObject);
        }
    }

    IEnumerator _showMe()
    {
        yield return new WaitForSeconds(0.9f);
        sp.sortingOrder = 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && transform.localPosition.y == -4.5f)
        {
            
            StageController.I.UpdateHp(0);
            isDead = true;
            
            
        }
    }
}
