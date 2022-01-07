using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DummyBtn : MonoBehaviour
{
    
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(()=> {
            SceneManager.LoadScene("Title");
        });
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
