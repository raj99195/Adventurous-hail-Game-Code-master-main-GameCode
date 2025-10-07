using System.Collections;
using UnityEngine;
public class HailReceiver : MonoBehaviour
{
    public static HailReceiver Instance;

    Vector3 _aNewPositionToBePlacedThere;
    [HideInInspector] public bool _canMoveThisHailReceiverWithLerp { set; get; } = false;



    //set color to gray and main
    Color _colorMainHailReciever;
    Color _colorMain;
    Color _endColor;
    Color colorMain;
    Color checkColor;

    private bool _canThisWhilerunForFadeToColor = false;
    private void Awake()
    {
       
        if (Instance == null){ Instance = this; }
    }
    public void SetMoveWithLerp(Vector3 TargetPosition)
    {
        this._aNewPositionToBePlacedThere = TargetPosition;
        _canMoveThisHailReceiverWithLerp = true;
    }
    private void Update()
    {

        if (HailReceiveManeger.Instance.canTouchForControlGamePlay) {
            if (_canMoveThisHailReceiverWithLerp)
            {
                transform.position = Vector3.Lerp(transform.position, _aNewPositionToBePlacedThere, HailReceiveManeger.Instance.speedMoveHailReceiver * Time.deltaTime);

                if (Vector3.Distance(transform.position, _aNewPositionToBePlacedThere) <= 0.3f)
                {
                    transform.position = _aNewPositionToBePlacedThere;
                }
                if (transform.position == _aNewPositionToBePlacedThere)
                {
                    _canMoveThisHailReceiverWithLerp = false;

                    HailReceiveManeger.Instance.SetCanMoveHailRecivers(true);

                }
            }

        }

    }
    public void startParticleUnderHailReceiver()
    {
        ParticleSystem.MainModule UnderHailReceiver = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>().main;
        UnderHailReceiver.startColor = transform.GetChild(1).GetComponent<SpriteRenderer>().color;
       gameObject.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
    }

    /// <summary>
    /// The main color of this hail receiver is stored
    /// </summary>
    /// <param name="SaveMainHailReceiverColor()"></param>
    public void SaveMainHailReceiverColor()
    {
        _colorMainHailReciever = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
    }
    /// <summary>
    ///  when this function is called, the hail color receiver returns to its original state 
    /// </summary>
    ///  /// <param name="ResetHailReceiverColor()"></param>
    public void ResetHailReceiverColor()
    {
        for (int i = 0; i <= 2; i++)
        {

            gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>().color = _colorMainHailReciever;
        }
    }
    /// <summary>
    ///  when this function is called,use this function in the gameplay, where the Hailreceiver color had to be gray. Items used when displaying menus in gameplay and user loss time.
    /// </summary>
    ///  /// <param name="ChangeHailreceiverColorToGray()"></param>
    public void ChangeHailreceiverColorToGray()
    {
        ColorUtility.TryParseHtmlString("#969696", out _endColor);
        _colorMain = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
        checkColor = _endColor;
        _canThisWhilerunForFadeToColor = true;
        StartCoroutine(changeHailReceiverColorWithFade(_endColor, _colorMain));
    }

    IEnumerator changeHailReceiverColorWithFade(Color endColor, Color startColor)
    {


        while (_canThisWhilerunForFadeToColor)
        {
            yield return new WaitForFixedUpdate();

            if (startColor.r > endColor.r) { startColor.r -= .01f; }
            else if (startColor.r < endColor.r) { startColor.r += .01f; }

            if (startColor.g > endColor.g) { startColor.g -= .01f; }
            else if (startColor.g < endColor.g) { startColor.g += .01f; }

            if (startColor.b > endColor.b) { startColor.b -= .01f; }
            else if (startColor.b < endColor.b) { startColor.b += .01f; }
            for (int i = 0; i <= 2; i++)
            {
                gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>().color = startColor;
            }
            if (gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color.r <= (checkColor.r + .03f) && gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color.r >= (checkColor.r - .03f))
            {

                if (gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color.g <= (checkColor.g + .03f) && gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color.g >= (checkColor.g - .03f))
                { 
                    if (gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color.b <= (checkColor.b + .03f) && gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color.b >= (checkColor.b - .03f))
                    {
                        _canThisWhilerunForFadeToColor = false;

                        for (int b = 0; b <= 2; b++)
                        {
                            gameObject.transform.GetChild(b).GetComponent<SpriteRenderer>().color = endColor;
                        }
                        StopCoroutine("fade_color_to_gray");
                      
                    }
                }
            }
        }
    }
    /// <summary>
    ///  when this function is called,use this function in the gameplay, where the Hailreceiver color had to be main. Items used when displaying menus in gameplay .
    /// </summary>
    ///  /// <param name="ChangeHailColorToMain()"></param>
    public void ChangeHailColorToMain(int numberchild)
    {
        ColorUtility.TryParseHtmlString("#969696", out _endColor);
        colorMain = _colorMainHailReciever;
        checkColor = _colorMainHailReciever;
        _canThisWhilerunForFadeToColor = true;
        StartCoroutine(changeHailReceiverColorWithFade(colorMain, _endColor));
    }


}
