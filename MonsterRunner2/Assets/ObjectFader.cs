using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFader : MonoBehaviour
{
    public float fadeSpeed;
    public float fadeAmount;
    [SerializeField ]private float currentalpha;
    float originalOpacity;
    //Material mat;
    //Renderer ren;
    public bool DoFade = false;
    // Start is called before the first frame update

    public GameObject solidMesh;
    public GameObject transparentMesh;

    public Material opaqueMaterial;
    public Material transparentMaterial;

    void Start()
    {
        Material mat = transparentMesh.GetComponentInChildren<Renderer>().material;
        //ren = GetComponent<Renderer>();
        //mat = GetComponent<Renderer>().material;
        originalOpacity = mat.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        Material mat = transparentMesh.GetComponentInChildren<Renderer>().material;
        currentalpha = mat.color.a;
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
     
        //ren.material = transparentMaterial;
        //mat = transparentMaterial;
        //Color currentcolour = mat.color;
        //Color smoothColour = new Color(currentcolour.r, currentcolour.g, currentcolour.b, Mathf.Lerp(currentcolour.a, fadeAmount, fadeSpeed * Time.deltaTime)); ;
        //mat.color = smoothColour;
        solidMesh.SetActive(false);
        transparentMesh.SetActive(true);
        Material mat = transparentMesh.GetComponent<Renderer>().material;
        Color currentcolour = mat.color;
        Color smoothColour = new Color(currentcolour.r, currentcolour.g, currentcolour.b, Mathf.Lerp(currentcolour.a, fadeAmount, fadeSpeed * Time.deltaTime)); ;
        mat.color = smoothColour;


    }

    void ResetFade()
    {
        //ren.material = opaqueMaterial;
        //mat = opaqueMaterial;
        //Color currentcolour = mat.color;
        //Color smoothColour = new Color(currentcolour.r, currentcolour.g, currentcolour.b, Mathf.Lerp(currentcolour.a, originalOpacity, fadeSpeed * Time.deltaTime));
        //mat.color = smoothColour;
        Material mat = transparentMesh.GetComponent<Renderer>().material;
        Color currentcolour = mat.color;
        Color smoothColour = new Color(currentcolour.r, currentcolour.g, currentcolour.b, Mathf.Lerp(currentcolour.a, originalOpacity, fadeSpeed * Time.deltaTime)); ;
        mat.color = smoothColour;
        transparentMesh.SetActive(false);
        solidMesh.SetActive(true);

    }
}
