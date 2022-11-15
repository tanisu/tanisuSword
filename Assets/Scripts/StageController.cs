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
    [SerializeField] GameObject rain;
    [SerializeField] TilemapController tilemap;
    [SerializeField] FakeEndingController fakeEnd;

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

    public bool isStop, isPlaying,canShoot,isStopOnly;
    
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
        Debug.Log(nextStage);
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
        if (isIdle) return;
        
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

        if(playStopCode == PlayStopCodeDef.PlayerDead) return;


        stageSeq.Step(stageProggresTime);
        if (!isStop && !isStopOnly)
        {
            stageProggresTime += Time.deltaTime;
        }

        
 
        transform.Translate(Vector3.up * Time.deltaTime * stageSpeed);

   
        float x = 0;
        float y = 0;

        
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        
        
        //if (d_joystick.gameObject.activeSelf == true)
        //{
        //    x = d_joystick.Horizontal;
        //    y = d_joystick.Vertical;
        //}
        //else if (f_joystick.gameObject.activeSelf == true)
        //{           
        //    x = f_joystick.Horizontal;
        //    y = f_joystick.Vertical;
        //}else if(fix_joystick.gameObject.activeSelf == true)
        //{
        //    x = fix_joystick.Horizontal;
        //    y = fix_joystick.Vertical;
        //}



        player.Move(new Vector3(x, y, 0));

        

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
        SoundManager.I.LoopSwitch();
        SoundManager.I.PlayBGM(BGMSoundData.BGM.STAGECLEAR);
        
        
        
        yield return new WaitForSeconds(6.9f);
        SoundManager.I.StopBGM();
        SoundManager.I.LoopSwitch();
        yield return new WaitForSeconds(0.5f);

        goalPanel.ViewGoal();
        yield return new WaitForSeconds(0.5f);
        
        
        
    }

    public void ViewStageClear()
    {
        ui.ViewStageClearPanel();
    }

    public void stopScroll(bool _only = false)
    {
        if (!_only)
        {
            isStop = true;
        }
        else
        {
            isStopOnly = true;
        }
        
        
        tmpStageSpeed = stageSpeed;
        stageSpeed = 0.0f;
    }

    

    public void ReScroll(bool _only = false)
    {
        
        if (player.isDead)
        {
            return;
        }
        if (!_only)
        {
            isStop = false;
        }
        else
        {
            
            SoundManager.I.PlayBGM(BGMSoundData.BGM.STAGE);
            isStopOnly = false;
            canShoot = true;
        }
        
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

    public void ToTitle()
    {
        GameManager.I.ResetParams();
        SceneManager.LoadScene("Title");
    }

    public void DoLight(bool domask = false)
    {
        coroutine = StartCoroutine(_light(domask));
    }

    public void StopRain()
    {
        RainController[] rains = rain.GetComponentsInChildren<RainController>();
        BGController[] bGs = tilemap.GetComponentsInChildren<BGController>();
        foreach(BGController bg in bGs)
        {
            bg.ChangeColor();
        }
        foreach(RainController _rain in rains)
        {
            _rain.StopRain();
        }
        SoundManager.I.FadeOutBGM();
    }


    public void FakeEndStart()
    {
        fakeEnd.gameObject.SetActive(true);
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
