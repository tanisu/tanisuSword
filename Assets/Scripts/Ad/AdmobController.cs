using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;

public class AdmobController : MonoBehaviour
{
    private AdmobBanner banner;
    private AdmobInter inter;
    private AdmobReward reward;
    private string sceneName;

    public static AdmobController I;
    bool isSceneChange = false;
    bool isHp = false;

    private void Awake()
    {
        if(I == null)
        {
            I = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    void Start()
    {
        banner = GetComponent<AdmobBanner>();
        inter = GetComponent<AdmobInter>();
        reward = GetComponent<AdmobReward>();
        inter.CloseAd = _endAd;
        reward.CloseAd = _endAd;
        MobileAds.Initialize(initStatus => { });
        
    }


    private void Update()
    {
        if (isSceneChange)
        {
            if (isHp)
            {
                StageController.I.HpKaihuku();
                isHp = false;
            }
            else
            {
                SceneManager.LoadScene(sceneName);
            }
            
            isSceneChange = false;
        }
    }

    public void ShowBanner()
    {
        banner.Banner();
    }

    public void HideBanner()
    {
        banner.HideBanner();
    }

    public void ShowInter(string _sceneName)
    {
        sceneName = _sceneName;
        inter.Inter();
    }

    private void _endAd()
    {
        isSceneChange = true;
    }

    public void ShowReward(string _sceneName)
    {
        sceneName = _sceneName;
        reward.Reward();
    }

    public void ShowRewardHp()
    {
        isHp = true;
        reward.Reward();

    }

}
