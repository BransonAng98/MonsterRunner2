using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseScript : MonoBehaviour
{
    public GameObject IndicatorVFX;
    // Start is called before the first frame update
    public missionManagerScript missionManager;
    void Start()
    {
        missionManager = GameObject.Find("MissionManager").GetComponent<missionManagerScript>();
        IndicatorVFX.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject == missionManager.destination)
        {
            IndicatorVFX.SetActive(true);
        }
    }
}
