using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TestPlayer : MonoBehaviour
{
    Rigidbody2D rgbd2d;
    void Start()
    {
        rgbd2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(_jump());
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.up * Time.deltaTime;
        }
    }

    IEnumerator _jump()
    {
        float startY = transform.localPosition.y;
        yield return new WaitForSeconds(0.1f);
        rgbd2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rgbd2d.AddForce(Vector2.up * 100f,ForceMode2D.Impulse);
        
        yield return new WaitForSeconds(0.05f);
        rgbd2d.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        rgbd2d.velocity = Vector3.zero;
        
        transform.localPosition = new Vector3(transform.localPosition.x, startY);
    }
}
