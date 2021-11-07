using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed;
    public int attack;
    PoolContent poolContent;

    void Start()
    {
        poolContent = GetComponent<PoolContent>();
    }

    
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        if(transform.localPosition.y > 6 || transform.localPosition.y < -6 || transform.localPosition.x > 4 || transform.localPosition.x < -4)
        {
            poolContent.HideFromStage();
        }
    }
}
