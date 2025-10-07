using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [HideInInspector] public int _score = 0;
    private int _bestScore = 0;

    private bool _isNewScoreRecorded = false;
    const string BEST_SCORE_KEY = "best score";

    //for send set creater hail for color to gray and save color main for ui control

    PlayerPrefsManeger _playerPrefs;
    UIManeger _UiManeger;
    HailCreater _hailCreater;
    SupriseManeger _supriseManeger;
    HailReceiveManeger _HailReceiveManeger;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _hailCreater = HailCreater.Instance;
        _HailReceiveManeger = HailReceiveManeger.Instance;
        _supriseManeger = SupriseManeger.Instance;
        _playerPrefs = new PlayerPrefsManeger();
        _UiManeger = UIManeger.Instance;
        loadPrefs();
        _UiManeger.ShowHomeMenuWhenGameLaunching();
    }

    void loadPrefs()
    {
        _bestScore = _playerPrefs.LoadInt(BEST_SCORE_KEY);
    }

    public void NewScoreReceived(int newReceiveScore)
    {
        _score += newReceiveScore;
        _UiManeger.SetScoreTxt(_score);
        if (_score > _bestScore)
        {
            _bestScore = _score;
            _playerPrefs.SaveInt(BEST_SCORE_KEY, _bestScore);
            _isNewScoreRecorded = true;
        }

        LevelDesigner.Instance.GoToLevel(_score);
    }

    public void Loss()
    {

        AudioAndVibrationManeger.Instance.VibrateWhenLoss();

        _HailReceiveManeger.canTouchForControlGamePlay = false;
        _HailReceiveManeger.GoAllChildToGrayWithOutTheCollidingObject();

        _supriseManeger.SetCanCreateSuprise(false);
        _supriseManeger.GoAllChildToGray();

        _hailCreater.SetCanCreateHail(false);
        _hailCreater.SetCanMoving(false);
        _hailCreater.GoAllChildToGrayWithOutTheCollidingObject();


        if (_isNewScoreRecorded & _bestScore > 4)   // if new recorded
        {
            _UiManeger.ShowLossMenuWhenNewRecordReceived(_bestScore);
            _isNewScoreRecorded = false;
        }
        else
        {
            if (AdsManeger.instance.isRewardAdLoaded && AdsManeger.instance.numberOfCanShowRewardAdAfterEveryLoss > 0)
            {
                // if ads loaded
                AdsManeger.instance.numberOfCanShowRewardAdAfterEveryLoss--;
                _UiManeger.ShowLossMenuWithCanWatchAds(_score, _bestScore);
            }
            else   //normal
            {
                _UiManeger.ShowLossMenu(_score, _bestScore);
            }
        }
    }
    public void Pause()
    {
        _HailReceiveManeger.canTouchForControlGamePlay = false;
        _HailReceiveManeger.GoAllChildToGray();

        _hailCreater.SetCanCreateHail(false);
        _hailCreater.SetCanMoving(false);
        _hailCreater.GoAllChildToGray();
        _supriseManeger.SetCanCreateSuprise(false);
        _supriseManeger.GoAllChildToGray();

    }

    public void Resume()
    {
        _HailReceiveManeger.GoAllChildToMainColor(true);
        _hailCreater.GoAllChildToMainColor();
        _supriseManeger.GoAllChildToMainColor();

    }

    public void Continue()
    {
        _hailCreater.HailCreaterRestart();
        _HailReceiveManeger.GoAllChildToMainColorWithOutTheCollidingObject();
        _supriseManeger.destroyChildeSuprise();
        _HailReceiveManeger.resetPositionHailReciever();
    }


    public void Restart()
    {
        _score = 0;
        _UiManeger.SetScoreTxt(_score);
        LevelDesigner.Instance.ResetToFirstLevel();
        HailCreater.Instance.changePaletteColor(true);
        _HailReceiveManeger.GoAllChildToMainColor(false);
        _HailReceiveManeger.canTouchForControlGamePlay = true;
        _HailReceiveManeger.resetPositionHailReciever();
        AudioAndVibrationManeger.Instance.ResetPlayingSpeed("Background Gameplay");
        // Ads ----------------------------------------------------------
#if (UNITY_ANDROID && !UNITY_EDITOR)
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            AdsManeger adsManeger = AdsManeger.instance;
            if (adsManeger.enableRewardAd)
            {
                adsManeger.ResetNumberOfCanShowRewardVideoAfterEveryLoss();

                if (!adsManeger.isRewardAdLoaded)
                    adsManeger.RequestRewardAd();
            }
            if (adsManeger.enableInterstitial && !adsManeger.isInterstitialAdsLoaded)
                adsManeger.RequestInterstitialAd();
            if (adsManeger.enableNativeAd && !adsManeger.isNativeLoaded)
                adsManeger.RequestNativeAd();
        }
#endif

    }

    /// <summary>
    /// start play gameplay(called in UIManager.CS)
    /// </summary>
    public void Play()
    {
        _HailReceiveManeger.canTouchForControlGamePlay = true;
        _hailCreater.SetCanCreateHail(true);
        _hailCreater.SetCanMoving(true);
        _supriseManeger.SetCanCreateSuprise(true);
    }

}
