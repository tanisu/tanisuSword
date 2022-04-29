using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    [SerializeField] Sprite onSwitch;
    //[SerializeField] bool isDummy;
    BreakWall[] breakWalls;
    Enemy[] enemies;
    BoxCollider2D bc2d;
    SpriteRenderer sp;
    void Start()
    {
        enemies = GetComponentsInChildren<Enemy>(true);
        breakWalls = GetComponentsInChildren<BreakWall>();
        bc2d = GetComponent<BoxCollider2D>();
        sp = GetComponent<SpriteRenderer>();
    }

    public void OnSwitch()
    {
        bc2d.enabled = false;
        sp.sprite = onSwitch;
        StartCoroutine(_breakStart());
        SoundManager.I.PlaySE(SESoundData.SE.SWITCH);

    }

    IEnumerator _breakStart()
    {
        if (breakWalls.Length > 0)
        {
            foreach (BreakWall bw in breakWalls)
            {
                bw.StartBreakWall();
                yield return new WaitForSeconds(0.1f);
            }
        }
        if(enemies.Length > 0)
        {
            foreach(Enemy enemy in enemies)
            {
                enemy.gameObject.SetActive(true);
                //yield return null;
                //yield return new WaitForSeconds(0.1f);
            }
        }
        
    }
}
