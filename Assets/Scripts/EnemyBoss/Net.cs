using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Net : MonoBehaviour
{
    public bool catchPlayer;
    SpriteRenderer sp;
    Spider spider;
    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        spider = transform.parent.GetComponent<Spider>();
        _shot();
    }

 
    private void _shot()
    {
        transform.SetParent(spider.transform.parent);
        transform.DOMove(StageController.I.player.transform.position, 0.5f).SetLink(gameObject).OnComplete(()=> {
            sp.DOColor(new Color(0, 0, 0, 0), 1f).SetEase(Ease.Flash,21).OnComplete(()=> {
                StageController.I.player.EscapeHole();
                Destroy(gameObject);
            });
        }) ;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !spider.isAttack)
        {
            StageController.I.player.CatchedHole();
            spider.CatchPlayer(transform.position);
        }
    }


}
