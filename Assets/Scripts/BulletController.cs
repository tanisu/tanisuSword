using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed;
    public int power;
    float dx, dy;
    Rigidbody2D rgbd2d;
    PoolContent poolContent;

    void Start()
    {
        poolContent = GetComponent<PoolContent>();
        rgbd2d = GetComponent<Rigidbody2D>();
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
        //transform.position += new Vector3(dx, dy, 0) * 5 * Time.deltaTime;
        
        rgbd2d.velocity = new Vector3(dx, dy, 0) * 5;

        if (transform.localPosition.y > 6 || transform.localPosition.y < -6 || transform.localPosition.x > 4 || transform.localPosition.x < -4)
        {
            
            poolContent.HideFromStage();
        }
    }
}
