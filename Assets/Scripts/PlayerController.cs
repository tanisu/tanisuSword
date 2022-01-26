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
    [SerializeField] GameObject shilde;
    [SerializeField] Sprite deadSprite;
    public bool isDead;
    [SerializeField] Vector3 smallScale = new Vector3(0.9f,0.9f);
    Vector3 defaultScale = new Vector3(1,1,1);
    bool isInHole;
    bool isDeadLine;
    bool hasShilde;
    Animator anim;

    void Start()
    {
        bulletPool = StageController.I.playerBulletPool;
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        isDead = false;
    }

    
    void Update()
    {
        interval -= Time.deltaTime;
        if (isInHole)
        {
            transform.position -= new Vector3(0,StageController.I.stageSpeed) * Time.deltaTime;
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
            SoundManager.I.PlaySE(SESoundData.SE.ATTACK);
        }
    }

    public void HasShilde()
    {
        hasShilde = !hasShilde;
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
            if (!shilde.GetComponent<Shilde>().isHit)
            {
                
                int damage = collision.CompareTag("Enemy") ? collision.gameObject.GetComponent<Enemy>().power : collision.gameObject.GetComponent<BulletController>().power;
                DelHp(damage, sp);
                StageController.I.UpdateHp(hp);

                if (hp <= 0)
                {
                    _deadMe();
                }
                SoundManager.I.PlaySE(SESoundData.SE.DAMAGE);
            }
            else
            {
                shilde.GetComponent<Shilde>().isHit = false;
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
            shilde.SetActive(false);
            StageController.I.stopScroll();
            hp = maxHp;
            StageController.I.UpdateHp(maxHp);
            SoundManager.I.PlaySE(SESoundData.SE.POWER_UP);
            StartCoroutine(_showMe());
        }
        if (collision.CompareTag("Item"))
        {
            Items i = collision.GetComponent<Items>();
            SoundManager.I.PlaySE(SESoundData.SE.ITEM);
            switch (collision.name)
            {
                
                case "Boots":
                    if(speed < maxSpeed)
                    {
                        speed += i.powerPoint;
                    }
                break;
                case "Sword":
                    if((shootInterval - i.powerPoint ) > minShootInterval)
                    {
                        shootInterval -= i.powerPoint;
                    }
                    else
                    {
                        shootInterval = minShootInterval;
                    }
                break;
                case "Shilde":
                    if (!hasShilde)
                    {
                        hasShilde = true;
                        shilde.SetActive(true);
                    }
                    else
                    {
                        shilde.GetComponent<Shilde>().Recovery();
                    }
                    break;
            }
            collision.gameObject.SetActive(false);
        }

        if (collision.CompareTag("Goal"))
        {
            StageController.I.ViewStageClear();
            gameObject.SetActive(false);
        }
    }

   

    IEnumerator _jump(Collider2D collision)
    {
        yield return new WaitForSeconds(1.5f);
        GetComponent<BoxCollider2D>().enabled = false;
        transform.DOLocalJump(transform.localPosition,0.5f,1,0.5f).SetLink(gameObject).OnComplete(()=> {
            GetComponent<BoxCollider2D>().enabled = true;
        });
        transform.localScale = defaultScale;
        collision.gameObject.SetActive(false);
        isInHole = false;
    }

    IEnumerator _showMe()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.9f);
        if (hasShilde)
        {
            shilde.SetActive(true);
        }
        GetComponent<BoxCollider2D>().enabled = true;
        sp.sortingOrder = 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Obstacle") )
        {

            if (isDeadLine && !isDead)
            {
                _deadMe();
            }
            
            
            
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            
            if (isDeadLine && !isDead)
            {
                _deadMe();
            }
        }
    }

    private void _deadMe()
    {
        shilde.SetActive(false);
        anim.enabled = false;
        sp.sprite = deadSprite;
        StageController.I.UpdateHp(0);
        isDead = true;
    }

    public void SetUpForStart()
    {
        transform.localPosition = new Vector3(0, -4.5f,0);
        isDead = false;
    }
}
