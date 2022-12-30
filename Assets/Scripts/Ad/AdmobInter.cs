using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.Events;

public class AdmobInter : MonoBehaviour
{
    private InterstitialAd inter;
    public UnityAction CloseAd;


    void Start()
    {
        RequestInter();
    }

    public void Inter()
    {
        if (inter.IsLoaded() == true)
        {
            inter.Show();
        }

    }


    private void RequestInter()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-1205963622209231/9241485685";
#else
            string adUnitId = "unexpcted_platform";
#endif
        inter = new InterstitialAd(adUnitId);

        inter.OnAdLoaded += HandleOnAdLoaded;
        inter.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        inter.OnAdClosed += HandleOnAdClose;

        AdRequest request = new AdRequest.Builder().Build();
        inter.LoadAd(request);
    }

    public void HandleOnAdLoaded(object sender , EventArgs args)
    {

    }
    

    public void HandleOnAdFailedToLoad(object sender ,AdFailedToLoadEventArgs args)
    {

    }
    public void HandleOnAdClose(object sender ,EventArgs args)
    {
        inter.Destroy();
        RequestInter();
        CloseAd?.Invoke();
    }


}
