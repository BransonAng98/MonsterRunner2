using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseScript : MonoBehaviour
{
    public GameObject IndicatorVFX;
    public GameObject ReachedVFX;
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
       
    }

    public void TurnOnVFX()
    {
        IndicatorVFX.SetActive(true);
    }

    public void TurnOffVFX()
    {
        IndicatorVFX.SetActive(false);
    }

    public void CreateReachedVFX()
    {
        Instantiate(ReachedVFX, transform.position, Quaternion.identity);
    }
}
