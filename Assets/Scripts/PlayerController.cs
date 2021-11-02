using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(Vector3 _moveVec)
    {
        transform.Translate(_moveVec * 3 * Time.deltaTime);
        Vector3 nowPos = transform.localPosition;
        nowPos.x = Mathf.Clamp(nowPos.x, -2.5f, 2.5f);
        nowPos.y = Mathf.Clamp(nowPos.y, -4.5f, 4.5f);
        transform.localPosition = nowPos;
    }
}
