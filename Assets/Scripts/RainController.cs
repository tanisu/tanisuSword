using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainController : MonoBehaviour
{
    [SerializeField] float maxY, minY,speed;
    SpriteRenderer sp;
    Vector3 startPos;
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        startPos = transform.localPosition;
    }

    private void FixedUpdate()
    {
        transform.Translate(new Vector3(0,-1f,0) * Time.fixedDeltaTime * speed);
        if (transform.localPosition.y <= minY)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, maxY);
        }
    }

    public void StopRain()
    {
        StartCoroutine(_stopRain());
    }

    IEnumerator _stopRain()
    {
        yield return null;
        float a = sp.color.a;
        while(a > 0)
        {
            a -= 0.1f;
            sp.color = new Color(1, 1, 1, a);
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void RedRain()
    {
        transform.localPosition = startPos;
        sp.color = new Color(0.8f, 0, 0, 0.5f);
    }



}
