using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapController : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] Tile blockTile;
    void Start()
    {
        SetTile();
    }

    void SetTile()
    {
        for(int y = -10;y < 11; y++)
        {
            for(int x = -6;x < 6; x++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0) ,blockTile);
                
            }
        }
    }
}
