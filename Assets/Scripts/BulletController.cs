using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed;
    public int power;
    float dx, dy;
    PoolContent poolContent;

    void Start()
    {
        poolContent = GetComponent<PoolContent>();
    }
    public void Setting(float angle)
    {
        dx = Mathf.Cos(angle);
        dy = Mathf.Sin(angle);
    }
    public void HideFromStage()
    {
        poolContent.HideFromStage();
    }

    
    void Update()
    {
        transform.position += new Vector3(dx, dy, 0) * 5 * Time.deltaTime;
        
        if(transform.localPosition.y > 6 || transform.localPosition.y < -6 || transform.localPosition.x > 4 || transform.localPosition.x < -4)
        {
            poolContent.HideFromStage();
        }
    }
}
