using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase : MonoBehaviour
{

    protected void Start()
    {
        StageController.I.canShoot = false;
    }

    public void StopBGM()
    {
        SoundManager.I.StopBGM();
    }

    public void StartBossBGM()
    {
        StageController.I.canShoot = true;
        SoundManager.I.PlayBGM(BGMSoundData.BGM.BOSS);
    }

    protected void DeadStart()
    {
        StageController.I.canShoot = false;
        SoundManager.I.PlaySE(SESoundData.SE.BOSS_EXPLOSION);

    }

    

    protected void Dead()
    {
        
        PlayerPrefs.SetInt("stageNumber", StageController.I.currentStage);

        StageController.I.BossDead();
    }

    protected void Dead_C()
    {
        StageController.I.canShoot = true;
        StageController.I.ReScroll(true);
    }
}
