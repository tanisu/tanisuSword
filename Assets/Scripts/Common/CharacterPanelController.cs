using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterPanelController : MonoBehaviour
{

    
    float angle = 0f;
    [SerializeField] CharacterDetail frontPanel, backPanel;
    [SerializeField] EndrollDatas[] endrollDatas;
    [SerializeField] GameObject textObj;
    public UnityAction MoveUp;

    void Start()
    {

        StartCoroutine(_rotatePanel());
    }

    IEnumerator _rotatePanel()
    {
        yield return new  WaitForSeconds(3f);


        for (int i = 0; i < endrollDatas.Length; i++)
        {


            for (int x = 0;x <= 180; x++)
            {
                angle += 1;
                
                transform.eulerAngles = new Vector3(angle, 0f);
                if(angle == 90)
                {
                    if(i == 0)
                    {
                        textObj.SetActive(false);
                    }
                    frontPanel.gameObject.SetActive(false);
                    backPanel.gameObject.SetActive(true);
                    backPanel.ChangeCharactor(endrollDatas[i].characterName, endrollDatas[i].characterSprite,endrollDatas[i].anim);
                }
                if(angle == 270)
                {
                    frontPanel.gameObject.SetActive(true);
                    backPanel.gameObject.SetActive(false);
                    frontPanel.ChangeCharactor(endrollDatas[i].characterName, endrollDatas[i].characterSprite, endrollDatas[i].anim);
                }
                if(angle % 180 == 0)
                {
                    yield return new WaitForSeconds(4.5f);
                }
                if(angle == 360)
                {
                    angle = 0;
                }
                yield return new WaitForSeconds(0.01f);

            }
        }

        MoveUp?.Invoke();     

    }



}
