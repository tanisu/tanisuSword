using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StageController : MonoBehaviour
{
    [SerializeField] public ObjectPool playerBulletPool = default;
    [SerializeField] public ObjectPool enemyBulletPool = default;
    [SerializeField] public PlayerController player = default;
    
    [SerializeField] float stageSpeed = 0.5f;
    [SerializeField] UIController ui;


    private static StageController i;
    public static StageController I { get => i; }
    public enum PlayStopCodeDef
    {
        PlayerDead,
        BossDefeat,
    }
    public PlayStopCodeDef playStopCode;

    public bool isStop;

    private void Awake()
    {
        i = GetComponent<StageController>();
    }
    void Start()
    {
    }

    
    void Update()
    {
        if (player.isDead)
        {
            playStopCode = PlayStopCodeDef.PlayerDead;
            enemyBulletPool.ResetAll();
            stopScroll();
        }

        

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
        stageSpeed = 0.0f;
    }

    public void ReScroll()
    {
        isStop = false;
        stageSpeed = 0.1f;
    }
}
