using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Spider : MonoBehaviour
{
    [SerializeField] Net net;
    float xLimit = 1.9f;
    Animator anim;
    public bool isAttack;
    enum DIRECTION
    {
        IDLE,
        LEFT,
        RIGHT
    }
    DIRECTION direction;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        
        direction = DIRECTION.RIGHT;
    }

    public void Attack()
    {
        Instantiate(net,transform);
    }
    
    public void IdleEnd()
    {
        anim.SetTrigger("Idle");
        anim.SetBool("Idle", false);
        
        switch (direction)
        {
            //‰E•ûŒü
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
            //¶•ûŒü
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

    //“®‚¢‚½‚çŽ~‚Ü‚éˆ—
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

    public void CatchPlayer(Vector3 netPos)
    {
        Vector3 beforePos = transform.position;
        isAttack = true;
        anim.SetBool("Attack", true);
        anim.SetBool("Idle", false);
        anim.SetBool("Left", false);
        anim.SetBool("Right", false);

        transform.DOMove(netPos, 0.53f).SetLink(gameObject).OnComplete(()=> {
            transform.DOMove(beforePos, 0.6f).SetLink(gameObject).OnComplete(()=> {
                anim.SetBool("Attack", false);
                anim.SetBool("Idle", true);
                isAttack = false;
            });
        });

    }
}
