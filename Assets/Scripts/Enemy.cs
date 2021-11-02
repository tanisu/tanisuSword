using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int hitPoint = 1;
    SpriteRenderer sp;
    
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        
    }

    
    void Update()
    {
        
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
