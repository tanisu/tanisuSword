using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int hitPoint = 1;
    [SerializeField] LayerMask ObstacleLayer;
    [SerializeField] float speed;
    SpriteRenderer sp;
    bool[] isObstacles;
    bool isTurn;
    float width = 0.2f;
    float lineLength = 0.2f;
    
    enum DIR
    {
        DOWN,
        LEFT,
        RIGHT,
        MAX
    }
    DIR dir = DIR.DOWN;

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

        //Debug.DrawLine(transform.position, leftPos, Color.blue);
        //Debug.DrawLine(transform.position, rightPos, Color.blue);
        //Debug.DrawLine(leftDwonPos, leftEndPos, Color.blue);
        //Debug.DrawLine(rightDwonPos, rightEndPos, Color.blue);

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

    public void HideFromStage()
    {
        Destroy(gameObject);
    }

     
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            
            PoolContent poolObj = collision.GetComponent<PoolContent>();
            poolObj.HideFromStage();
            hitPoint -= 1;
            if(hitPoint <= 0)
            {
                HideFromStage();
            }
            else
            {
                StartCoroutine(_flashColor());
            }
        }
    }

    IEnumerator _flashColor()
    {
        for(int i = 0; i < 10; i++)
        {
            sp.enabled = false;
            yield return new WaitForSeconds(0.01f);
            sp.enabled = true;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
