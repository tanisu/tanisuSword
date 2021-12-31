using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] TextAsset stageFile;
    [SerializeField] GameObject[] prefabs;

    enum MAP_TYPE
    {
        MOUNTAIN_T_L,
        MOUNTAIN_T_C,
        MOUNTAIN_T_R,
        MOUNTAIN_M_L,
        MOUNTAIN_M_C,
        MOUNTAIN_M_R,
        MOUNTAIN_B_L,
        MOUNTAIN_B_C,
        MOUNTAIN_B_R,
    }
    MAP_TYPE[,] mapTable;
    float mapSize;
    Vector2 centerPos;


    void Start()
    {
        LoadMapData();
        CreateStage();
    }

    void LoadMapData()
    {
        string[] lines = stageFile.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        int row = lines.Length;
        int col = lines[0].Split(new[] { ',' }).Length;
        mapTable = new MAP_TYPE[col, row];
        for(int y = 0;y < row; y++)
        {
            string[] values = lines[y].Split(new[] { ',' });
            for(int x = 0;x < col; x++)
            {
                mapTable[x, y] = (MAP_TYPE)int.Parse(values[x]);
              //  Debug.Log(values[x]);
            }
        }
    }

    void CreateStage()
    {
        mapSize = prefabs[0].GetComponent<SpriteRenderer>().bounds.size.x;
        centerPos.x = (mapTable.GetLength(0) / 2 * mapSize);
        centerPos.y = (mapTable.GetLength(1) / 2 * mapSize);
        for(int y = 0; y < mapTable.GetLength(1); y++)
        {
            for(int x = 0; x < mapTable.GetLength(0); x++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                int mapType = (int)mapTable[x, y];
                if(mapType > 0)
                {
                    GameObject obj = Instantiate(prefabs[mapType]);
                    obj.transform.position = GetScreenPosFromMapTable(pos);
                    //obj.transform.position = new Vector2(x, y);
                }
                
            }
        }
    }

    Vector2 GetScreenPosFromMapTable(Vector2Int pos)
    {
        return new Vector2(pos.x * mapSize - centerPos.x + 0.25f, -(pos.y * mapSize - centerPos.y - 8));
    }
}
