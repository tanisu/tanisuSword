using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    [SerializeField] List<GameObject> enemies,tmpEnemies;



    public void ShowEnemies()
    {
        
        foreach (GameObject enemy in enemies)
        {
            GameObject clone = Instantiate(enemy,transform);
            tmpEnemies.Add(clone);
            clone.SetActive(true);
        }
    }

    public void ResetEnemies()
    {
        foreach(GameObject enemy in tmpEnemies)
        {
            if (enemy)
            {
                enemy.GetComponent<Enemy>().HideFromStage();
            }
        }
        tmpEnemies.Clear();
    }
}
