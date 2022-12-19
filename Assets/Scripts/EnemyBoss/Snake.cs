using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public float moveSpeed = 1;
    public float bodySpeed = 5;
    public float steerSpeed = 180;
    public GameObject bodyPrefab;
    [SerializeField]SnakeHead snakeHead;
    public int gap = 10;
    int bodyCount;
    public Transform target;
    public int bodyNums;
    private List<GameObject> BodyParts = new List<GameObject>();
    private List<Vector3> PositionsHistory = new List<Vector3>();
    
    private List<SpriteRenderer> bodyRenderers = new List<SpriteRenderer>();
    
    enum STATE
    {
        MOVE,
        STOP,
        ATTACK
        
    }

    STATE state;

    void Start()
    {
        for (int i = 0; i < bodyNums; i++)
        {
            GrowSnake();
        }
        snakeHead.HitDamage = _flash;
        snakeHead.OnPlayer = _onPlayer;
        snakeHead.ToNext = _next;
        snakeHead.IsDead = _isDead;


    }



    private void _onPlayer()
    {
        state = STATE.ATTACK;
        
    }
    private void _isDead()
    {
        state = STATE.STOP;
        StageController.I.StopRain();
        StartCoroutine(_deadBodies());
    }


    IEnumerator _deadBodies()
    {

        bodyRenderers.Reverse();
        foreach(SpriteRenderer body in bodyRenderers)
        {
            
            yield return StartCoroutine(_flashRed(body));
            // yield return new WaitForSeconds(0.1f);
            SoundManager.I.PlaySE(SESoundData.SE.BOSS_EXPLOSION);

            body.gameObject.SetActive(false);
        }
    }
    
    
    private void _attack()
    {
        Vector3 headPos = transform.position;
        int idx = 0;
        PositionsHistory.Insert(0, transform.position);
        foreach (GameObject body in BodyParts)
        {
            
            if (idx != 0)
            {
                if ((headPos - body.transform.position).sqrMagnitude > Mathf.Epsilon)
                {

                    body.transform.position = Vector3.MoveTowards(body.transform.position, headPos, bodySpeed * 2 * Time.deltaTime);

                }
                else if((headPos - BodyParts[BodyParts.Count -1].transform.position).sqrMagnitude <= Mathf.Epsilon)
                {
                    state = STATE.STOP;
                    snakeHead.Attack();
                    PositionsHistory.Clear();
                }
            }
            idx++;
            
        }
    }

    private void _next(int _hp)
    {
        
        state = STATE.MOVE;
    }

    void Update()
    {
        if (target == null || state == STATE.STOP) return;
        switch (state)
        {
            case STATE.MOVE:
                _move();
                break;
            case STATE.ATTACK:
                _attack();
                break;
        }
        
        
        
    }


    private void _move()
    {
        transform.position += transform.up * moveSpeed * Time.deltaTime;

        transform.up = target.position - transform.position;
        
        PositionsHistory.Insert(0, transform.position);

        int idx = 0;
        foreach (GameObject body in BodyParts)
        {
            Vector3 point = PositionsHistory[Mathf.Min(idx * gap, PositionsHistory.Count - 1)];
            body.transform.position = point;
            idx++;
        }
    }

    private void GrowSnake()
    {
        bodyCount++;
        GameObject body = Instantiate(bodyPrefab,transform.parent.transform);
        SpriteRenderer spBody =  body.GetComponent<SpriteRenderer>();
        bodyRenderers.Add(spBody);
        spBody.sortingOrder = -bodyCount;
        
        if(bodyCount == 1)
        {
            spBody.enabled = false;
        }
        BodyParts.Add(body);
    }

    private void _flash(int _hp)
    {
        //Debug.Log(_hp);
        //if (_hp % 10 == 0)
        //{

        //    GameObject body = BodyParts[BodyParts.Count - 1];
        //    body.SetActive(false);
        //    BodyParts.RemoveAt(BodyParts.Count - 1);
        //    bodyRenderers.RemoveAt(bodyRenderers.Count - 1);

        //}
        foreach (SpriteRenderer spBody in bodyRenderers)
        {
            StartCoroutine(_flashRed(spBody));
        }

    }

    IEnumerator _flashRed(SpriteRenderer _spBody)
    {
        yield return null;


            for (int i = 0; i < 5; i++)
            {
                _spBody.color = Color.red;
                yield return new WaitForSeconds(0.05f);
                _spBody.color = Color.white;
                yield return new WaitForSeconds(0.05f);
            }
        

    }
}
