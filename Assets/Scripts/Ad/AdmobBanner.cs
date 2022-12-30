using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdmobBanner : MonoBehaviour
{
    private BannerView bannerView;


    private void Start()
    {
//        MobileAds.Initialize(initStatus => { });

    }

    public void Banner()
    {
        RequestBanner();
    }

    public void HideBanner()
    {
        bannerView.Destroy();
    }


    private void RequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-1205963622209231/8913310481";
#else
            string adUnitId = "unexpcted_platform";
#endif
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        AdRequest request = new AdRequest.Builder().Build();
        bannerView.LoadAd(request);
    }
}
