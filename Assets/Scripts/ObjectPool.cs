using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] PoolContent content = default;
    [SerializeField] int maxCount = 20;
    Queue<PoolContent> objQueue;
    void Start()
    {
        objQueue = new Queue<PoolContent>(maxCount);
        for(int i = 0; i < maxCount; ++i)
        {
            PoolContent tmpObj = Instantiate(content);
            tmpObj.transform.parent = transform;
            tmpObj.transform.localPosition = new Vector3(100, 100, 0);
            objQueue.Enqueue(tmpObj);
        }
    }

    
    void Update()
    {
        
    }

    public void Collect(PoolContent poolContent)
    {

    }
}
