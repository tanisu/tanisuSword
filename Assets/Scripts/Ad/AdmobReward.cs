using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.Events;

public class AdmobReward : MonoBehaviour
{
    bool rewardFlag = false;
    bool closeFlag = false;
    private RewardedAd rewarded;
    private string adUnitId;

    public UnityAction CloseAd;

    void Start()
    {
#if UNITY_ANDROID
        adUnitId = "ca-app-pub-1205963622209231/4729511138";
#else
        adUnitId = "";
#endif
        CreateAndLoadRewardedAd();
    }

    void Update()
    {
        if (closeFlag)
        {
            if (!rewardFlag)
            {
                GameManager.I.ResetParams();
            }
            rewardFlag = false;
            closeFlag = false;
            CloseAd?.Invoke();
        }
        
    }

    private void CreateAndLoadRewardedAd()
    {
        rewarded = new RewardedAd(adUnitId);
        rewarded.OnAdLoaded += HandleRewardedAdLoaded;
        rewarded.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        rewarded.OnAdClosed += HandleRewardedAdClosed;
        rewarded.OnUserEarnedReward += HandleUserEarnedReward;

        AdRequest request = new AdRequest.Builder().Build();
        rewarded.LoadAd(request);
    }

    public void HandleRewardedAdLoaded(object sender,EventArgs args)
    {

    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {

    }


    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        closeFlag = true;
        CreateAndLoadRewardedAd();
        
    }

    public void HandleUserEarnedReward(object sender,Reward args)
    {
        
        rewardFlag = true;
    }

    public void Reward()
    {
        if(rewarded.IsLoaded() == true)
        {
            rewarded.Show();
        }
    }

}
