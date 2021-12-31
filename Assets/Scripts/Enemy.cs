using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    [SerializeField] bool isFling;
    
    [SerializeField] LayerMask ObstacleLayer;
    [SerializeField] float speed;
    SpriteRenderer sp;
    bool[] isObstacles;
    bool isTurn;
    float width = 0.2f;
    float lineLength = 0.2f;
    ObjectPool bulletPool;
    Animator anim;
    bool reStart;

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
            Debug.Log("reScroll");
            anim.speed = 1;
            reStart = false;
            return;
        }
        
        if ( !isFling)
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
        if (collision.CompareTag("PlayerBullet"))
        {
            
            PoolContent poolObj = collision.GetComponent<PoolContent>();
            int damage = poolObj.GetComponent<BulletController>().power;
            poolObj.HideFromStage();
            DelEnemyHp(damage,sp);
            if(hp <= 0)
            {
                HideFromStage();
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
        for (int i = 0; i < _o.Count; i++)
        {
            float d = 0f;
            if (_o.IsAimPlayer)
            {
                d = openAngle ;
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


}
