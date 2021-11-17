using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    [SerializeField] Vector3 smallScale = new Vector3(0.9f,0.9f);
    Vector3 defaultScale = new Vector3(1,1,1);
    bool isInHole;
    bool isDeadLine;
    
    void Start()
    {
        bulletPool = StageController.I.playerBulletPool;
        sp = GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        interval -= Time.deltaTime;
        if (isInHole)
        {
            transform.position -= new Vector3(0,0.1f) * Time.deltaTime;
        }
    }

    public void Move(Vector3 _moveVec)
    {
        if (StageController.I.isStop || isInHole)
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
        if (StageController.I.isStop || isInHole)
        {
            return;
        }
        if (interval <= 0)
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("DeadLine"))
        {
            isDeadLine = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeadLine"))
        {
            isDeadLine = true;
        }
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
        if (collision.CompareTag("Trap"))
        {
            switch (collision.name)
            {
                case "Hole":
                    transform.position = collision.gameObject.transform.position;
                    transform.localScale = smallScale;
                    isInHole = true;
                    collision.GetComponent<Hole>().ViewChild();
                    StartCoroutine(_jump(collision));
                    break;
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

   

    IEnumerator _jump(Collider2D collision)
    {
        yield return new WaitForSeconds(1.5f);
        transform.DOLocalJump(transform.localPosition,0.5f,1,0.5f).SetLink(gameObject);
        transform.localScale = defaultScale;
        collision.gameObject.SetActive(false);
        isInHole = false;

    }

    IEnumerator _showMe()
    {
        yield return new WaitForSeconds(0.9f);
        sp.sortingOrder = 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Obstacle") )
        {
            Debug.Log("collision");
            if (isDeadLine)
            {
                Debug.Log("Dead");
                StageController.I.UpdateHp(0);
                isDead = true;
            }
            
            
            
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Stay");
            if (isDeadLine)
            {
                Debug.Log("Dead");
                StageController.I.UpdateHp(0);
                isDead = true;
            }
        }
    }
}
