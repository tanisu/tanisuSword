using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : Actor
{
    ObjectPool bulletPool;
    float interval = 0;//‚±‚êˆÈ‰º‚Ì‚Æ‚«‚µ‚©”­ŽË‚Å‚«‚È‚¢
    SpriteRenderer sp;
    Vector3 defaultScale = new Vector3(1, 1, 1);
    Animator anim;
    Rigidbody2D rgbd2d;

    [SerializeField] LayerMask blockLayer;
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
    [SerializeField]bool isInHole;
    [SerializeField] Vector3 smallScale = new Vector3(0.9f, 0.9f);
    public bool isDead, hasShilde, isDeadLine, isObstacle,isHitting;
    public int level = 0;
    public int exp = 0;
    public int[] levelTable = new int[4] { 32, 64, 128, 256 };
    int upHp = 20;
    public UnityAction<Dictionary<string,int>> LevelUp;
    Dictionary<string, int> levelParams;


    private void Start()
    {
        bulletPool = StageController.I.playerBulletPool;
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rgbd2d = GetComponent<Rigidbody2D>();
        isDead = false;
        isObstacle = false;
        hp = maxHp;
        levelParams = new Dictionary<string, int>() {
            { "maxHp", maxHp },
            { "level", level +1},
            { "currentHp", hp},
            {"currentExp",exp },
            {"nextExp", levelTable[level] }
        };
        if (!GameManager.I.levelParams.ContainsKey("maxHp"))
        {
            GameManager.I.levelParams = levelParams;
        }
        else
        {
            maxHp = GameManager.I.levelParams["maxHp"];
            hp = maxHp;
            level = GameManager.I.levelParams["level"] - 1;
        }
    }


    void Update()
    {
        interval -= Time.deltaTime;
        if (isInHole)
        {
            transform.position -= new Vector3(0, StageController.I.stageSpeed) * Time.deltaTime;
        }
        if (isHitting || isInHole)
        {
            
            isObstacle = _checkFrontObstacle();
        }
        else
        {
            isObstacle = false;
        }
        if (isObstacle && isDeadLine)
        {
            _deadMe();
        }
    }

    public void AddExp(int _exp)
    {

        exp += _exp;
        if (exp >= levelTable[level])
        {
            SoundManager.I.PlaySE(SESoundData.SE.LEVELUP);
            exp = 0;
            maxHp += upHp;
            hp += upHp;
            level++;
            levelParams["maxHp"] = maxHp;
            levelParams["level"] = level + 1;
            levelParams["currentHp"] = hp;
            levelParams["currentExp"] = exp;
            if(level <= levelTable.Length - 1)
            {
                levelParams["nextExp"] = levelTable[level];
            }
                
            LevelUp?.Invoke(levelParams);
                
        }
        

    }

    bool _checkFrontObstacle()
    {
        Vector2 topStartRayPos = transform.position + (transform.up * 0.1f);
        Vector2 topEndRayPos = transform.position + (transform.up * 0.22f);
        Debug.DrawLine(topStartRayPos, topEndRayPos,Color.green);
        RaycastHit2D hit = Physics2D.Linecast(topStartRayPos, topEndRayPos,blockLayer);

        return hit;
    }

    public void Move(Vector3 _moveVec)
    {
        if (StageController.I.isStop || isInHole) return;
        
        
        
        rgbd2d.velocity = _moveVec.normalized * speed;
        
        Vector3 nowPos = transform.localPosition;
        nowPos.x = Mathf.Clamp(nowPos.x, -moveLimitX, moveLimitX);
        nowPos.y = Mathf.Clamp(nowPos.y, -moveLimitY, 3.7f);
        transform.localPosition = nowPos;
    }

    void _stopMove()
    {
        rgbd2d.velocity = Vector3.zero;
    }

    public void Shot()
    {
        if (StageController.I.isStop || isInHole) return;
  
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

    public void SetParams(float _shootInterval ,float _speed,bool _hasShilde,Dictionary<string,int> _levelParams)
    {
        shootInterval = _shootInterval;
        speed = _speed;
        hasShilde = _hasShilde;
        levelParams = _levelParams;
        exp = _levelParams["currentExp"];
        level = _levelParams["level"] - 1;
        maxHp = _levelParams["maxHp"];
        hp = maxHp;

        if (hasShilde)
        {
            shilde.SetActive(true);
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
        if (collision.CompareTag("Girl"))
        {
            StageController.I.ToEnding();
        }
        if (collision.CompareTag("DeadLine"))
        {
            isDeadLine = true;
        }
        if (collision.CompareTag("Enemy") || collision.CompareTag("EnemyBullet"))
        {
            if (!shilde.GetComponent<Shilde>().isHit)
            {
                
                int damage = collision.CompareTag("Enemy") ? collision.gameObject.GetComponent<Enemy>().power : collision.gameObject.GetComponent<BulletController>().power;
                _damaged(damage);
                if (collision.CompareTag("EnemyBullet"))
                {
                    collision.GetComponent<BulletController>().HideFromStage();
                }
            }
            else
            {
                shilde.GetComponent<Shilde>().isHit = false;
            }
            
        }

        if (collision.CompareTag("Boss"))
        {
            int damage = collision.gameObject.GetComponent<Enemy>().power;
            _damaged(damage);

        }
        if (collision.CompareTag("Thunder"))
        {
            int damage = collision.gameObject.GetComponent<Thunder>().power;
            _damaged(damage);
        }

        if (collision.CompareTag("Trap"))
        {
            switch (collision.name)
            {
                case "Hole":
                    _stopMove();
                    transform.position = collision.gameObject.transform.position;
                    transform.localScale = smallScale;
                    isInHole = true;
                    collision.GetComponent<Hole>().ViewChild();
                    StartCoroutine(_jump(collision));
                    break;
            }
            _damaged(1);

        }
        if (collision.CompareTag("House"))
        {
            _stopMove();
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
                case "candle":
                    StageController.I.DoLight(true);
                    break;

                case "Portion":
                    StartCoroutine(_toumei());
                    break;
            }
            collision.gameObject.SetActive(false);
        }

        if (collision.CompareTag("Switch"))
        {
            collision.GetComponent<SwitchController>().OnSwitch();
        }


        if (collision.CompareTag("Goal"))
        {
            levelParams["currentExp"] = exp;
            
            GameManager.I.currentHasShilde = hasShilde;
            GameManager.I.currentShootInterval = shootInterval;
            GameManager.I.currentSpeed = speed;
            GameManager.I.levelParams = levelParams;
            StageController.I.ViewStageClear();
            gameObject.SetActive(false);
        }
    }

    private void _damaged(int _damage)
    {
        DelHp(_damage, sp);
        if (hp <= 0)
        {
            _deadMe();
        }
        StageController.I.UpdateHp(hp);
        SoundManager.I.PlaySE(SESoundData.SE.DAMAGE);
    }

    IEnumerator _toumei()
    {
        anim.SetBool("Toumei", true);
        gameObject.layer = LayerMask.NameToLayer("Toumei");
        yield return new WaitForSeconds(6f);
        anim.SetBool("Toumei", false);
        anim.SetBool("ToumeiEnd", true);
        yield return new WaitForSeconds(3f);
        anim.SetBool("ToumeiEnd", false);
        gameObject.layer = LayerMask.NameToLayer("Default");

    }

    IEnumerator _jump(Collider2D collision = null)
    {

        yield return new WaitForSeconds(1.5f);
        Vector2 startPos = transform.localPosition;
        rgbd2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rgbd2d.AddForce(Vector2.up * 9f, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.1f);
        rgbd2d.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        rgbd2d.velocity = Vector3.zero;

        transform.localScale = defaultScale;
        transform.localPosition = startPos;
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

        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("River"))
        {
            isHitting = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("River"))
        //{
            isHitting = false;
        //}
    }

    private void _deadMe()
    {
        _stopMove();
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

    public void EscapeHole()
    {
        isInHole = false;
    }

    public void CatchedHole()
    {
        _stopMove();
        isInHole = true;
    }


}
