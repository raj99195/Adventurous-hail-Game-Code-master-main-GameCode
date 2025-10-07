using UnityEngine;
using UnityEngine.UI;

public class UIManeger : MonoBehaviour
{
    public static UIManeger Instance;

    [Header("Link of Instagram Page")]
    [SerializeField] string _instagramPageLink;
    
    public bool isHomeMenuShowed { set; get; } = false;
    public bool isLossMenuShowed { set; get; } = false;
    public bool isPauseMenuShowed { set; get; } = false;
    public bool isSettingMenuShowed { set; get; } = false;
    public bool isGameplayShowed { set; get; } = false;    // pause button and score text
    public bool isExitMenuShowed { set; get; } = false;
    public bool isAboutMenuShowed { set; get; } = false;
    public bool isNativeAdShowed { set; get; } = false;
    public bool isSupriseGuideShowed { set; get; } = false;

    [Header("Score texts")]
    [SerializeField] Text _scoreTxt;
    [SerializeField] Text _scoreTxtInMenu, _bestScoreTxt;

    [Header("For Change setting buttons background stutos after click on it")]
    [SerializeField] Image _soundBtnImg;
    [SerializeField] Image _musicBtnImg;
    [SerializeField] Image _vibrationBtnImg;
    [SerializeField] Sprite _soundOff, _soundOn, _musicOff, _musicOn, _vibrationOff, _vibrationOn;

    [Header("Suprise guide option")]
    [SerializeField] Image _supriseGuideImage;
    [SerializeField]
    Sprite _chanceSupriseSp, _changeDirctionHailSupriseSp, _changeHailColorSupriseSp,
        _changeHailSpeedSupriseSp, _changePaletteColorSupriseSp, _destroyHailSupriseSp, _slowMotionSupriseSp;


    Animator _animator;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        _animator = GetComponent<Animator>();

    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscapeButtonClick();
        }
    }

    public void SetScoreTxt(int score)
    {
        _scoreTxt.text = score.ToString();
    }

    #region Home menu showing ========================================================
    public void ShowHomeMenuWhenGameLaunching()
    {
        isHomeMenuShowed = true;
        _animator.SetTrigger("Home menu (Show at game  launch)");
        AudioAndVibrationManeger.Instance.play("Background HomeMenu");
    }

    public void ShowHomeMenu()
    {
        isHomeMenuShowed = true;
        _animator.SetTrigger("Home menu (Show)");
        AudioAndVibrationManeger.Instance.play("Background HomeMenu");
    }

    public void HideHomeMenu()
    {
        isHomeMenuShowed = false;
        _animator.SetTrigger("Home menu (Hide)");
        AudioAndVibrationManeger.Instance.stop("Background HomeMenu");
    }
    #endregion

    #region Pause menu showing ==========================================================

    public void ShowPauseMenu()
    {

        _animator.SetTrigger("Pause menu (Show)");
        isPauseMenuShowed = true;
//       if (AdsManeger.instance.isNativeLoaded) // if native ad loaded
//       {
//           if (!isNativeAdShowed)
//               showNativeAd();
//
//       }
    }

    public void HidePauseMenu()
    {

        _animator.SetTrigger("Pause menu (Hide)");
        isPauseMenuShowed = false;
        if (isNativeAdShowed)
            hideNativeAd();

    }

    #endregion

    #region Loss menu showing ===================================================================

    /// <summary>
    /// We can continue playing by watching the ad
    /// </summary>
    public void ShowLossMenuWithCanWatchAds(int score, int bestScore)
    {
        if (!isGameplayShowed)
            return;

        _scoreTxtInMenu.text = score.ToString();
        _bestScoreTxt.text = bestScore.ToString();


        hideGameplay();

        _animator.SetTrigger("Loss Menu with continue btn (Show)");
        isLossMenuShowed = true;

        if (AdsManeger.instance.isNativeLoaded) // if native ad loaded
        {
            if (!isNativeAdShowed)
                showNativeAd();
        }
    }

    public void ShowLossMenuWhenNewRecordReceived(int newRecordScore)
    {
        if (!isGameplayShowed)
            return;

        _bestScoreTxt.text = newRecordScore.ToString();

        hideGameplay();

        _animator.SetTrigger("Loss Menu with New record bg (Show)");
        isLossMenuShowed = true;

        if (AdsManeger.instance.isNativeLoaded) // if native ad loaded
        {
            if (!isNativeAdShowed)
                showNativeAd();

        }
    }

    public void ShowLossMenu(int score, int bestScore)
    {
        if (!isGameplayShowed)
            return;

        _scoreTxtInMenu.text = score.ToString();
        _bestScoreTxt.text = bestScore.ToString();


        hideGameplay();

        _animator.SetTrigger("Loss Menu (Show)");
        isLossMenuShowed = true;

        if (AdsManeger.instance.isNativeLoaded) // if native ad loaded
        {
            if (!isNativeAdShowed)
                showNativeAd();

        }
    }

    public void HideLossMenu()
    {

        _animator.SetTrigger("Loss Menu (Hide)");
        isLossMenuShowed = false;

        if (isNativeAdShowed)
            hideNativeAd();
    }

    // called in animation event
    void playNewRecordSoundInLossMenu()
    {
        AudioAndVibrationManeger.Instance.play("New record menu");
    }

    #endregion

    #region Counter for play sound events
    void playSound_1_Counter()
    {
        AudioAndVibrationManeger.Instance.play("counter 1");
    }

    void playSound_2_Counter()
    {
        AudioAndVibrationManeger.Instance.play("counter 2");
    }

    void playSound_3_Counter()
    {
        AudioAndVibrationManeger.Instance.play("counter 3");
    }

    void playSound_Go_Counter()
    {
        AudioAndVibrationManeger.Instance.play("counter go");
    }

    void counterIsDone()
    {
        AudioAndVibrationManeger.Instance.VibrateWhenGamePlayedForAlert();
    }

    #endregion

    #region Gameplay showing -------------------------------------------------------------------
    public void ShowGameplay()
    {
        if (!isGameplayShowed)
        {
            AudioAndVibrationManeger.Instance.play("Background Gameplay");
            _animator.SetTrigger("Gameplay (Show)");
            isGameplayShowed = true;
        }

    }

    void hideGameplay()
    {
        AudioAndVibrationManeger.Instance.stop("Background Gameplay");
        _animator.SetTrigger("Gameplay (Hide)");
        isGameplayShowed = false;

    }

    // Called in animation event
    void GameplayShowingIsDone()
    {
        GameManager.Instance.Play();
    }

    #endregion

    #region setting menu showing --------------------------------------------------------------


    void showSettingMenu()
    {
            _animator.SetTrigger("Setting menu (Show)");
            isSettingMenuShowed = true;
    }

    void hideSettingMenu()
    {
            _animator.SetTrigger("Setting menu (Hide)");
            isSettingMenuShowed = false;
    }

    public void ChangeSoundBtnStatus(bool mute)
    {
        _soundBtnImg.sprite = mute ? _soundOff : _soundOn;
    }

    public void ChangeVibrationBtnStatus(bool mute)
    {
        _vibrationBtnImg.sprite = mute ? _vibrationOff : _vibrationOn;
    }

    public void ChangeMusicBtnStatus(bool mute)
    {
        _musicBtnImg.sprite = mute ? _musicOff : _musicOn;
    }
    #endregion

    #region Exit menu showing ------------------------------------------------------------------
    private void showExitMenu()
    {
        _animator.SetTrigger("Exit Menu (Show)");
        isExitMenuShowed = true;
    }

    private void hideExitMenu()
    {
        _animator.SetTrigger("Exit Menu (Hide)");
        isExitMenuShowed = false;
    }
    #endregion

    #region About menu showing ---------------------------------------------------------
    private void showAboutMenu()
    {
        _animator.SetTrigger("About menu (Show)");
        isAboutMenuShowed = true;
    }

    private void hideAboutMenu()
    {
        _animator.SetTrigger("About menu (Hide)");
        isAboutMenuShowed = false;
    }

    #endregion

    #region Native ad panel showing --------------------------------------------------------

    private void showNativeAd()
    {
        _animator.SetTrigger("Native ad panel (Show)");
        isNativeAdShowed = true;
    }

    private void hideNativeAd()
    {
        _animator.SetTrigger("Native ad panel (Hide)");
        isNativeAdShowed = false;
    }

    #endregion

    #region Suprise guide menu showing --------------------------------------------------

    public void ShowSupriseGuide(string supriseName)
    {
        if (!isGameplayShowed || isSupriseGuideShowed)
            return;

        HailCreater.Instance.SetCanCreateHail(false);
        HailCreater.Instance.SetCanMoving(false);
        SupriseManeger.Instance.SetCanCreateSuprise(false);
        HailReceiveManeger.Instance.canTouchForControlGamePlay = false;

        hideGameplay();
        AudioAndVibrationManeger.Instance.play("Guide suprise");
        isSupriseGuideShowed = true;

        switch (supriseName)
        {
            case SupriseManeger.chance_suprise:
            {
                _supriseGuideImage.sprite = _chanceSupriseSp;
            }
            break;
            case SupriseManeger.changeDirctionHail_suprise:
            {
                _supriseGuideImage.sprite = _changeDirctionHailSupriseSp;
            }
            break;
            case SupriseManeger.changeHailColor_suprise:
            {
                _supriseGuideImage.sprite = _changeHailColorSupriseSp;
            }
            break;
            case SupriseManeger.changeHailSpeed_suprise:
            {
                _supriseGuideImage.sprite = _changeHailSpeedSupriseSp;
            }
            break;
            case SupriseManeger.changePaletteColor_suprise:
            {
                _supriseGuideImage.sprite = _changePaletteColorSupriseSp;
            }
            break;
            case SupriseManeger.destroyHail_suprise:
            {
                _supriseGuideImage.sprite = _destroyHailSupriseSp;
            }
            break;
            case SupriseManeger.slowMotion_suprise:
            {
                _supriseGuideImage.sprite = _slowMotionSupriseSp;
            }
            break;
            default:
                break;
        }

        _animator.SetTrigger("Suprise guide menu (Show)");
    }

    private void hideSupriseGuide()
    {
        _animator.SetTrigger("Suprise guide menu (Hide)");
        ShowGameplay();
        isSupriseGuideShowed = false;
    }

    #endregion 

    #region Buttons click functions  ---------------------------------------------------------------------

    public void PlayBtnClick()
    {
        AudioAndVibrationManeger.Instance.play("Button click");
        if (isHomeMenuShowed)
        {
            HideHomeMenu();
        }
        GameManager.Instance.Restart();
        ShowGameplay();
    }

    public void PauseBtnClick()
    {
        if (!isGameplayShowed)
            return;

        AudioAndVibrationManeger.Instance.play("Button click");
        GameManager.Instance.Pause();
        hideGameplay();
        ShowPauseMenu();
        Debug.Log("pause btn");
    }

    public void ResumeBtnClick()
    {
        if (isPauseMenuShowed)
        {
            AudioAndVibrationManeger.Instance.play("Button click");
            HidePauseMenu();
            GameManager.Instance.Resume();
            ShowGameplay();
        }

    }


    public void HomeMenuBtnClick()
    {

        if (isPauseMenuShowed)
        {
            HidePauseMenu();
        }
        else if (isLossMenuShowed)
        {
            HideLossMenu();
        }

        if (AdsManeger.instance.isInterstitialAdsLoaded)
        {
            AdsManeger.instance.ShowInterstitalAd();
        }

        ShowHomeMenu();
        AudioAndVibrationManeger.Instance.play("Button click");
    }

    // In loss meno
    public void ContinueBtnClick()
    {
        AudioAndVibrationManeger.Instance.play("Button click");
        if (AdsManeger.instance.isRewardAdLoaded)
        {
            AdsManeger.instance.ShowRewardAd();
        }
        else
        {
            HideLossMenu();
            ShowHomeMenu();
        }
    }

    // In loss meno
    public void RestartBtnClick()
    {
        AudioAndVibrationManeger.Instance.play("Button click");
        if (AdsManeger.instance.isInterstitialAdsLoaded)
        {
            AdsManeger.instance.ShowInterstitalAd();
        }


        GameManager.Instance.Restart();

        HideLossMenu();
        ShowGameplay();
    }
    // Calleds in AdsManeger.cs
    public void ContinueAfterInterstitialAdClosed()
    {

        if (isLossMenuShowed)
        {
            GameManager.Instance.Restart();
            HideLossMenu();
            ShowGameplay();
        }

    }


    public void EscapeButtonClick()
    {
        AudioAndVibrationManeger.Instance.play("Button click");
        if (isGameplayShowed)
            return;

        if (!isExitMenuShowed )
            showExitMenu();
        else
            hideExitMenu();
    }

    // In About dialog menu
    public void OkBtnClick()
    {
        AudioAndVibrationManeger.Instance.play("Button click");
        if (isAboutMenuShowed)
            hideAboutMenu();


        if (isSupriseGuideShowed)
            hideSupriseGuide();
    }

    // In Exit dialog menu
    public void YesBtnClick()
    {
        AudioAndVibrationManeger.Instance.play("Button click");
        if (isExitMenuShowed)
            Application.Quit();
    }

    // In Exit dialog menu
    public void NoBtnClick()
    {
        AudioAndVibrationManeger.Instance.play("Button click");
        if (isExitMenuShowed)
            hideExitMenu();
    }



    public void SettingBtnClick()
    {
        AudioAndVibrationManeger.Instance.play("Button click");
        if (isSettingMenuShowed)
        {
            hideSettingMenu();
        }
        else
        {
            showSettingMenu();
        }
    }

    public void ReviewBtnClick()
    {
        AudioAndVibrationManeger.Instance.play("Button click");
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
    }

    public void InstagramBtnClick()
    {
        AudioAndVibrationManeger.Instance.play("Button click");
        Application.OpenURL(_instagramPageLink);
    }

    public void AboutBtnClick()
    {
        if (!isAboutMenuShowed)
        {
            AudioAndVibrationManeger.Instance.play("Button click");
            showAboutMenu();
        }
    }

    public void SoundBtnClick()
    {
        AudioAndVibrationManeger.Instance.play("Button click");
        bool isMute = AudioAndVibrationManeger.Instance.GetIsSoundMute();
        ChangeSoundBtnStatus(!isMute);
        AudioAndVibrationManeger.Instance.SetSoundMute(!isMute);
    }

    public void MusicBtnClick()
    {
        AudioAndVibrationManeger.Instance.play("Button click");
        bool isMute = AudioAndVibrationManeger.Instance.GetIsMusicMute();
        ChangeMusicBtnStatus(!isMute);
        AudioAndVibrationManeger.Instance.SetMusicMute(!isMute);
        if (isMute)
            AudioAndVibrationManeger.Instance.play("Background HomeMenu");
        else
            AudioAndVibrationManeger.Instance.stop("Background HomeMenu");

    }

    public void VibrationBtnClick()
    {
        AudioAndVibrationManeger.Instance.play("Button click");
        bool isMute = AudioAndVibrationManeger.Instance.GetIsVibrationMute();
        ChangeVibrationBtnStatus(!isMute);
        AudioAndVibrationManeger.Instance.SetVibrationMute(!isMute);
    }
    #endregion
}
