using System.Collections;
using UnityEngine;

public class PlaceOfCreateHails : MonoBehaviour
{
    public static PlaceOfCreateHails Instance;

    //set color creater hail to gray
    Color _colorMainCreaterHail;
    Color _endColor;
    Color _colorMain;
    bool _canRunWhileForFadeToColor = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void SaveMainColor()
    {
        _colorMainCreaterHail = gameObject.GetComponent<SpriteRenderer>().color;
    }
    public void ResetColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = _colorMainCreaterHail;
    }
    public void ChangeColorToGray()
    {
        ColorUtility.TryParseHtmlString("#969696", out _endColor);
        _colorMain = gameObject.GetComponent<SpriteRenderer>().color;
        _canRunWhileForFadeToColor = true;
        StartCoroutine(changeColorWithFade(_endColor, _colorMain));

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endColor"> gray color</param>
    /// <param name="startColor"> this object color </param>
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

            if (gameObject.GetComponent<SpriteRenderer>().color.r <= .60f && gameObject.GetComponent<SpriteRenderer>().color.r >= .57f)
            {
                if (gameObject.GetComponent<SpriteRenderer>().color.g <= .60f && gameObject.GetComponent<SpriteRenderer>().color.g >= .57f)
                {
                    if (gameObject.GetComponent<SpriteRenderer>().color.b <= .60f && gameObject.GetComponent<SpriteRenderer>().color.b >= .57f)
                    {
                        _canRunWhileForFadeToColor = false;                      
                        gameObject.GetComponent<SpriteRenderer>().color = endColor;
                        StopCoroutine("fade_color_to_gray");

                    }
                }
            }
        }
    }
}


