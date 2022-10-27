using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakWall : MonoBehaviour
{
    BoxCollider2D bc2d;
    SpriteRenderer sp;
    [SerializeField] Sprite[] bomSprites;
    [SerializeField] Sprite brokenSprite;
    void Start()
    {
        bc2d = GetComponent<BoxCollider2D>();
        sp = GetComponent<SpriteRenderer>();

    }

    public void StartBreakWall()
    {
        StartCoroutine(_breakWall());
    }

    IEnumerator _breakWall()
    {
        SoundManager.I.PlaySE(SESoundData.SE.BOMWALL);
        gameObject.layer = 0;
        for (int i = 0; i < 5; i++)
        {
            
            sp.sprite = bomSprites[i % 2];
            yield return new WaitForSeconds(0.1f);
        }
        sp.sprite = brokenSprite;
        bc2d.enabled = false;
        
        
    }
}
