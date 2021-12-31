using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "StageSeq")]
public class StageSeq : ScriptableObject
{
    [SerializeField] string filename = "";
    public enum COMMANDTYPE
    {
        SETSPEED,
        PUTENEMY
    }
    static readonly Dictionary<string, COMMANDTYPE> commandList =
        new Dictionary<string, COMMANDTYPE>()
        {
            { "SETSPEED",COMMANDTYPE.SETSPEED},
            { "PUTENEMY",COMMANDTYPE.PUTENEMY},
        };
    public struct StageData
    {
        public readonly float eventPos;
        public readonly COMMANDTYPE command;
        public readonly float arg1, arg2;
        public readonly uint arg3;
        public readonly uint arg4;
        public StageData(float _eventPos,string _command, float _x,float _y,uint _type,uint _enemyType)
        {
            eventPos = _eventPos;
            command = commandList[_command];
            arg1 = _x;
            arg2 = _y;
            arg3 = _type;
            arg4 = _enemyType;

        }
    }
    StageData[] stageDatas;
    int stageDataIdx = 0;
    [SerializeField] GameObject[] enemyPrefabs = default;

    //ì«Ç›çûÇ›Ç±Ç±Ç©ÇÁ
    public void Load()
    {
        Dictionary<string, uint> revarr = new Dictionary<string, uint>();
        for(uint i = 0; i < enemyPrefabs.Length; i++)
        {
            revarr.Add(enemyPrefabs[i].name, i);
        }

        List<StageData> stageCsvData = new List<StageData>();
        string csvData = Resources.Load<TextAsset>(filename).text;
        StringReader sr = new StringReader(csvData);
        while(sr.Peek() != -1)
        {
            string line = sr.ReadLine();
            string[] cols = line.Split(",");
            if (cols.Length != 6) continue;

            stageCsvData.Add(
                new StageData(
                    float.Parse(cols[0]),
                    cols[1],
                    float.Parse(cols[2]),
                    float.Parse(cols[3]),
                    revarr.ContainsKey(cols[4]) ? revarr[cols[4]] : 0,
                    uint.Parse(cols[5])//îÚÇ‘ìGÇ©Ç«Ç§Ç©îªíË
                    )
                );
        }
        stageDatas = stageCsvData.OrderBy(i => i.eventPos).ToArray();
    }

    public void Reset()
    {
        stageDataIdx = 0;
    }

    public void Step(float _stageProgressTime)
    {
        while (stageDataIdx < stageDatas.Length && 
            stageDatas[stageDataIdx].eventPos <= _stageProgressTime)
        {
            switch (stageDatas[stageDataIdx].command)
            {
                case COMMANDTYPE.SETSPEED:
                    StageController.I.stageSpeed = stageDatas[stageDataIdx].arg1;
                    break;
                case COMMANDTYPE.PUTENEMY:
                    GameObject enemyTmp = Instantiate(enemyPrefabs[stageDatas[stageDataIdx].arg3]);
                    if(stageDatas[stageDataIdx].arg4 <= 0)
                    {
                        enemyTmp.transform.parent = StageController.I.enemyFling;
                    }
                    else
                    {
                        enemyTmp.transform.parent = StageController.I.enemyGround;
                    }
                    
                    enemyTmp.transform.localPosition = new Vector3(
                        stageDatas[stageDataIdx].arg1,
                        stageDatas[stageDataIdx].arg2
                        );
                    break;
            }
            ++stageDataIdx;
        }
    }

}
