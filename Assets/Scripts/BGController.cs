using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGController : MonoBehaviour
{
    Vector3 basePos;
    
    void Start()
    {
        basePos = transform.position;
    }

    
    void Update()
    {
        float posY = Mathf.Floor((StageController.I.transform.position.y - basePos.y) /0.5f ) * 0.5f;
        if(posY < 1.5f)
        {
            posY += 0.5f;
        }
        Debug.Log(posY);
        Vector3 nowPos = transform.position;
        nowPos.y = posY + basePos.z;
        transform.position = nowPos;
    }
}
