using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGController : MonoBehaviour
{
    Vector3 basePos;
    SpriteRenderer sp;
    void Start()
    {
        basePos = transform.position;
        sp = GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        float posY = Mathf.Floor((StageController.I.transform.position.y - basePos.y) /0.5f ) * 0.5f;
        if(posY < 1.5f)
        {
            posY += 0.5f;
        }
        Vector3 nowPos = transform.position;
        nowPos.y = posY + basePos.z;
        transform.position = nowPos;
    }

    public void ChangeColor(float _c)
    {
        StartCoroutine(_changeColor(_c));
    }
    IEnumerator _changeColor(float _c)
    {
        yield return null;
        float c = sp.color.r;
        //float _c = sp.color.r;
        float _nextColor = _c;
        
        while(_nextColor != c)
        {
            if(_nextColor == 1f)
            {
                c += 0.1f;
            }
            else
            {
                c -= 0.1f;
            }
            sp.color = new Color(c, c, c);
            yield return new WaitForSeconds(0.43f);
        }

    }
}
