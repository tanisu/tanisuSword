using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [SerializeField] public ObjectPool playerBulletPool = default;
    [SerializeField] public PlayerController player = default;
    [SerializeField] float stageSpeed = 0.5f;
    private static StageController i;
    public static StageController I { get => i; }
    public enum PlayStopCodeDef
    {
        PlayerDead,
        BossDefeat,
    }
    public PlayStopCodeDef playStopCode;

    private void Awake()
    {
        i = GetComponent<StageController>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isDead)
        {
            playStopCode = PlayStopCodeDef.PlayerDead;
            stageSpeed = 0;
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
}
