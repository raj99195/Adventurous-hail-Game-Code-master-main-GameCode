using System.Collections.Generic;
using UnityEngine;

public class HailReceiveManeger : MonoBehaviour
{
    public static HailReceiveManeger Instance;

    //hails receiver
    public GameObject[] childHailReceiver = new GameObject[6];
    [HideInInspector] public Vector3[] _firstPositionOfHailReceivers = new Vector3[6];
    [HideInInspector] public List<string> storageMemoryCommandHailsReceiver = new List<string>();
    int _numberOfStorageList = 0;

    bool _isMovingDown = true;
    //color
    Color _colorObtainedFromInsideThePaletteForHailsReceiver;
    [HideInInspector] public bool canChangeColorHailsReceiver = true;
    [HideInInspector] public bool firstChangeColorHailsReceiver = true;

    GameObject _recieveColorHailsReceiberWandering;

    [HideInInspector] public bool canTouchForControlGamePlay = false;
    [SerializeField] GameObject _symbolicBtn_R, _symbolicBtn_L;

    bool isChildsColorIsGray = false;
    public float speedMoveHailReceiver;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        for (int a = 1; a <= 5; a += 1)
        {
            childHailReceiver[5] = gameObject.transform.GetChild(5).gameObject;
            childHailReceiver[a - 1] = gameObject.transform.GetChild(childHailReceiver.Length - (a + 1)).gameObject;
        }
        for (int b = 0; b <= 5; b++)
        {
            _firstPositionOfHailReceivers[b] = childHailReceiver[b].transform.position;

        }
    }

    private void Update()
    {
        setInitialColorForHailReceiver();

        checkInputsForGameplayControl();

        if (_isMovingDown)
        {

            if (storageMemoryCommandHailsReceiver.Count > _numberOfStorageList)
            {
                _isMovingDown = false;

                if (storageMemoryCommandHailsReceiver[_numberOfStorageList] == "right")
                {
                    for (int a = 0; a <= 5; a++)
                    {
                        if (childHailReceiver[a].CompareTag("hailReceiver_Wandering"))
                        {
                            childHailReceiver[a].transform.position = new Vector3(-5.25f, transform.position.y, 0);
                            break;

                        }

                    }

                    foreach (GameObject number_array_of_childe_hailReceiver in childHailReceiver)
                    {
                        if (number_array_of_childe_hailReceiver.transform.position.x >= 3.5f)
                        {
                            _recieveColorHailsReceiberWandering = GameObject.FindGameObjectWithTag("hailReceiver_Wandering");
                            for (int i = 0; i <= 2; i++)
                            {
                                _recieveColorHailsReceiberWandering.transform.GetChild(i).GetComponent<SpriteRenderer>().color = number_array_of_childe_hailReceiver.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
                            }
                            _recieveColorHailsReceiberWandering.transform.tag = number_array_of_childe_hailReceiver.transform.tag;

                            HailReceiver move_Lerp = number_array_of_childe_hailReceiver.GetComponent<HailReceiver>();
                            move_Lerp.SetMoveWithLerp(new Vector3(5.25f, transform.position.y, 0));
                            number_array_of_childe_hailReceiver.transform.tag = "hailReceiver_Wandering";
                        }
                        else
                        {
                            HailReceiver move_Lerp = number_array_of_childe_hailReceiver.GetComponent<HailReceiver>();
                            move_Lerp.SetMoveWithLerp(new Vector3(number_array_of_childe_hailReceiver.transform.position.x + 1.75f, transform.position.y, 0));

                        }
                    }
                }


                if (storageMemoryCommandHailsReceiver[_numberOfStorageList] == "left")
                {
                    for (int a = 0; a <= 5; a++)
                    {
                        if (childHailReceiver[a].CompareTag("hailReceiver_Wandering"))
                        {
                            childHailReceiver[a].transform.position = new Vector3(5.25f, transform.position.y, 0);
                            break;
                        }
                    }
                    foreach (GameObject number_array_of_childe_hailReceiver in childHailReceiver)
                    {
                        if (number_array_of_childe_hailReceiver.transform.position.x <= -3.5f)
                        {
                            _recieveColorHailsReceiberWandering = GameObject.FindGameObjectWithTag("hailReceiver_Wandering");
                            for (int i = 0; i <= 2; i++)
                            {
                                _recieveColorHailsReceiberWandering.transform.GetChild(i).GetComponent<SpriteRenderer>().color = number_array_of_childe_hailReceiver.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
                            }
                            _recieveColorHailsReceiberWandering.transform.tag = number_array_of_childe_hailReceiver.transform.tag;

                            HailReceiver move_Lerp = number_array_of_childe_hailReceiver.GetComponent<HailReceiver>();
                            move_Lerp.SetMoveWithLerp(new Vector3(-5.25f, transform.position.y, 0));
                            number_array_of_childe_hailReceiver.transform.tag = "hailReceiver_Wandering";
                        }
                        else
                        {
                            HailReceiver move_Lerp = number_array_of_childe_hailReceiver.GetComponent<HailReceiver>();
                            move_Lerp.SetMoveWithLerp(new Vector3(number_array_of_childe_hailReceiver.transform.position.x - 1.75f, transform.position.y, 0));
                        }
                    }
                }
                _numberOfStorageList++;
            }
        }
    }
    public void SetCanMoveHailRecivers(bool isMovingDown)
    {
        _isMovingDown = isMovingDown;
    }

    void setInitialColorForHailReceiver()
    {
        if (canChangeColorHailsReceiver)
        {
            if (firstChangeColorHailsReceiver)
            {
                HailCreater.Instance.changePaletteColor(true);
                canChangeColorHailsReceiver = false;
            }

            canChangeColorHailsReceiver = false;

            for (int i = 0; i <= 4; i++)
            {

                if (childHailReceiver[i].CompareTag("Scoroll"))
                {
                    ColorUtility.TryParseHtmlString(HailCreater.A_set_of_palettes_and_color_codes_inside_each_palette[HailCreater.Instance.randomForSelectPalette][i], out _colorObtainedFromInsideThePaletteForHailsReceiver);
                    for (int b = 0; b <= 2; b++)
                    {
                        childHailReceiver[i].transform.GetChild(b).GetComponent<SpriteRenderer>().color = _colorObtainedFromInsideThePaletteForHailsReceiver;
                    }

                }
                else
                {
                    ColorUtility.TryParseHtmlString(HailCreater.A_set_of_palettes_and_color_codes_inside_each_palette[HailCreater.Instance.randomForSelectPalette][i], out _colorObtainedFromInsideThePaletteForHailsReceiver);
                    for (int q = 0; q <= 2; q++)
                    {
                        childHailReceiver[5].transform.GetChild(q).GetComponent<SpriteRenderer>().color = _colorObtainedFromInsideThePaletteForHailsReceiver;
                    }

                }
            }

        }
    }
    void checkInputsForGameplayControl()
    {

        if (canTouchForControlGamePlay)
        {
            if (_isMovingDown)
            {
                if (Input.GetKeyDown(KeyCode.D) || Input.GetMouseButtonDown(0) && Camera.main.ScreenToWorldPoint(Input.mousePosition).x > 0 && Camera.main.ScreenToWorldPoint(Input.mousePosition).y < 7f)
                {
                    AudioAndVibrationManeger.Instance.play("Controller Btn R");
                    storageMemoryCommandHailsReceiver.Add("right");
                    _symbolicBtn_R.GetComponent<Animator>().SetTrigger("On click");
                }

                if (Input.GetKeyDown(KeyCode.A) || Input.GetMouseButtonDown(0) && Camera.main.ScreenToWorldPoint(Input.mousePosition).x < 0 && Camera.main.ScreenToWorldPoint(Input.mousePosition).y < 7f)
                {
                    AudioAndVibrationManeger.Instance.play("Controller Btn L");
                    storageMemoryCommandHailsReceiver.Add("left");
                    _symbolicBtn_L.GetComponent<Animator>().SetTrigger("On click");
                }
            }
        }
    }
    /// <summary>
    /// It calls all the functions of its children to go gray
    /// </summary>
    public void GoAllChildToGray()
    {
        if (isChildsColorIsGray)
            return;

        for (int i = 0; i <= 5; i++)
        {
            if (gameObject.transform.GetChild(i).CompareTag("Scoroll") || gameObject.transform.GetChild(i).CompareTag("hailReceiver_Wandering"))
            {
                gameObject.transform.GetChild(i).GetComponent<HailReceiver>().SaveMainHailReceiverColor();
                gameObject.transform.GetChild(i).GetComponent<HailReceiver>().ChangeHailreceiverColorToGray();
            }
        }

        isChildsColorIsGray = true;
    }

    /// <summary>
    /// All these actions of the children call them to the gray. except collision to wrong hailReceiver  
    /// </summary>
    public void GoAllChildToGrayWithOutTheCollidingObject()
    {
        if (isChildsColorIsGray)
            return;

        for (int i = 0; i <= 5; i++)
        {
            if (gameObject.transform.GetChild(i).CompareTag("Scoroll") || gameObject.transform.GetChild(i).CompareTag("hailReceiver_Wandering"))
            {
                gameObject.transform.GetChild(i).GetComponent<HailReceiver>().SaveMainHailReceiverColor();
                gameObject.transform.GetChild(i).GetComponent<HailReceiver>().ChangeHailreceiverColorToGray();
            }


        }

        isChildsColorIsGray = true;

    }

    /// <summary>
    /// It calls all the functions of its children to go maincolor
    /// </summary>
    public void GoAllChildToMainColor(bool canWithLerp)
    {
        if (!isChildsColorIsGray)
            return;

        if (canWithLerp)
        {
            for (int i = 0; i <= 5; i++)
            {
                if (gameObject.transform.GetChild(i).CompareTag("Scoroll") || gameObject.transform.GetChild(i).CompareTag("hailReceiver_Wandering"))
                {

                    gameObject.transform.GetChild(i).GetComponent<HailReceiver>().ChangeHailColorToMain(i);
                }
            }
        }
        else
        {
            for (int i = 0; i <= 5; i++)
            {
                if (gameObject.transform.GetChild(i).CompareTag("Scoroll") || gameObject.transform.GetChild(i).CompareTag("hailReceiver_Wandering"))
                {
                    gameObject.transform.GetChild(i).GetComponent<HailReceiver>().ResetHailReceiverColor();
                }
                else
                {
                    gameObject.transform.GetChild(i).tag = "Scoroll";
                    gameObject.transform.GetChild(i).GetComponent<Animator>().SetTrigger("is_run_anim_hailReceiver");
                }

            }

        }
        isChildsColorIsGray = false;

    }



    /// <summary>
    /// All these actions of the children call them to the main color. except collision to wrong hailReceiver  
    /// </summary>
    public void GoAllChildToMainColorWithOutTheCollidingObject()
    {
        if (!isChildsColorIsGray)
            return;

        for (int i = 0; i <= 5; i++)
        {
            if (gameObject.transform.GetChild(i).CompareTag("Scoroll") || gameObject.transform.GetChild(i).CompareTag("hailReceiver_Wandering"))
            {

                gameObject.transform.GetChild(i).GetComponent<HailReceiver>().ChangeHailColorToMain(i);
            }
            else
            {
                gameObject.transform.GetChild(i).tag = "Scoroll";
                gameObject.transform.GetChild(i).GetComponent<Animator>().SetTrigger("is_run_anim_hailReceiver");
            }
        }

        isChildsColorIsGray = false;
    }

    public void resetPositionHailReciever()
    {
        for (int c = 0; c <= 5; c++)
        {

            for (int g = 0; g <= 5; g++)
            {
                if (childHailReceiver[g].CompareTag("hailReceiver_Wandering"))
                {
                    GameObject n = childHailReceiver[g];
                    childHailReceiver[g] = childHailReceiver[5];
                    childHailReceiver[5] = n;
                    break;
                }
            }

            for (int l = 0; l <= 5; l++)
            {
                childHailReceiver[l].transform.position = _firstPositionOfHailReceivers[l];
            }
        }
    }

}
