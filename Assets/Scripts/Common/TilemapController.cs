using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapController : MonoBehaviour
{
    [SerializeField] GameObject blockTile;
    Vector3 basePos;
    

    void Start()
    {       
        SetTile();
    }

    

    void SetTile()
    {
        for(float y = -5.5f;y < 6f; y += 0.5f)
        {
            for(float x = -2.5f;x < 3f; x += 0.5f)
            {
                GameObject t = Instantiate(blockTile);
                t.transform.SetParent(transform);
                t.transform.localPosition = new Vector3(x, y, 0);
                
            }
        }
        
    }
}
