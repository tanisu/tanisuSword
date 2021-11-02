using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private ObjectPool bulletPool;
    private float interval = 0;//‚±‚êˆÈ‰º‚Ì‚Æ‚«‚µ‚©”­ŽË‚Å‚«‚È‚¢
    [SerializeField] float shootInterval = 0.5f;//”­ŽËŠÔŠu
    [SerializeField] float bulletSpeed = 10f;

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
        nowPos.x = Mathf.Clamp(nowPos.x, -2.5f, 2.5f);
        nowPos.y = Mathf.Clamp(nowPos.y, -4.5f, 4.5f);
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
}
