using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolContent : MonoBehaviour
{
    ObjectPool pool;

    // Start is called before the first frame update
    void Start()
    {
        pool = transform.parent.GetComponent<ObjectPool>();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowInStage(Vector3 _pos, float _angle)
    {
        transform.position = _pos;
        transform.eulerAngles = new Vector3(0, _angle, 0);
    }

    public void HideFromStage()
    {
        //Debug.Assert(gameObject.activeInHierarchy);
        pool.Collect(this);
    }

}
