using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private ObjectPool bulletPool;
    private float interval = 0;//‚±‚êˆÈ‰º‚Ì‚Æ‚«‚µ‚©”­ŽË‚Å‚«‚È‚¢
    [SerializeField] float shootInterval = 0.5f;//”­ŽËŠÔŠu
    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] float moveLimitY = 4.5f;
    [SerializeField] float moveLimitX = 2.5f;

    public bool isDead;

    void Start()
    {
        bulletPool = StageController.I.playerBulletPool;
    }

    
    void Update()
    {
        interval -= Time.deltaTime;
    }

    public void Move(Vector3 _moveVec)
    {
        transform.Translate(_moveVec * 3 * Time.deltaTime);
        Vector3 nowPos = transform.localPosition;
        nowPos.x = Mathf.Clamp(nowPos.x, -moveLimitX, moveLimitX);
        nowPos.y = Mathf.Clamp(nowPos.y, -moveLimitY, moveLimitY);
        transform.localPosition = nowPos;
    }

    public void Shot()
    {
        if(interval <= 0)
        {
            PoolContent bullet = bulletPool.Launch(transform.position + Vector3.up * 0.1f, 0);
            if (bullet != null) bullet.GetComponent<BulletController>().speed = bulletSpeed;
            interval = shootInterval;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && transform.localPosition.y == -4.5f)
        {
            
            isDead = true;
        }
    }
}
