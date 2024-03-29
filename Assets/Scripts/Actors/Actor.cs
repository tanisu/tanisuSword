using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    
    [SerializeField] protected int hp;
    protected Coroutine coroutine;
    protected bool isFlash;
    public int power;

    protected IEnumerator _flashColor(SpriteRenderer sp)
    {
        isFlash = true;
        for (int i = 0; i < 10; i++)
        {
            
            sp.enabled = false;
            yield return new WaitForSeconds(0.05f);
            sp.enabled = true;
            yield return new WaitForSeconds(0.05f);
        }
        isFlash = false;
    }

    public void HideFromStage()
    {
        Destroy(gameObject);
    }

    protected void DelHp(int damage,SpriteRenderer sp)
    {
        
        hp -= damage;
        
        if (hp >= 0 && !isFlash)
        {
            StartCoroutine(_flashColor(sp));
        }
    }

    protected virtual void DelEnemyHp(int damage,SpriteRenderer sp)
    {
        hp -= damage;

        if(sp != null && !isFlash)
        {

            coroutine = StartCoroutine(_flashColor(sp));
        }
    }
}
