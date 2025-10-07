using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;


//this script is for only android platform
public class AdsManeger : MonoBehaviour
{

    public static AdsManeger instance;

    [Header("Interstitial Ad")]
    public bool enableInterstitial;
    [SerializeField] string _interstitialId;

    [Header("Native Ad")]
    public bool enableNativeAd;
    [SerializeField] string _nativeId;

    [Header("Reward Ad")]
    public bool enableRewardAd;
    [SerializeField] string _rewardId;

    [HideInInspector] public int numberOfCanShowRewardAdAfterEveryLoss;  // This is variable for change in script
    [SerializeField] int _numberOfCanShowRewardAdAfterEveryLoss;  // This is constant for reset

    [HideInInspector]
    public bool isNativeLoaded = false, isRewardAdLoaded = false, isInterstitialAdsLoaded = false;

    InterstitialAd _interstitialAd;
    UnifiedNativeAd _nativeAd;
    RewardedAd _rewardAd;

    [Header("Allowed number of ad requests after fail")]
    //Allowed number of ad requests after fail
    [SerializeField] int _allowOfRequestAds;  // This is constant for reset
    int _allowOfRequestNative2, _allowOfRequestReward2, _allowOfRequestInterstitial2;   // This is variable for change in script

    [Header("Native objects")]
    [SerializeField] RawImage _adChoicesIcon;
    [SerializeField] RawImage _adMainImage;

    [SerializeField] Text _adHeadline;
    [SerializeField] Text _adBodyText;
    [SerializeField] Text _adCallToAction;
    [SerializeField] Text _adAdvertiser;

    bool _isRewardReceived = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }



        isRewardAdLoaded = false;
        isInterstitialAdsLoaded = false;
        isNativeLoaded = false;


#if (UNITY_ANDROID && !UNITY_EDITOR)

        if (Application.internetReachability != NetworkReachability.NotReachable)
        {

            if (enableInterstitial)
            {
                RequestInterstitialAd();
            }

            if (enableRewardAd)
            {
                ResetNumberOfCanShowRewardVideoAfterEveryLoss();
                RequestRewardAd();
            }

            if (enableNativeAd)
            {
                RequestNativeAd();

            }
        }
#endif
    }



    #region Interstitial ad metods ---------------------------------------------------------------
    public void RequestInterstitialAd()
    {
        _allowOfRequestInterstitial2 = _allowOfRequestAds;
        StartCoroutine(sendRequestInterstitialAd());


    }
    public void ShowInterstitalAd()
    {
        _interstitialAd.Show();
    }

    IEnumerator sendRequestInterstitialAd()
    {
        yield return null;

        this._interstitialAd = new InterstitialAd(_interstitialId);
        _interstitialAd.OnAdLoaded += interstitialAd_OnAdLoaded;
        _interstitialAd.OnAdFailedToLoad += interstitialAd_OnAdFailedToLoad;
        _interstitialAd.OnAdClosed += interstitialAd_OnAdClosed;
        AdRequest adRequest = new AdRequest.Builder().Build();
        _interstitialAd.LoadAd(adRequest);

    }

    void interstitialAd_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        isInterstitialAdsLoaded = false;
        if (_allowOfRequestInterstitial2 > 0)
        {
            _allowOfRequestInterstitial2--;
            StartCoroutine(sendRequestInterstitialAd());
        }
    }
    void interstitialAd_OnAdLoaded(object sender, System.EventArgs e)
    {
        isInterstitialAdsLoaded = true;
    }
    void interstitialAd_OnAdClosed(object sender, System.EventArgs e)
    {


        if (!_interstitialAd.IsLoaded())
        {
            isInterstitialAdsLoaded = false;
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                StartCoroutine(sendRequestInterstitialAd());
            }
        }
    }

    #endregion -----------------------------------------------------------------------------------

    #region Native ad metods ---------------------------------------------------------------------
    public void RequestNativeAd()
    {
        _allowOfRequestNative2 = _allowOfRequestAds;
        StartCoroutine(sendRequestNativeAd());
    }
    void nativeAd_OnAdFailedToNativeAdLoad(object sender, AdFailedToLoadEventArgs e)
    {
        isNativeLoaded = false;
        // try
        if (_allowOfRequestNative2 > 0)
        {
            _allowOfRequestNative2--;
            StartCoroutine(sendRequestNativeAd());
        }
    }

    IEnumerator sendRequestNativeAd()
    {
        yield return null;

        AdLoader adLoader = new AdLoader.Builder(_nativeId).ForUnifiedNativeAd().Build();
        adLoader.OnUnifiedNativeAdLoaded += nativeAd_OnUnifiedNativeAdLoaded;
        adLoader.OnAdFailedToLoad += nativeAd_OnAdFailedToNativeAdLoad;
        adLoader.LoadAd(new AdRequest.Builder().Build());

    }
    void nativeAd_OnUnifiedNativeAdLoaded(object sender, UnifiedNativeAdEventArgs e)
    {
        isNativeLoaded = true;

        _nativeAd = e.nativeAd;

        _adChoicesIcon.texture = _nativeAd.GetAdChoicesLogoTexture();
        _adHeadline.text = _nativeAd.GetHeadlineText();
        _adBodyText.text = _nativeAd.GetBodyText();
        _adCallToAction.text = _nativeAd.GetCallToActionText();
        _adAdvertiser.text = _nativeAd.GetAdvertiserText();
        _adMainImage.texture = _nativeAd.GetImageTextures()[0];

        _nativeAd.RegisterAdChoicesLogoGameObject(_adChoicesIcon.gameObject);
        _nativeAd.RegisterHeadlineTextGameObject(_adHeadline.gameObject);
        _nativeAd.RegisterBodyTextGameObject(_adBodyText.gameObject);
        _nativeAd.RegisterAdvertiserTextGameObject(_adAdvertiser.gameObject);
        _nativeAd.RegisterCallToActionGameObject(_adCallToAction.gameObject);
        _nativeAd.RegisterImageGameObjects(new List<GameObject>() { _adMainImage.gameObject });

    }
    #endregion -----------------------------------------------------------------------------------

    #region Reward ad metods ---------------------------------------------------------------------
    public void ResetNumberOfCanShowRewardVideoAfterEveryLoss()
    {
        numberOfCanShowRewardAdAfterEveryLoss = _numberOfCanShowRewardAdAfterEveryLoss;
    }

    public void RequestRewardAd()
    {
        _allowOfRequestReward2 = _allowOfRequestAds;
        StartCoroutine(sendRequestRewardAd());
    }
    public void ShowRewardAd()
    {
        _rewardAd.Show();
    }

    IEnumerator sendRequestRewardAd()
    {
        yield return null;

        _rewardAd = new RewardedAd(_rewardId);
        _rewardAd.OnAdClosed += rewardAd_OnAdClosed;
        _rewardAd.OnAdLoaded += rewardAd_OnAdLoaded;
        _rewardAd.OnAdFailedToLoad += rewardAd_OnAdFailedToLoad;
        _rewardAd.OnAdFailedToShow += rewardAd_OnAdFailedToShow;
        _rewardAd.OnUserEarnedReward += rewardAd_OnUserEarnedReward;

        AdRequest adRequest = new AdRequest.Builder().Build();
        _rewardAd.LoadAd(adRequest);

    }

    void rewardAd_OnUserEarnedReward(object sender, Reward e)
    {
        _isRewardReceived = true;
    }

    void rewardAd_OnAdFailedToShow(object sender, AdErrorEventArgs e)
    {
        UIManeger.Instance.HideLossMenu();
        UIManeger.Instance.ShowHomeMenu();

        if (!_rewardAd.IsLoaded())
        {
            isRewardAdLoaded = false;
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                StartCoroutine(sendRequestRewardAd());
            }
        }
    }

    void rewardAd_OnAdFailedToLoad(object sender, AdErrorEventArgs e)
    {
        isRewardAdLoaded = false;
        // try  
        if (_allowOfRequestReward2 > 0)
        {
            _allowOfRequestReward2--;
            StartCoroutine(sendRequestRewardAd());
        }
    }

    void rewardAd_OnAdLoaded(object sender, System.EventArgs e)
    {
        isRewardAdLoaded = true;
    }

    void rewardAd_OnAdClosed(object sender, System.EventArgs e)
    {


        if (_isRewardReceived)
        {
            UIManeger.Instance.HideLossMenu();
            GameManager.Instance.Continue();
            UIManeger.Instance.ShowGameplay();
            _isRewardReceived = false;
        }
        else
        {
            UIManeger.Instance.HideLossMenu();
            UIManeger.Instance.ShowHomeMenu();
        }

        if (!_rewardAd.IsLoaded())
        {
            isRewardAdLoaded = false;
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                StartCoroutine(sendRequestRewardAd());
            }
        }
    }
    #endregion -----------------------------------------------------------------------------------

}
