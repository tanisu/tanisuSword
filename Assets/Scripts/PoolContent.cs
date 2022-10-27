using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolContent : MonoBehaviour
{
    ObjectPool pool;
    Rigidbody2D rgbd2d;

    void Start()
    {
        pool = transform.parent.GetComponent<ObjectPool>();
        gameObject.SetActive(false);
        rgbd2d = GetComponent<Rigidbody2D>();
        rgbd2d.simulated = false;
    }

    public void ShowInStage(Vector3 _pos)
    {
        rgbd2d.simulated = true;
        transform.position = _pos;
        
    }

    public void HideFromStage()
    {
        rgbd2d.simulated = false;
        pool.Collect(this);
    }

}
