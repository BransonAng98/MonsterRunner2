using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFader : MonoBehaviour
{
    public float fadeSpeed;
    public float fadeAmount;
    float originalOpacity;
    Material mat;
    public bool DoFade = false;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        originalOpacity = mat.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        if(DoFade)
        {
            FadeNow();
        }
        else
        {
            ResetFade();
        }
    }

    void FadeNow()
    {
        Color currentcolour = mat.color;
        Color smoothColour = new Color(currentcolour.r, currentcolour.g, currentcolour.b, Mathf.Lerp(currentcolour.a, fadeAmount, fadeSpeed * Time.deltaTime)); ;
        mat.color = smoothColour;
    }

    void ResetFade()
    {
        Color currentcolour = mat.color;
        Color smoothColour = new Color(currentcolour.r, currentcolour.g, currentcolour.b, Mathf.Lerp(currentcolour.a, originalOpacity, fadeSpeed * Time.deltaTime));
        mat.color = smoothColour;
    }
}
