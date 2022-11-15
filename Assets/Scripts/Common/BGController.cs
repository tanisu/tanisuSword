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

    public void ChangeColor()
    {
        StartCoroutine(_changeColor());
    }
    IEnumerator _changeColor()
    {
        yield return null;
        float _c = sp.color.r;
        float _nextColor = _c == 0 ? 1f : 0f;
        while(_nextColor != _c)
        {
            if(_nextColor == 1f)
            {
                _c += 0.1f;
            }
            else
            {
                _c -= 0.1f;
            }
            sp.color = new Color(_c, _c, _c);
            yield return new WaitForSeconds(0.43f);
        }

    }
}
