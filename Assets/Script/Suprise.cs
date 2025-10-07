
using UnityEngine;

public class Suprise : MonoBehaviour
{
    private bool istoChangeColor = false;

  
    string ReactionFadeName;
    float _alghaColor = 0f;
    SpriteRenderer sp;

    void Update()
    {
        if (istoChangeColor)
        {
            switch (ReactionFadeName)
            {
                case "FadeToGray":

                    if (sp.color.a <= .9f)
                    {
                        _alghaColor += .05f;
                        sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, _alghaColor);
                        if (sp.color.a > .9f && sp.color.a < 1f)
                        {

                            sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, 1f);
                            Debug.Log("stop");

                            istoChangeColor = false;

                        }
                    }
                    break;
                case "FadeToMainColor":
                    if (sp.color.a > 0f)
                    {

                        Debug.Log("alpha" + sp.color.a);
                        _alghaColor -= .05f;
                        sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, _alghaColor);
                        if (sp.color.a > 0f && sp.color.a < .1f)
                        {


                            Debug.Log("alpha" + sp.color.a);
                            sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, 0f);

                            istoChangeColor = false;


                        }
                    }



                    break;


            }
        }

    }
    public void goTogGrayoRMain(string FadeName)
    {
        ReactionFadeName = FadeName;
        sp = gameObject.transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>();
        istoChangeColor = true;

    }

    void destroyGameObject()
    {
        Destroy(gameObject);
    }
}
