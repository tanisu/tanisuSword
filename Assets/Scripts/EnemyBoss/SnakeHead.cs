using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SnakeHead : Enemy
{
    public Transform target;

    public UnityAction OnPlayer,IsDead;
    public UnityAction<int> HitDamage,ToNext;

    //Animator attackAnim;
    
    private void Start()
    {
        
        anim = GetComponent<Animator>();
        bulletPool = StageController.I.enemyBulletPool;
        base.sp = GetComponent<SpriteRenderer>();
        //Debug.Log(attackAnim);
        
    }

    void Update()
    {
        transform.position = target.position;
        
    }

    protected override void DelEnemyHp(int damage, SpriteRenderer sp)
    {
        base.DelEnemyHp(damage, base.sp);
        HitDamage?.Invoke(hp);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Player"))
        {
            OnPlayer?.Invoke();

            
        }

    }

    public void Attack()
    {
        
        anim.SetTrigger("Attack");
    }

    public void EndAttack()
    {
        anim.SetTrigger("Attack");
        ToNext?.Invoke(hp);
    }

    public void IsDeadHead()
    {
        StageController.I.canShoot = false;
        SoundManager.I.StopBGM();
        SoundManager.I.PlaySE(SESoundData.SE.BOSS_EXPLOSION);
        
        IsDead?.Invoke();
    }


    public void DeadMusic()
    {
        SoundManager.I.PlayBGM(BGMSoundData.BGM.SADENDING);
        _changeGirl();
    }

    private void _changeGirl()
    {
        gameObject.tag = "Girl";
        

    }



}
