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
    float stageProggresTime = 0;
    [SerializeField] UIController ui;
    float tmpStageSpeed;


    private static StageController i;
    public static StageController I { get => i; }
    public enum PlayStopCodeDef
    {
        PlayerDead,
        BossDefeat,
    }
    public PlayStopCodeDef playStopCode;

    public bool isStop;
    public bool isPlaying;
    private void Awake()
    {
        i = GetComponent<StageController>();
    }

    private void Start()
    {
        stageSeq.Load();
        stageSeq.Reset();
        stageProggresTime = 0;
      //  isPlaying = false;
    }


    void Update()
    {
       // if (!isPlaying) return;
        if (player.isDead)
        {
            playStopCode = PlayStopCodeDef.PlayerDead;
            enemyBulletPool.ResetAll();
            stopScroll();
            ui.ViewGameOverPanel();
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

    public void Retry()
    {
        
        SceneManager.LoadScene(0);
    }
}
