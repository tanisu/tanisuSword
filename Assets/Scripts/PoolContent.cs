using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolContent : MonoBehaviour
{
    ObjectPool pool;


    void Start()
    {
        pool = transform.parent.GetComponent<ObjectPool>();
        gameObject.SetActive(false);
    }

    void Update()
    {
        
    }

    public void ShowInStage(Vector3 _pos)
    {
        transform.position = _pos;
        
    }

    public void HideFromStage()
    {
        //Debug.Assert(gameObject.activeInHierarchy);
        pool.Collect(this);
    }

}
