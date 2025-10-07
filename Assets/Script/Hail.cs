using System.Collections;
using UnityEngine;
public class Hail : MonoBehaviour
{
    [SerializeField] Animator _animator;

    //hail move
    public ParticleSystem prefabDestroyParticle;
    [HideInInspector] public float min_speed_hail;
    [HideInInspector] public float max_speed_hail;
    public bool canMoveHail = false;

    public static Hail Instance;

    //position for suprise chane direction
    Vector3 _startPointForChangingDirection;
    Vector3 _endPointForChangingDirection;
    bool _canRunLerpForSupriseDirection = false;


    //speed for all setting hail and suprise
    [HideInInspector] public float speed;
    [HideInInspector] public float Lastspeed;

    //control suprise speed
    [HideInInspector] public float _speedForSupriseSpeed;
    float _lerpSupriseSpeed;

    //color fade to gray  hail
    bool _canRunWhileForFadeToColor = false;
    Color _colorMainHail;
    Color _endColor;
    Color colorMain;
    Color checkcolor;
    string nameFade;
    //score for send to game manager 
    int _score = 0;

    // for then collisioned with 2 hailreceicer in the moment
    bool _isCollisionedWithHailReceivers = false;

    bool _disableSlowMotionMode = false;


    bool[] _surpriseChance = new bool[7];
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D hail_coallition)
    {
        if (_isCollisionedWithHailReceivers)
        {
            return;
        }

        if (hail_coallition.tag.StartsWith("surprise:"))
        {
            AudioAndVibrationManeger.Instance.play("hit with suprise");
            AudioAndVibrationManeger.Instance.VibrateWhenCollisionHailWithSuprise();

        }

        if (hail_coallition.gameObject.CompareTag(SupriseManeger.chance_suprise))
        {
            SupriseManeger.Instance.SetSurpriseChance();
            _surpriseChance[SupriseManeger.Instance.randomSurpriseChance] = true;
        }

        if (hail_coallition.gameObject.CompareTag("Scoroll") && hail_coallition.transform.GetChild(0).GetComponent<SpriteRenderer>().color == gameObject.GetComponent<SpriteRenderer>().color)
        {

            _isCollisionedWithHailReceivers = true;
            _animator.SetTrigger("receive");
            hail_coallition.gameObject.GetComponent<Animator>().SetTrigger("runChangeScale");
            AudioAndVibrationManeger.Instance.play("Received hail");
            AudioAndVibrationManeger.Instance.VibrateReceiveHail();
            GameManager.Instance.NewScoreReceived(_score += 1);
            gameObject.transform.parent = hail_coallition.transform;
            gameObject.transform.position = new Vector3(hail_coallition.transform.position.x, transform.position.y, 0);
            hail_coallition.GetComponent<HailReceiver>().startParticleUnderHailReceiver();

            if (_disableSlowMotionMode)
            {
                Timecontroller.Instance_time_Slow.dontRunSlowMotion = true;
                Timecontroller.Instance_time_Slow.runSlowMotion = false;
                _disableSlowMotionMode = false;
            }
        }

        if (hail_coallition.gameObject.CompareTag("Scoroll") && hail_coallition.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color != gameObject.GetComponent<SpriteRenderer>().color)
        {
           
            _isCollisionedWithHailReceivers = true;
            AudioAndVibrationManeger.Instance.play("Loss");
            gameObject.tag = "hail loss";
            hail_coallition.gameObject.tag = "hailReceiver loss";
            GameManager.Instance.Loss();
          
            hail_coallition.gameObject.GetComponent<Animator>().SetTrigger("runAniamtionCrashShredder");
        }

        if (hail_coallition.gameObject.CompareTag(SupriseManeger.changeHailColor_suprise) || _surpriseChance[0])
        {
            _surpriseChance[0] = false;
            SupriseManeger.Instance.SendColorForActionOfSurprisingColorChange();
            if (gameObject.GetComponent<SpriteRenderer>().color != SupriseManeger.colorObtainedFromInsideThePaletteForColorSuprise)
            {
                gameObject.GetComponent<SpriteRenderer>().color = SupriseManeger.colorObtainedFromInsideThePaletteForColorSuprise;
            }
            else
            {
                SupriseManeger.Instance.SendColorForActionOfSurprisingColorChange();
                gameObject.GetComponent<SpriteRenderer>().color = SupriseManeger.colorObtainedFromInsideThePaletteForColorSuprise;
            }
            runWaveAnimation(hail_coallition.gameObject);
            SetParticleHail(SupriseManeger.colorObtainedFromInsideThePaletteForColorSuprise);
        }
        if (hail_coallition.gameObject.CompareTag(SupriseManeger.changeDirctionHail_suprise) || _surpriseChance[4])
        {
            _surpriseChance[4] = false;
            runWaveAnimation(hail_coallition.gameObject);
            switch (hail_coallition.gameObject.transform.position.x)
            {
                case -1.75f:
                    startChangingDirection(new Vector3(transform.position.x - speed, 0, 0), new Vector3(0, 0, 0));
                    break;
                case 1.75f:
                    startChangingDirection(new Vector3(transform.position.x + speed, 0, 0), new Vector3(0, 0, 0));
                    break;
                case 0:
                    if (hail_coallition.gameObject.GetComponent<SpriteRenderer>().sprite == SupriseManeger.Instance.SpriteDirection[0])
                    {
                        startChangingDirection(new Vector3(transform.position.x + speed, 0, 0), new Vector3(-1.75f, 0, 0));
                    }
                    else if (hail_coallition.gameObject.GetComponent<SpriteRenderer>().sprite == SupriseManeger.Instance.SpriteDirection[1])
                    {
                        startChangingDirection(new Vector3(transform.position.x - speed, 0, 0), new Vector3(1.75f, 0, 0));
                    }
                    break;
            }
        }
        if (hail_coallition.gameObject.CompareTag(SupriseManeger.changeHailSpeed_suprise) || _surpriseChance[2])
        {
            Lastspeed = speed;
            _surpriseChance[2] = false;
            runWaveAnimation(hail_coallition.gameObject);
            if (HailCreater.Instance._check)
            {
                Debug.Log("return");
                return;
            }
            if (HailCreater.Instance._canSupriseSpeedStop == false)
            {
                HailCreater.Instance._check = true;

                _lerpSupriseSpeed = LevelDesigner.Instance.steps[LevelDesigner.Instance.stepNumber].SpeedHail;
                _speedForSupriseSpeed = speed * 2f;
                HailCreater.Instance.suprisespeedallchild(true, Lastspeed, _speedForSupriseSpeed);
                HailCreater.Instance._canSupriseSpeedRun = true;
                HailCreater.Instance.StartCoroutine(HailCreater.Instance.timerspeedsuprise());
                AudioAndVibrationManeger.Instance.SetToActiveSpeedSuprise("Background Gameplay");
            }
        }
        if (hail_coallition.gameObject.CompareTag(SupriseManeger.changePaletteColor_suprise) || _surpriseChance[1])
        {
            runWaveAnimation(hail_coallition.gameObject);
            Timecontroller.Instance_time_Slow.dontRunSlowMotion = true;

            HailCreater.Instance.changePaletteColor(true);
            HailCreater.Instance.destroyAllHailCloneInGameplay = true;
            _surpriseChance[1] = false;

        }
        if (hail_coallition.gameObject.CompareTag(SupriseManeger.destroyHail_suprise) || _surpriseChance[5])
        {

            _surpriseChance[5] = false;
            runWaveAnimation(hail_coallition.gameObject);
            ShowParticleVFXWhenDestroyHail();
            Destroy(gameObject);




        }
        if (hail_coallition.gameObject.CompareTag(SupriseManeger.slowMotion_suprise) || _surpriseChance[3])
        {
            _surpriseChance[3] = false;
            runWaveAnimation(hail_coallition.gameObject);
            Timecontroller.Instance_time_Slow.runSlowMotion = true;

            AudioAndVibrationManeger.Instance.SetToActiveSlowMotion("Background Gameplay");
            StartCoroutine(endTimerSuprise(true));
            _disableSlowMotionMode = true;
        }
        if (hail_coallition.gameObject.CompareTag("up recive color hail"))
        {

            hail_coallition.gameObject.transform.GetChild(0).GetComponent<Animator>().SetBool("is_run_anim", true);
            hail_coallition.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = gameObject.GetComponent<SpriteRenderer>().color;
        }
    }
    private void OnTriggerExit2D(Collider2D collisionExit)
    {
        if (collisionExit.gameObject.CompareTag("up recive color hail"))
        {
            collisionExit.gameObject.transform.GetChild(0).GetComponent<Animator>().SetBool("is_run_anim", false);
            ColorUtility.TryParseHtmlString("#969696", out Color up_recive_color_hail);
            collisionExit.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = up_recive_color_hail;
        }

        if (collisionExit.gameObject.CompareTag("Scoroll") || collisionExit.gameObject.CompareTag("hailReceiver_Wandering"))
        {
            Destroy(gameObject);
        }

    }

    void startChangingDirection(Vector3 startPoint, Vector3 endPoint)
    {
        this._startPointForChangingDirection = startPoint;
        this._endPointForChangingDirection = endPoint;
        _canRunLerpForSupriseDirection = true;

    }
    private void FixedUpdate()
    {
        if (_canRunLerpForSupriseDirection)
        {
            transform.position -= new Vector3(_startPointForChangingDirection.x * Time.deltaTime, 0, 0);
            if (Mathf.Abs(transform.position.x - _endPointForChangingDirection.x) < ((speed + .005f) * Time.deltaTime))

            {


                transform.position = new Vector3(_endPointForChangingDirection.x, transform.position.y - .17f, 0);
                _canRunLerpForSupriseDirection = false;
            }
        }
    }
    void Update()
    {

        if (canMoveHail) { transform.position -= new Vector3(0, speed * Time.deltaTime, 0); }
        if (HailCreater.Instance._canSupriseSpeedRun)
        {
            if (speed <= _speedForSupriseSpeed)
            {
                speed += LevelDesigner.Instance.steps[LevelDesigner.Instance.stepNumber].SpeedHail * Time.deltaTime;
            }
        }
        if (HailCreater.Instance._canSupriseSpeedStop)
        {
            speed -= LevelDesigner.Instance.steps[LevelDesigner.Instance.stepNumber].SpeedHail * Time.deltaTime;
            HailCreater.Instance.suprisespeedallchild(false, speed, _speedForSupriseSpeed);
            if (speed <= LevelDesigner.Instance.steps[LevelDesigner.Instance.stepNumber].SpeedHail)
            {

                speed = LevelDesigner.Instance.steps[LevelDesigner.Instance.stepNumber].SpeedHail;
                HailCreater.Instance.suprisespeedallchild(false, speed, _speedForSupriseSpeed);
                HailCreater.Instance._canSupriseSpeedStop = false;
                HailCreater.Instance._check = false;
                Debug.Log("now is ready to cool" + "....." + "mainspeed =" + speed);
                AudioAndVibrationManeger.Instance.ResetPlayingSpeed("Background Gameplay");

            }
        }
    }
    IEnumerator endTimerSuprise(bool slowmotionsuprise)
    {
        if (slowmotionsuprise)
        {
            yield return new WaitForSecondsRealtime(LevelDesigner.Instance.steps[LevelDesigner.Instance.stepNumber].LifeTimeOfSlowMotionSuprise);
            Timecontroller.Instance_time_Slow.dontRunSlowMotion = true;
            Timecontroller.Instance_time_Slow.runSlowMotion = false;
            AudioAndVibrationManeger.Instance.ResetPlayingSpeed("Background Gameplay");
            StopCoroutine("endTimerSuprise");
        }

    }


    void runWaveAnimation(GameObject collitionInSuprise)
    {
        collitionInSuprise.transform.GetChild(3).GetComponent<SpriteRenderer>().color = gameObject.GetComponent<SpriteRenderer>().color;
        collitionInSuprise.GetComponent<Animator>().SetTrigger("runWaveAndEndSuprise");
    }

    public void SetParticleHail(Color particle_color)
    {
        ParticleSystem.MainModule Setting = gameObject.GetComponent<ParticleSystem>().main;

        Setting.startColor = particle_color;
    }
    public void ShowParticleVFXWhenDestroyHail()
    {
        ParticleSystem.MainModule destroy = prefabDestroyParticle.main;
        destroy.startColor = gameObject.GetComponent<SpriteRenderer>().color;
        ParticleSystem o = Instantiate(prefabDestroyParticle, transform.position, Quaternion.identity);
        Destroy(o.gameObject, 1.5f);
    }
    public void SaveMainHailColor()
    {
        _colorMainHail = gameObject.GetComponent<SpriteRenderer>().color;

    }
    public void ResetHailColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = _colorMainHail;
    }
    public void ChangeHailColorToGray()
    {
        ColorUtility.TryParseHtmlString("#969696", out _endColor);
        colorMain = gameObject.GetComponent<SpriteRenderer>().color;
        checkcolor = _endColor;
        nameFade = "graycolor";
        _canRunWhileForFadeToColor = true;
        StartCoroutine(changeColorWithFade(_endColor, colorMain));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endColor">Gray color  </param>
    /// <param name="startColor"> hail color </param>
    /// <returns></returns>
    IEnumerator changeColorWithFade(Color endColor, Color startColor)
    {

        while (_canRunWhileForFadeToColor)
        {
            yield return new WaitForFixedUpdate();
            if (startColor.r > endColor.r) { startColor.r -= .01f; }
            else if (startColor.r < endColor.r) { startColor.r += .01f; }

            if (startColor.g > endColor.g) { startColor.g -= .01f; }
            else if (startColor.g < endColor.g) { startColor.g += .01f; }

            if (startColor.b > endColor.b) { startColor.b -= .01f; }
            else if (startColor.b < endColor.b) { startColor.b += .01f; }

            gameObject.GetComponent<SpriteRenderer>().color = startColor;

            if (gameObject.GetComponent<SpriteRenderer>().color.r <= (checkcolor.r + .03f) && gameObject.GetComponent<SpriteRenderer>().color.r >= (checkcolor.r - .03f))
            {

                if (gameObject.GetComponent<SpriteRenderer>().color.g <= (checkcolor.g + .03f) && gameObject.GetComponent<SpriteRenderer>().color.g >= (checkcolor.g - .03f))
                {

                    if (gameObject.GetComponent<SpriteRenderer>().color.b <= (checkcolor.b + .03f) && gameObject.GetComponent<SpriteRenderer>().color.b >= (checkcolor.b - .03f))
                    {
                        _canRunWhileForFadeToColor = false;
                        gameObject.GetComponent<SpriteRenderer>().color = endColor;
                        if (nameFade == "graycolor")
                        {
                            gameObject.GetComponent<ParticleSystem>().Stop();
                        }
                        else
                        {
                            gameObject.GetComponent<ParticleSystem>().Play();
                        }
                        StopCoroutine("fade_color_to_gray");

                    }
                }
            }
        }
    }
    public void ChangeHailColorToMain()
    {
        ColorUtility.TryParseHtmlString("#969696", out _endColor);
        colorMain = _colorMainHail;
        checkcolor = _colorMainHail;
        nameFade = "maincolor";
        _canRunWhileForFadeToColor = true;

        StartCoroutine(changeColorWithFade(colorMain, _endColor));
    }

}
