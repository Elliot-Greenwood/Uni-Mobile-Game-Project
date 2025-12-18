using UnityEngine;
using System.Collections;

public class AddManager : MonoBehaviour
{
    public AdsINNIT InitializeAd;
    public Banner_ads BannerAd;
    public interstitial_ads InterstitialAd;
    public Reward_Ads RewardAd;


    public static AddManager Instance { get; private set;}

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;

        }
        Instance = this;
        DontDestroyOnLoad(gameObject);



        BannerAd.LoadBanner();
        InterstitialAd.LoadAd();
        RewardAd.LoadRewardAd();

    }


}
