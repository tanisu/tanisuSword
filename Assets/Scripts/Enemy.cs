using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    
    [SerializeField] LayerMask ObstacleLayer;
    [SerializeField] float speed;
    SpriteRenderer sp;
    bool[] isObstacles;
    bool isTurn;
    float width = 0.2f;
    float lineLength = 0.2f;
    public int power;

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
        
    }

    
    void Update()
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
            else if(isObstacles[(int)DIR.LEFT])
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

        if (transform.position.y - StageController.I.transform.position.y < -5.5)
        {
            HideFromStage();
        }

    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            
            PoolContent poolObj = collision.GetComponent<PoolContent>();
            poolObj.HideFromStage();

            DelHp(1,sp);

        }
    }


}
