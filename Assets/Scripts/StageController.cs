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
    [SerializeField] SpriteRenderer mask = default;
    [SerializeField] StageSeq stageSeq = default;

    [SerializeField] public PlayerController player = default;
    
    [SerializeField] public float stageSpeed;
    [SerializeField] GoalMapChips goalPanel;
    [SerializeField] DynamicJoystick d_joystick;
    [SerializeField] FloatingJoystick f_joystick;
    [SerializeField] FixedJoystick  fix_joystick;

    float stageProggresTime = 0;
    [SerializeField] UIController ui;
    float tmpStageSpeed;
    public int currentStage;
    int nextStage;

    

    private static StageController i;
    public static StageController I { get => i; }
    public enum PlayStopCodeDef
    {
        NotDead,
        PlayerDead,
        BossDefeat,
    }
    public PlayStopCodeDef playStopCode;

    public bool isStop, isPlaying,canShoot;
    
    public bool isIdle { get; private set; }

    Coroutine coroutine;

    private void Awake()
    {
        if(i == null)
        {
            i = this;
        }
        
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
        canShoot = true;
    }

    IEnumerator ShowStagePanel()
    {
        SoundManager.I.LoopSwitch();
        SoundManager.I.PlayBGM(BGMSoundData.BGM.INTORO);
        yield return new WaitForSeconds(2.0f);
        ui.HideStartPanel();
        isIdle = false;
        SoundManager.I.PlayBGM(BGMSoundData.BGM.STAGE);
        SoundManager.I.LoopSwitch();
        if (currentStage > 1 && GameManager.I.currentSpeed > 0)
        {
            player.SetParams(GameManager.I.currentShootInterval, GameManager.I.currentSpeed, GameManager.I.currentHasShilde);
        }

    }


    void Update()
    {
        if (isIdle)
        {
            return;
        }
        if (player.isDead && isPlaying)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            d_joystick.gameObject.SetActive(false);
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
        if (!isStop)
        {
            stageProggresTime += Time.deltaTime;
        }
        
 
        transform.Translate(Vector3.up * Time.deltaTime * stageSpeed);

   
        float x = 0;
        float y = 0;


        if (d_joystick.gameObject.activeSelf == true)
        {
            x = d_joystick.Horizontal;
            y = d_joystick.Vertical;
        }
        else if (f_joystick.gameObject.activeSelf == true)
        {           
            x = f_joystick.Horizontal;
            y = f_joystick.Vertical;
        }else if(fix_joystick.gameObject.activeSelf == true)
        {
            x = fix_joystick.Horizontal;
            y = fix_joystick.Vertical;
        }

        player.Move(new Vector3(x,y,0));

        if (canShoot)
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
        
        isStop = true;
        tmpStageSpeed = stageSpeed;
        stageSpeed = 0.0f;
    }

    public void ReScroll()
    {
        
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
        GameManager.I.ResetParams();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void DoLight(bool domask = false)
    {
        coroutine = StartCoroutine(_light(domask));
    }

    IEnumerator _light(bool _domask)
    {
        float a = mask.color.a;
        
        while (a > 0)
        {
            a -= 0.2f;
            mask.color = new Color(0, 0, 0, a);
            yield return new WaitForSeconds(0.3f);
        }

        if (_domask)
        {
            yield return new WaitForSeconds(3f);
            while(a <= 1)
            {
                a += 0.2f;
                mask.color = new Color(0, 0, 0, a);
                yield return new WaitForSeconds(0.3f);
            }
        }
    }

}
