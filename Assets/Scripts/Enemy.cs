using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    [SerializeField] bool isFling;
    [SerializeField] bool randomTrun;
    [SerializeField] LayerMask ObstacleLayer;
    [SerializeField] float speed;
    [SerializeField] GameObject launchPort;
    SpriteRenderer sp;
    bool[] isObstacles;
    bool isTurn;
    float width = 0.2f;
    float lineLength = 0.2f;
    ObjectPool bulletPool;
    Animator anim;
    bool reStart;
    bool turnRight;

    public enum DIR
    {
        DOWN,
        LEFT,
        RIGHT,
        MAX
    }
    public DIR dir = DIR.DOWN;

    void Start()
    {
        if (randomTrun)
        {
            turnRight = Random.Range(0, 2) == 0;
        }
        isObstacles = new bool[(int)DIR.MAX];
        sp = GetComponent<SpriteRenderer>();
        bulletPool = StageController.I.enemyBulletPool;
        anim = GetComponent<Animator>();
        
    }

    
    void Update()
    {

        if (StageController.I.isStop)
        {
            anim.speed = 0;
            reStart = true;
            return;
        }
        if (reStart && !StageController.I.isStop)
        {
            anim.speed = 1;
            reStart = false;
            return;
        }
        
        if ( !isFling && !StageController.I.isIdle)
        {
            WalkingMove();
        }

        if (transform.position.y - StageController.I.transform.position.y < -5.5)
        {
            HideFromStage();
        }

    }

    private void WalkingMove()
    {
        Vector3 rightDwonPos = transform.position + Vector3.right * width;
        Vector3 leftDwonPos = transform.position - Vector3.right * width;
        Vector3 rightEndPos = transform.position - Vector3.up * lineLength + Vector3.right * width;
        Vector3 leftEndPos = transform.position - Vector3.up * lineLength - Vector3.right * width;
        isObstacles[(int)DIR.DOWN] = Physics2D.Linecast(leftDwonPos, leftEndPos, ObstacleLayer) || Physics2D.Linecast(rightDwonPos, rightEndPos, ObstacleLayer);

        Vector3 leftPos = transform.position - Vector3.right * lineLength;
        isObstacles[(int)DIR.LEFT] = Physics2D.Linecast(transform.position, leftPos, ObstacleLayer);
        Vector3 rightPos = transform.position + Vector3.right * lineLength;
        isObstacles[(int)DIR.RIGHT] = Physics2D.Linecast(transform.position, rightPos, ObstacleLayer);



        if (!isObstacles[(int)DIR.DOWN])
        {
            dir = DIR.DOWN;
            isTurn = false;


        }
        else
        {
            if (!turnRight)
            {
                if (!isObstacles[(int)DIR.LEFT] && !isTurn)
                {
                    dir = DIR.LEFT;

                }
                else if (isObstacles[(int)DIR.LEFT])
                {
                    dir = DIR.RIGHT;
                    isTurn = true;
                }
                else if (isObstacles[(int)DIR.RIGHT])
                {
                    isTurn = false;
                }
            }
            else
            {
                if (!isObstacles[(int)DIR.RIGHT] && !isTurn)
                {
                    dir = DIR.RIGHT;

                }
                else if (isObstacles[(int)DIR.RIGHT])
                {
                    dir = DIR.LEFT;
                    isTurn = true;
                }
                else if (isObstacles[(int)DIR.LEFT])
                {
                    isTurn = false;
                }
                
            }
            

        }

        switch (dir)
        {
            case DIR.DOWN:
                transform.Translate(Vector3.down * speed * Time.deltaTime);
                break;
            case DIR.LEFT:
                transform.Translate(Vector3.right * -speed * Time.deltaTime);
                break;
            case DIR.RIGHT:
                transform.Translate(Vector3.right * speed * Time.deltaTime);
                break;
            default:
                transform.Translate(Vector3.down * speed * Time.deltaTime);
                break;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet") || collision.CompareTag("Shiled"))
        {
            SoundManager.I.PlaySE(SESoundData.SE.ENEMY_DAMAGE);
            PoolContent poolObj = collision.GetComponent<PoolContent>();
            int damage = poolObj != null ? poolObj.GetComponent<BulletController>().power : collision.GetComponent<Shilde>().power;
            if(poolObj != null)  poolObj.HideFromStage();
            DelEnemyHp(damage,sp);
            if(hp <= 0)
            {
                if (!gameObject.CompareTag("Boss") )
                {
                    
                    HideFromStage();
                }
                else {
                    StopCoroutine(coroutine);
                    sp.enabled = true;
                    
                    anim.SetBool("isDead", true);
                }
            }
        }
    }

    public void Shot(EnemyBulletPattern _o)
    {
        float angleOffset = (_o.Count - 1) / 2.0f;
        float openAngle = 0;
        if (_o.IsAimPlayer)
        {
            Vector3 diff = StageController.I.player.transform.localPosition - transform.localPosition;
            openAngle = Mathf.Atan2(diff.y,diff.x);
        }
        else
        {
            openAngle = _o.OpenAngle;
        }

        if (!_o.IsWaitTime)
        {
            for (int i = 0; i < _o.Count; i++)
            {
                float d = 0f;
                if (_o.IsAimPlayer)
                {
                    d = openAngle;
                }
                else
                {
                    d = -Mathf.PI / 2 + ((i - angleOffset) * openAngle * Mathf.Deg2Rad);
                }

                PoolContent obj = bulletPool.Launch(transform.position + Vector3.up * 0.2f);
                if (obj != null)
                {
                    BulletController bullet = obj.GetComponent<BulletController>();
                    bullet.speed = _o.Speed;
                    bullet.power = _o.Power;
                    bullet.Setting(d);
                }
            }
        }
        else
        {
            StartCoroutine(_waitShot(_o, openAngle,angleOffset));
        }

    }

    IEnumerator _waitShot(EnemyBulletPattern _o,float _openAngle,float _angleOffset)
    {
        for(int i = 0; i < _o.Count; i++)
        {
            float d = 0f;
            d = -Mathf.PI / 2 + ((i - _angleOffset) * _openAngle * Mathf.Deg2Rad);
            Vector3 port;
            if (launchPort)
            {
                port = launchPort.transform.position;
            }
            else
            {
                port = transform.position;
            }
            PoolContent obj = bulletPool.Launch(port);
            if (obj != null)
            {
                BulletController bullet = obj.GetComponent<BulletController>();
                bullet.speed = _o.Speed;
                bullet.power = _o.Power;
                bullet.Setting(d);
                yield return new WaitForSeconds(0.01f);
            }
        }
        
    }


}
