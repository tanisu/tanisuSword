using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public float moveSpeed = 1;
    public float bodySpeed = 5;
    public float steerSpeed = 180;
    public GameObject bodyPrefab;
    public int gap = 10;
    public int bodyCount;
    public Transform target;

    private List<GameObject> BodyParts = new List<GameObject>();
    private List<Vector3> PositionsHistory = new List<Vector3>();
    private List<Vector3> RotateHistory = new List<Vector3>();
    void Start()
    {
        GrowSnake();
        GrowSnake();
        GrowSnake();
        GrowSnake();
        GrowSnake();
        GrowSnake();

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * moveSpeed * Time.deltaTime;

        transform.up = target.position - transform.position;
        Vector3 angle = transform.rotation.eulerAngles;

        //transform.eulerAngles = angle;
        
        PositionsHistory.Insert(0, transform.position);
        RotateHistory.Insert(0, angle);
        int idx = 0;
        foreach(GameObject body in BodyParts)
        {
            Vector3 point = PositionsHistory[ Mathf.Min( idx * gap,PositionsHistory.Count-1)];
            Vector3 rote = RotateHistory[Mathf.Min(idx * gap, RotateHistory.Count - 1)];
            body.transform.position = point;
            body.transform.eulerAngles = rote;
            idx++;
        }
    }


    private void GrowSnake()
    {
        bodyCount++;
        GameObject body = Instantiate(bodyPrefab,transform);
        body.GetComponent<SpriteRenderer>().sortingOrder = -bodyCount;
        if(bodyCount == 1)
        {
            body.GetComponent<SpriteRenderer>().enabled = false;
        }
        BodyParts.Add(body);
    }
}
