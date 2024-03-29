using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Spider : BossBase
{
    [SerializeField] Net net;
    float xLimit = 1.9f;
    const float DEFAULTY = 2.5f;
    float retrunY = 2.5f;
    float yLimit = -0.5f;
    Animator anim;
    public bool isDirectAttack;
   // int attackCount = 0;
    Tween tween;
    enum DIRECTION
    {
        IDLE,
        LEFT,
        RIGHT
    }
    DIRECTION direction;
    
    new void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        direction = DIRECTION.RIGHT;
    }

    public void Attack()
    {
        //attackCount++;
        ////Debug.Log(attackCount);
        //if(attackCount < 9)
        //{
            
            Instantiate(net, transform);
        //}
        //else if(!isDirectAttack )
        //{
        //    _attack2();
        //}
        
    }
    public void IsDead()
    {
        tween.Kill();
        DeadStart();
    }
    
    public void IdleEnd()
    {
        
        anim.SetBool("Idle", false);
        
        switch (direction)
        {
            //右方向
            case DIRECTION.RIGHT:
                
                if (transform.localPosition.x < xLimit)
                {
                    anim.SetBool("Right", true);
                }
                else
                {
                    direction = DIRECTION.LEFT;
                    anim.SetBool("Left", true);
                }
                break;
            //左方向
            case DIRECTION.LEFT:
                if(transform.localPosition.x > -xLimit )
                {
                    anim.SetBool("Left", true);
                }
                else
                {
                    direction = DIRECTION.RIGHT;
                    anim.SetBool("Right", true);
                }
                break;
  
        }
    }

    //動いたら止まる処理
    public void MoveLimit()
    {
        switch (direction)
        {
            case DIRECTION.RIGHT:
                anim.SetBool("Right", false);
                break;
            case DIRECTION.LEFT:
                anim.SetBool("Left", false);
            break;
        }
        anim.SetBool("Idle", true);
    }

    //private void _attack2()
    //{
    //    //isAttack = false;
    //    //anim.SetBool("Attack", false);

    //    anim.SetBool("Idle", false);
    //    anim.SetBool("Left", false);
    //    anim.SetBool("Right", false);
        
    //    Vector3 beforePos = new Vector3(0,2.5f);
    //    tween = transform.DOLocalMove(new Vector3(0, 0), 0.3f).SetLink(gameObject).OnComplete(() =>
    //    {
    //        anim.SetBool("Attack2", true);
    //        tween = transform.DOLocalRotate(new Vector3(0, 0, 360f), 1.5f, RotateMode.FastBeyond360)
    //        .SetEase(Ease.Linear).SetLink(gameObject).OnComplete(()=> {
    //            tween = transform.DOLocalMove(beforePos, 0.6f).SetLink(gameObject);
    //        });
    //    });
    //}

    private void _stopAttack2()
    {
        anim.SetBool("Attack2", false);
       // attackCount = 0;
        anim.SetBool("Idle", true);
    }

    public void CatchPlayer(Vector3 netPos)
    {
        


        Vector3 beforePos = transform.position;
        isDirectAttack = true;
        anim.SetBool("Attack", true);
        anim.SetBool("Idle", false);
        anim.SetBool("Left", false);
        anim.SetBool("Right", false);
        

        retrunY = retrunY > yLimit ? retrunY - 0.75f : DEFAULTY;



        tween =  transform.DOMove(netPos, 0.53f).SetLink(gameObject).OnComplete(()=> {
            tween = transform.DOLocalMove(new Vector3(0,retrunY), 0.6f).SetLink(gameObject).OnComplete(()=> {
                
                anim.SetBool("Attack", false);
                direction = (DIRECTION)Random.Range(1, 3);
                IdleEnd();
                isDirectAttack = false;
            });
        });
    }

    protected override void DeadStart()
    {
        tween.Kill();
        base.DeadStart();
        
    }
}
