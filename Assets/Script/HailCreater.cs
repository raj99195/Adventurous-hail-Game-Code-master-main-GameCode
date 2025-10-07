using System.Collections;
using UnityEngine;
public class HailCreater : MonoBehaviour
{
    //create hail
    public GameObject hailPrefab;
    float _delayCreateHail;
    private GameObject _gameobjectsMadeOfPrefab;
    public Transform[] positionsCreaterHail = new Transform[3];
    static public float min_delay_create_hail;
    public bool canCreateHail = false;


    //for number instance hail in gameplay
    public GameObject hail_container;
    [HideInInspector] public bool destroyAllHailCloneInGameplay = false;
    [HideInInspector] public bool canSetTheSpeedOfTheHailsThatAreStillInTheGameplayWithTheSpeedOfTheNewStep = false;


    //color hail
    private RandomUtils _randomUtilPalleteColor;
    Color _colorObtainedFromInsideThePalette;
    [HideInInspector] public bool canChangeThePalette;
    public int randomForSelectPalette;

    public static HailCreater Instance;

    //colors palette
    static public string[][] A_set_of_palettes_and_color_codes_inside_each_palette = new string[][] { new string[] { "#,", "#,", "#,", "#,", "#," },
                                                                                                      new string[] { "#,", "#,", "#,", "#,", "#,"},
                                                                                                      new string[] { "#,", "#,", "#,", "#,", "#," },
                                                                                                      new string[] { "#,", "#,", "#,", "#,", "#," },
                                                                                                      new string[] { "#,", "#,", "#,", "#,", "#,"},
                                                                                                      new string[] { "#,", "#,", "#,", "#,", "#," }};
    public Color[] palette1 = new Color[5];
    public Color[] palette2 = new Color[5];
    public Color[] palette3 = new Color[5];
    public Color[] palette4 = new Color[5];
    public Color[] palette5 = new Color[5];
    public Color[] palette6 = new Color[5];
    int numbercolor = 1;


    [HideInInspector] public bool _canSupriseSpeedRun = false;
    [HideInInspector] public bool _canSupriseSpeedStop = false;
    [HideInInspector] public bool _check = false;
    private void Awake()
    {
        prepareColorsData();

        for (int i = 0; i <= 5; i++)
        {
            for (int b = 0; b <= 4; b++)
            {
                switch (numbercolor)
                {
                    case 1:
                        A_set_of_palettes_and_color_codes_inside_each_palette[i][b] = ("#" + ColorUtility.ToHtmlStringRGB(palette1[b]));
                        if (b == 4) { numbercolor = 2; }
                        break;
                    case 2:
                        A_set_of_palettes_and_color_codes_inside_each_palette[i][b] = ("#" + ColorUtility.ToHtmlStringRGB(palette2[b]));
                        if (b == 4) { numbercolor = 3; }
                        break;
                    case 3:
                        A_set_of_palettes_and_color_codes_inside_each_palette[i][b] = ("#" + ColorUtility.ToHtmlStringRGB(palette3[b]));
                        if (b == 4) { numbercolor = 4; }
                        break;
                    case 4:
                        A_set_of_palettes_and_color_codes_inside_each_palette[i][b] = ("#" + ColorUtility.ToHtmlStringRGB(palette4[b]));
                        if (b == 4) { numbercolor = 5; }
                        break;
                    case 5:
                        A_set_of_palettes_and_color_codes_inside_each_palette[i][b] = ("#" + ColorUtility.ToHtmlStringRGB(palette5[b]));
                        if (b == 4) { numbercolor = 6; }
                        break;
                    case 6:
                        A_set_of_palettes_and_color_codes_inside_each_palette[i][b] = ("#" + ColorUtility.ToHtmlStringRGB(palette6[b]));
                        if (b == 4) { numbercolor = 1; }
                        break;

                }
            }

        }

        if (HailCreater.Instance == null)
        {
            HailCreater.Instance = this;
        }
        _randomUtilPalleteColor = new RandomUtils();
        SetCanCreateHail(false);
    }

    void prepareColorsData()
    {
        if (palette1.Length != 5)
        {
            emergencyExit("The number of colors you entered in variable palette1 is less than or greater than 5. Please change the size of the array to 5., refer to this section and modify the size >>>>>( Hail Creater GameObject >> Inspector window >> Array palette1 >> size ");

        }
        if (palette2.Length != 5)
        {
            emergencyExit("The number of colors you entered in variable palette2 is less than or greater than 5. Please change the size of the array to 5., refer to this section and modify the size >>>>>( Hail Creater GameObject >> Inspector window >> Array palette2 >> size ");

        }
        if (palette3.Length != 5)
        {
            emergencyExit("The number of colors you entered in variable palette3 is less than or greater than 5. Please change the size of the array to 5., refer to this section and modify the size >>>>>( Hail Creater GameObject >> Inspector window >> Array palette3 >> size ");
        }
        if (palette4.Length != 5)
        {
            emergencyExit("The number of colors you entered in variable palette4 is less than or greater than 5. Please change the size of the array to 5., refer to this section and modify the size >>>>>( Hail Creater GameObject >> Inspector window >> Array palette4 >> size ");
        }
        if (palette5.Length != 5)
        {
            emergencyExit("The number of colors you entered in variable palette5 is less than or greater than 5. Please change the size of the array to 5., refer to this section and modify the size >>>>>( Hail Creater GameObject >> Inspector window >> Array palette5 >> size ");
        }
        if (palette6.Length != 5)
        {
            emergencyExit("The number of colors you entered in variable palette6 is less than or greater than 5. Please change the size of the array to 5., refer to this section and modify the size >>>>>( Hail Creater GameObject >> Inspector window >> Array palette6 >> size ");
        }

    }

    void emergencyExit(string message)
    {
        Debug.LogError(message);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }

    void Update()
    {
        if (destroyAllHailCloneInGameplay)
        {
            destroyAllHail();
            destroyAllHailCloneInGameplay = false;
        }

        if (canCreateHail)
        {

            createHail();
        }

    }
    /// <summary>
    ///Operation of making Hail material
    /// </summary>
    /// <param name="createHail()"></param>
    void createHail()
    {


        if (_delayCreateHail >= 0)
            _delayCreateHail -= 1 * Time.deltaTime;
        if (_delayCreateHail <= 0)
        {
            int pos_random = UnityEngine.Random.Range(0, 3);
            _gameobjectsMadeOfPrefab = Instantiate(hailPrefab, positionsCreaterHail[pos_random].position, Quaternion.identity);
            _gameobjectsMadeOfPrefab.transform.parent = hail_container.transform;

            AudioAndVibrationManeger.Instance.play("Create hail");


            setColorHailsOfColorsInPalette();
            setMove();
            min_delay_create_hail = LevelDesigner.Instance.steps[LevelDesigner.Instance.stepNumber].delayOfCreateHail;
            _delayCreateHail = Random.Range(min_delay_create_hail, min_delay_create_hail + .2f);
        }
    }
    void destroyAllHail()
    {

        foreach (Transform child_hail_clone in hail_container.transform)
        {
            Destroy(child_hail_clone.gameObject);
            child_hail_clone.gameObject.GetComponent<Hail>().ShowParticleVFXWhenDestroyHail();
        }

    }

    private void setMove()
    {

        Hail hail = _gameobjectsMadeOfPrefab.GetComponent<Hail>();
        hail.canMoveHail = true;
        hail.speed = 0;

        hail.min_speed_hail = LevelDesigner.Instance.steps[LevelDesigner.Instance.stepNumber].SpeedHail;
        //hail.speed = UnityEngine.Random.Range(hail.min_speed_hail, hail.min_speed_hail + .2f);

        hail.speed = hail.min_speed_hail;
        if (canSetTheSpeedOfTheHailsThatAreStillInTheGameplayWithTheSpeedOfTheNewStep)
        {
            changeSpeedOfTheHailsInTheGameplayWhileChangingTheStep();
            canSetTheSpeedOfTheHailsThatAreStillInTheGameplayWithTheSpeedOfTheNewStep = false;
        }
    }

    /// <summary>
    ///This function synchronizes the ball with the speed of the new stage when changing the stage of the game.
    /// </summary>
    /// <param name="changeSpeedOfTheHailsInTheGameplayWhileChangingTheStep()"></param>
    void changeSpeedOfTheHailsInTheGameplayWhileChangingTheStep()
    {
        Hail hail = _gameobjectsMadeOfPrefab.GetComponent<Hail>();

        foreach (Transform number_hails in hail_container.transform)
        {
            number_hails.gameObject.GetComponent<Hail>().speed = hail.speed;
        }
    }


    public void changePaletteColor(bool canChangeThePalette)
    {
        if (canChangeThePalette)
        {
            randomForSelectPalette = _randomUtilPalleteColor.GenerateRandom(0, 5);
            HailReceiveManeger.Instance.canChangeColorHailsReceiver = true;
        }

    }


    void setColorHailsOfColorsInPalette()
    {
        int random_for_select_color_in_palette = UnityEngine.Random.Range(0, 5);
        ColorUtility.TryParseHtmlString(A_set_of_palettes_and_color_codes_inside_each_palette[randomForSelectPalette][random_for_select_color_in_palette], out _colorObtainedFromInsideThePalette);
        _gameobjectsMadeOfPrefab.GetComponent<SpriteRenderer>().color = _colorObtainedFromInsideThePalette;
        Hail hail = _gameobjectsMadeOfPrefab.GetComponent<Hail>();
        hail.SetParticleHail(_colorObtainedFromInsideThePalette);
    }

    public void HailCreaterRestart()
    {
        destroyAllHail();
    }

    public void GoAllChildToGray()
    {
        foreach (Transform number_child_hail in hail_container.transform)
        {

            Hail hail = number_child_hail.GetComponent<Hail>();
            if (number_child_hail.gameObject.CompareTag("prefab"))
            {
                hail.SaveMainHailColor();
                hail.ChangeHailColorToGray();
            }

            else
            {

                hail.gameObject.GetComponent<ParticleSystem>().Stop();
            }
        }
    }
    public void GoAllChildToMainColor()
    {
        foreach (Transform number_child_hail in hail_container.transform)
        {

            Hail hail = number_child_hail.GetComponent<Hail>();

            if (number_child_hail.gameObject.CompareTag("prefab"))
            {
                hail.ChangeHailColorToMain();
            }
        }
    }
    public void GoAllChildToGrayWithOutTheCollidingObject()
    {
        GoAllChildToGray();
    }

    public void SetCanCreateHail(bool canCreate)
    {
        canCreateHail = canCreate;
    }
    public void SetCanMoving(bool canMove)
    {


        foreach (Transform number_child_hail in hail_container.transform)
        {
            Hail hail = number_child_hail.GetComponent<Hail>();
            hail.canMoveHail = canMove;
        }

    }
    public void DestroyHaillosser()
    {

        foreach (Transform number_child_hail in hail_container.transform)
        {
            if (number_child_hail.gameObject.CompareTag("hail loss"))
            {
                Destroy(number_child_hail.gameObject);
            }
        }
    }
    public void suprisespeedallchild(bool isrun, float mainspeed, float highSpeed)
    {
        foreach (Transform number_child_hail in hail_container.transform)
        {
            if (!isrun)
            {
                number_child_hail.gameObject.GetComponent<Hail>().speed = mainspeed;
            }
            number_child_hail.gameObject.GetComponent<Hail>()._speedForSupriseSpeed = highSpeed;
            _canSupriseSpeedRun = isrun;

        }


    }
    public IEnumerator timerspeedsuprise()
    {

        yield return new WaitForSecondsRealtime(LevelDesigner.Instance.steps[LevelDesigner.Instance.stepNumber].LifeTimeOfSpeedSuprise);
        HailCreater.Instance._canSupriseSpeedRun = false;
        HailCreater.Instance._canSupriseSpeedStop = true;

        Debug.Log("end crotin");
        StopCoroutine("endTimerSuprise");

    }
}
