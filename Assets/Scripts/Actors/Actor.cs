using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    
    [SerializeField] protected int hp;
    public int power;
    private void Start()
    {
        
        
    }


    protected IEnumerator _flashColor(SpriteRenderer sp)
    {
        for (int i = 0; i < 10; i++)
        {
            sp.enabled = false;
            yield return new WaitForSeconds(0.01f);
            sp.enabled = true;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void HideFromStage()
    {
        Destroy(gameObject);
    }

    protected void DelHp(int damage,SpriteRenderer sp)
    {
        hp -= damage;
        if (hp >= 0)
        {
            StartCoroutine(_flashColor(sp));
        }
    }

    protected void DelEnemyHp(int damage,SpriteRenderer sp)
    {
        hp -= damage;
        if(hp <= 0)
        {
            HideFromStage();
        }
        else
        {
            if(sp != null)
            {
                StartCoroutine(_flashColor(sp));
            }
            
        }
    }
}
