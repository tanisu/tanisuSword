using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StageController : MonoBehaviour
{
    [SerializeField] public ObjectPool playerBulletPool = default;
    [SerializeField] public ObjectPool enemyBulletPool = default;
    [SerializeField] public Transform enemyFling = default;
    [SerializeField] public Transform enemyGround = default;

    [SerializeField] StageSeq stageSeq = default;

    [SerializeField] public PlayerController player = default;
    
    [SerializeField] public float stageSpeed;
    [SerializeField] GoalMapChips goalPanel;

    float stageProggresTime = 0;
    [SerializeField] UIController ui;
    float tmpStageSpeed;
    public int currentStage;
    private int nextStage;

    private static StageController i;
    public static StageController I { get => i; }
    public enum PlayStopCodeDef
    {
        NotDead,
        PlayerDead,
        BossDefeat,
    }
    public PlayStopCodeDef playStopCode;

    public bool isStop;
    public bool isPlaying;
    private bool isIdle;

    private void Awake()
    {
        i = GetComponent<StageController>();
    }

    private void Start()
    {
        stageSeq.Load();
        stageSeq.Reset();
        stageProggresTime = 0;
        isPlaying = true;
        isIdle = true;
        if(currentStage == 0)
        {
            string currentScene = SceneManager.GetActiveScene().name;
            int len = currentScene.Length;
            string currentStr = currentScene.Substring(len - 1);
            currentStage = int.Parse(currentStr);
        }
        nextStage = currentStage + 1;
        StartCoroutine(ShowStagePanel());
    }

    IEnumerator ShowStagePanel()
    {
        SoundManager.I.StopBGM();
        SoundManager.I.LoopSwitch();
        SoundManager.I.PlayBGM(BGMSoundData.BGM.INTORO);
        yield return new WaitForSeconds(2.0f);
        ui.HideStartPanel();
        isIdle = false;
        SoundManager.I.PlayBGM(BGMSoundData.BGM.STAGE);
        SoundManager.I.LoopSwitch();
        
    }


    void Update()
    {
        if (isIdle)
        {
            return;
        }
        if (player.isDead && isPlaying)
        {
            isPlaying = false;
            playStopCode = PlayStopCodeDef.PlayerDead;
            enemyBulletPool.ResetAll();
            stopScroll();
            ui.ViewGameOverPanel();
            return;
        }
        if(playStopCode == PlayStopCodeDef.PlayerDead)
        {
            return;
        }
        
        stageSeq.Step(stageProggresTime);
        stageProggresTime += Time.deltaTime;
        

        
 
        transform.Translate(Vector3.up * Time.deltaTime * stageSpeed);

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        player.Move(new Vector3(x,y,0));


        if (Input.GetKey(KeyCode.Space))
        {
            player.Shot();
            
        }
    }

    public void UpdateHp(int hp)
    {
        ui.UpdateSlider(hp);
    }

    public void BossDead()
    {
        playStopCode = PlayStopCodeDef.BossDefeat;
        StartCoroutine(ViewGoalAction());
    }

    IEnumerator ViewGoalAction()
    {
        yield return new WaitForSeconds(0.5f);
        isStop = true;
        goalPanel.ViewGoal();
        yield return new WaitForSeconds(0.5f);
        isStop = false;
    }

    public void ViewStageClear()
    {
        ui.ViewStageClearPanel();
    }

    public void stopScroll()
    {
        //Debug.Log("StopScroll");
        isStop = true;
        tmpStageSpeed = stageSpeed;
        stageSpeed = 0.0f;
    }

    public void ReScroll()
    {
        //Debug.Log("ReScroll");
        if (player.isDead)
        {
            return;
        }
        
        isStop = false;
        stageSpeed = tmpStageSpeed;
    }

    public void PlayerHasShilde()
    {
        player.HasShilde();
    }

    public void NextStage()
    {
        SceneManager.LoadScene($"stage0{nextStage}");
    }


    public void Retry()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
