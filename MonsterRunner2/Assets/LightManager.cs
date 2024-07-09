using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public Light[] lightsLeft;
    public Light[] lightsRight;
    public float flashInterval = 0.5f;

    private void Start()
    {
        StartCoroutine(FlashLights());
    }

    private IEnumerator FlashLights()
    {
        while (true)
        {
            // Turn on left lights and turn off right lights
            SetLights(lightsLeft, true);
            SetLights(lightsRight, false);
            yield return new WaitForSeconds(flashInterval);

            // Turn off left lights and turn on right lights
            SetLights(lightsLeft, false);
            SetLights(lightsRight, true);
            yield return new WaitForSeconds(flashInterval);
        }
    }

    private void SetLights(Light[] lights, bool state)
    {
        foreach (Light light in lights)
        {
            light.enabled = state;
        }
    }
}
