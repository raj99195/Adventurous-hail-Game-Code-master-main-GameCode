using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupriseManeger : MonoBehaviour
{

    public static SupriseManeger Instance;

    public Sprite[] SpriteDirection = new Sprite[2];
    public Sprite[] SpriteDirectionBlackAndWite = new Sprite[2];
    public float Position_X_ToMakeSurprises;

    //check active suprise for step
    public List<int> ActiveSurprisesInThisSteppe;
    int Step = 0;

    //color for suprisr color
    static public Color colorObtainedFromInsideThePaletteForColorSuprise;
    private RandomUtils _randomUtilitForColorInPalette;
    [HideInInspector] public bool resetAlphaColor = false;

    //create suprise
    public Animator cameraAnimationAfterMakingSurprises;
    public List<GameObject> Surprises;
    [HideInInspector] public bool canCreateSurprise = true;
    public bool[] CanRunSurpriseStep;
    private float _delayCreateSurprise;
    private int _randomSurprise;
    private RandomUtils _randomUtilitForSurprises;

    //random SurpriseChance
    private RandomUtils _randomUtilitForSurpriseChance;
    [HideInInspector] public int randomSurpriseChance;

    //checked collition with ArtificialIntelligence
    [HideInInspector] public int collisionDetectionToArtificialIntelligence = 0;

    public GameObject SupriseContainer;
    GameObject _gameobjectsMadeOfPrefab;

    public const string changeHailColor_suprise = "surprise:color hail";
    public const string changePaletteColor_suprise = "surprise:change pakage color";
    public const string changeHailSpeed_suprise = "surprise: speed hail";
    public const string slowMotion_suprise = "surprise: slow motion";
    public const string changeDirctionHail_suprise = "surprise: change direction";
    public const string chance_suprise = "surprise : chance";
    public const string destroyHail_suprise = "surprise: destroy";

    private bool _isChangeHailColor_supriseGuideShowed = false;
    private bool _isChangePaletteColor_supriseGuideShowed = false;
    private bool _isChangeHailSpeed_supriseGuideShowed = false;
    private bool _isChangeDirctionHail_supriseGuideShowed = false;
    private bool _isSlowMotion_supriseGuideShowed = false;
    private bool _isChance_supriseGuideShowed = false;
    private bool _isDestroyHail_supriseGuideShowed = false;
    PlayerPrefsManeger playerPrefs;


    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        _randomUtilitForColorInPalette = new RandomUtils();
        _randomUtilitForSurpriseChance = new RandomUtils();
        _randomUtilitForSurprises = new RandomUtils();
        playerPrefs = new PlayerPrefsManeger();
        loadPrefs();
    }

    private void loadPrefs()
    {
        if (playerPrefs.LoadBoolean(chance_suprise))
            _isChance_supriseGuideShowed = true;

        if (playerPrefs.LoadBoolean(changeDirctionHail_suprise))
            _isChangeDirctionHail_supriseGuideShowed = true;

        if (playerPrefs.LoadBoolean(changeHailColor_suprise))
            _isChangeHailColor_supriseGuideShowed = true;

        if (playerPrefs.LoadBoolean(changeHailSpeed_suprise))
            _isChangeHailSpeed_supriseGuideShowed = true;

        if (playerPrefs.LoadBoolean(changePaletteColor_suprise))
            _isChangePaletteColor_supriseGuideShowed = true;

        if (playerPrefs.LoadBoolean(destroyHail_suprise))
            _isDestroyHail_supriseGuideShowed = true;

        if (playerPrefs.LoadBoolean(slowMotion_suprise))
            _isSlowMotion_supriseGuideShowed = true;
    }

    void Start()
    {
        _delayCreateSurprise = LevelDesigner.Instance.steps[LevelDesigner.Instance.stepNumber].delayOfCreateSurprises;
    }

    private void Update()
    {
        if (canCreateSurprise)
        {
            if (_delayCreateSurprise > 0)
                _delayCreateSurprise -= 1 * Time.deltaTime;
            if (_delayCreateSurprise <= 0)
            {
                for (int i = 0; i <= LevelDesigner.Instance.steps.Length - 1; i++)
                {
                    if (CanRunSurpriseStep[i] == true)
                    {
                        checkActiveSurprisesThisStep(i);
                        break;
                    }
                }

                _delayCreateSurprise = LevelDesigner.Instance.steps[LevelDesigner.Instance.stepNumber].delayOfCreateSurprises;

            }
        }

    }

    /// <summary>
    ///Responsible for storing active surprises.
    /// </summary>
    /// <param name="saveActiveSurprisesInThisStep(int gameStepNumber)"></param>
    void saveActiveSurprisesInThisStep(int gameStepNumber)
    {
        for (int i = 0; i <= Surprises.Count - 1; i++)
        {
            if (LevelDesigner.Instance.steps[gameStepNumber].activeSupriseList[i])
            {
                ActiveSurprisesInThisSteppe.Add(i);
            }
        }
    }

    /// <summary>
    ///In this step, it examines the active surprises in LevelDesigner, and stores them to randomly place these surprises in the GamePlay.
    /// </summary>
    /// <param name="checkActiveSurprisesThisStep(int numberStep)"></param>
    void checkActiveSurprisesThisStep(int numberStep)
    {

        if (Step == numberStep)
        {
            if (numberStep > 0)
            {
                ActiveSurprisesInThisSteppe.Clear();
            }
            saveActiveSurprisesInThisStep(numberStep);
            Step = (numberStep + 1);
        }

        _randomSurprise = _randomUtilitForSurprises.GenerateRandom(0, ActiveSurprisesInThisSteppe.Count - 1);

        for (int n = 0; n <= Surprises.Count - 1; n++)
        {

            if (LevelDesigner.Instance.steps[numberStep].activeSupriseList[n] && ActiveSurprisesInThisSteppe[_randomSurprise] == n)
            {

                int maximum_value_for_surprise_position_step_i = LevelDesigner.Instance.steps[numberStep].theMaximumDistanceToCreateSurpriseFromTop;
                int minimum_value_for_surprise_position_step_i = LevelDesigner.Instance.steps[numberStep].theMinimumDistanceToCreateSurpriseFromTop;
                adjustTheLocationSurprise(minimum_value_for_surprise_position_step_i, maximum_value_for_surprise_position_step_i);
                break;
            }
        }

    }


    /// <summary>
    ///Finds the right place to place a surprise
    /// </summary>
    /// <param name="minadjustTheLocationSurprise(int minimumPosition, int maximumPosition)imumPosition"></param>
    void adjustTheLocationSurprise(int minimumPosition, int maximumPosition)
    {
        Position_X_ToMakeSurprises = Random.Range(-1, 1 + 1);
        setSpriteForSupriseOfChangingDirection();
        CheckerOfRightSpaceForSurprises.Instance.MoveSurpriseManager(minimumPosition, maximumPosition);
        StartCoroutine(instantiateSurprise());
    }

    public void SendColorForActionOfSurprisingColorChange()
    {
        int change_color_of_surprise_color = _randomUtilitForColorInPalette.GenerateRandom(0, 4);
        ColorUtility.TryParseHtmlString(HailCreater.A_set_of_palettes_and_color_codes_inside_each_palette[HailCreater.Instance.randomForSelectPalette][change_color_of_surprise_color], out colorObtainedFromInsideThePaletteForColorSuprise);
    }

    void setSpriteForSupriseOfChangingDirection()
    {
        if (ActiveSurprisesInThisSteppe[_randomSurprise] == 4)
        {

            switch (Position_X_ToMakeSurprises)
            {
                case -1:
                    Surprises[4].GetComponent<SpriteRenderer>().sprite = SpriteDirection[1];
                    Surprises[4].transform.GetChild(4).GetComponent<SpriteRenderer>().sprite = SpriteDirectionBlackAndWite[1];
                    break;

                case 0:
                    int right_or_left = Random.Range(0, 1 + 1);
                    Surprises[4].GetComponent<SpriteRenderer>().sprite = SpriteDirection[right_or_left];
                    if (right_or_left == 1)
                    {
                        Surprises[4].transform.GetChild(4).GetComponent<SpriteRenderer>().sprite = SpriteDirectionBlackAndWite[1];
                    }
                    else
                    {
                        Surprises[4].transform.GetChild(4).GetComponent<SpriteRenderer>().sprite = SpriteDirectionBlackAndWite[0];
                    }
                    break;
                case 1:

                    Surprises[4].GetComponent<SpriteRenderer>().sprite = SpriteDirection[0];
                    Surprises[4].transform.GetChild(4).GetComponent<SpriteRenderer>().sprite = SpriteDirectionBlackAndWite[0];
                    break;
            }
        }
    }

    public void SetSurpriseChance()
    {
        randomSurpriseChance = _randomUtilitForSurpriseChance.GenerateRandom(0, Surprises.Count - 2);
    }
    IEnumerator instantiateSurprise()
    {

        yield return new WaitForSeconds(.01f);
        if (collisionDetectionToArtificialIntelligence == 0)
              
        {
            _gameobjectsMadeOfPrefab = Instantiate(Surprises[ActiveSurprisesInThisSteppe[_randomSurprise]], transform.position, Quaternion.identity);
            CheckIsGuideShowedForItAndShow(_gameobjectsMadeOfPrefab.tag);

            AudioAndVibrationManeger.Instance.play("Comming suprise object");
            cameraAnimationAfterMakingSurprises.SetTrigger("run_move_cam");
            _gameobjectsMadeOfPrefab.transform.parent = SupriseContainer.transform;
        }
    }

    void CheckIsGuideShowedForItAndShow(string supriseName)
    {
        bool showed = false;
        switch (supriseName)
        {
            case chance_suprise:
            {
                showed = _isChance_supriseGuideShowed;
                if (!_isChance_supriseGuideShowed)
                {
                    _isChance_supriseGuideShowed = true;
                }
            }
            break;
            case changeDirctionHail_suprise:
            {
                showed = _isChangeDirctionHail_supriseGuideShowed;
                if (!_isChangeDirctionHail_supriseGuideShowed)
                {
                    _isChangeDirctionHail_supriseGuideShowed = true;
                }
            }
            break;
            case changeHailColor_suprise:
            {
                showed = _isChangeHailColor_supriseGuideShowed;
                if (!_isChangeHailColor_supriseGuideShowed)
                {
                    _isChangeHailColor_supriseGuideShowed = true;
                }
            }
            break;
            case changeHailSpeed_suprise:
            {
                showed = _isChangeHailSpeed_supriseGuideShowed;
                if (!_isChangeHailSpeed_supriseGuideShowed)
                {
                    _isChangeHailSpeed_supriseGuideShowed = true;
                }
            }
            break;
            case changePaletteColor_suprise:
            {
                showed = _isChangePaletteColor_supriseGuideShowed;
                if (!_isChangePaletteColor_supriseGuideShowed)
                {
                    _isChangePaletteColor_supriseGuideShowed = true;
                }
            }
            break;
            case destroyHail_suprise:
            {
                showed = _isDestroyHail_supriseGuideShowed;
                if (!_isDestroyHail_supriseGuideShowed)
                {
                    _isDestroyHail_supriseGuideShowed = true;
                }
            }
            break;
            case slowMotion_suprise:
            {
                showed = _isSlowMotion_supriseGuideShowed;
                if (!_isSlowMotion_supriseGuideShowed)
                {
                    _isSlowMotion_supriseGuideShowed = true;
                }
            }
            break;
            default:
                break;
        }

        if (!showed)
        {
            UIManeger.Instance.ShowSupriseGuide(supriseName);
            playerPrefs.SaveBoolean(supriseName, true);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("prefab") ||
            collision.gameObject.CompareTag(changeHailColor_suprise) ||
            collision.gameObject.CompareTag(changePaletteColor_suprise) ||
            collision.gameObject.CompareTag(changeHailSpeed_suprise) ||
            collision.gameObject.CompareTag(slowMotion_suprise) ||
            collision.gameObject.CompareTag(changeDirctionHail_suprise) ||
            collision.gameObject.CompareTag(chance_suprise) ||
            collision.gameObject.CompareTag(destroyHail_suprise))
        {
            collisionDetectionToArtificialIntelligence++;
        }

    }
    void OnTriggerExit2D(Collider2D collision_exit)
    {

        if (collision_exit.gameObject.CompareTag("prefab") ||
            collision_exit.gameObject.CompareTag(changeHailColor_suprise) ||
            collision_exit.gameObject.CompareTag(changePaletteColor_suprise) ||
            collision_exit.gameObject.CompareTag(changeHailSpeed_suprise) ||
            collision_exit.gameObject.CompareTag(slowMotion_suprise) ||
            collision_exit.gameObject.CompareTag(changeDirctionHail_suprise) ||
            collision_exit.gameObject.CompareTag(chance_suprise) ||
            collision_exit.gameObject.CompareTag(destroyHail_suprise))
        {

            collisionDetectionToArtificialIntelligence--;
        }

    }

    public void destroyChildeSuprise()
    {
        foreach (Transform child_suprise_clone in SupriseContainer.transform)
        {
            Destroy(child_suprise_clone.gameObject);
        }
    }

    public void SetCanCreateSuprise(bool cancreate)
    {
        canCreateSurprise = cancreate;
    }

    public void GoAllChildToGray()
    {
   
        foreach (Transform child_suprise_clone in SupriseContainer.transform)
        {
            Debug.Log(child_suprise_clone.tag);
         
            child_suprise_clone.transform.GetComponent<Suprise>().goTogGrayoRMain("FadeToGray");
        }
    }
    public void GoAllChildToMainColor()
    {
        foreach (Transform child_suprise_clone in SupriseContainer.transform)
        {
            Debug.Log(child_suprise_clone.tag);
            child_suprise_clone.transform.GetComponent<Suprise>().goTogGrayoRMain("FadeToMainColor");
        }
    }
    public void SupriseRestartForLevelDesigner()
    {
        Step = 0;
        canCreateSurprise = false;
        destroyChildeSuprise();

    }

}