using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponScript : MonoBehaviour
{
    public WeaponSO weaponData; // Reference to the ScriptableObject
    public LineRenderer lineRenderer;
    public Transform firePoint;

    public Slider slider;
    public Image sliderImage;
    public Image sliderFillImage;

    private float nextFireTime;
    private int shotsInBurst;

    [SerializeField] float fireRate;
    [SerializeField] float maxOffsetDistance;
    [SerializeField] float weaponRange;
    [SerializeField] float reloadTime;
    [SerializeField] int magzineSize;

    private void Awake()
    {
        fireRate = weaponData.fireRate;
        maxOffsetDistance = weaponData.maxOffsetDistance;
        weaponRange = weaponData.weaponRange;
        reloadTime = weaponData.reloadTime;
        magzineSize = weaponData.magzineSize;
        slider.maxValue = magzineSize;
        slider.value = 0f;
    }

    void Update()
    {
        // Detect enemies within firing range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, weaponData.weaponRange);
        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Enemy"))
            {
                // Shoot enemy
                Shoot(col.transform);
            }
        }
    }

    void Shoot(Transform target)
    {
        if (Time.time >= nextFireTime && shotsInBurst < weaponData.magzineSize)
        {
            // Calculate a random offset for the target position
            Vector3 randomOffset = Random.insideUnitSphere * weaponData.maxOffsetDistance;

            // Apply the offset to the target position
            Vector3 adjustedTargetPosition = target.position + randomOffset;

            // Raycast to check if we hit the enemy
            RaycastHit hit;
            if (Physics.Raycast(firePoint.position, adjustedTargetPosition - firePoint.position, out hit, weaponData.weaponRange))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    // Enemy hit, do damage or other actions here
                    Debug.Log("Enemy hit!");
                    hit.collider.gameObject.GetComponent<EnemyScript>().TakeDamage(10);
                }
                else
                {
                   
                    Debug.Log("Missed!");
                }
            }
      

            // Set line renderer positions
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hit.point); // Set the line renderer endpoint to the point of impact

            // Start line renderer
            lineRenderer.enabled = true;

            // Increment the number of shots in the current burst
            shotsInBurst++;

            // Check if the burst is complete
            if (shotsInBurst >= weaponData.magzineSize)
            {
                // Set next fire time after the burst delay
                nextFireTime = Time.time + weaponData.reloadTime;

                // Reset burst count
                shotsInBurst = 0;
                slider.value = 0;
                //Refills the bar during reload
                StartCoroutine(FillSliderCoroutine());
            }
            else
            {
                // Set next fire time within the burst
                nextFireTime = Time.time + 1f / weaponData.fireRate;
            }

            // Disable line renderer after a short duration
            StartCoroutine(DisableLineRendererAfterDelay(0.1f));
        }
    }

    IEnumerator FadeImage(Image image1, Image image2, float startAlpha, float targetAlpha, float duration)
    {
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            Color newColor = image1.color;
            newColor.a = Mathf.Lerp(startAlpha, targetAlpha, (Time.time - startTime) / duration);
            image1.color = newColor;
            image2.color = newColor;
            yield return null;
        }
        Color finalColor = image1.color;
        finalColor.a = targetAlpha;
        image1.color = finalColor;
        image2.color = finalColor;
    }

    IEnumerator FillSliderCoroutine()
    {
        StartCoroutine(FadeImage(sliderImage, sliderFillImage, sliderImage.color.a, 1f, 0.5f));
        float timer = 0f;
        float currentValue;
        while (timer < reloadTime - 0.5f)
        {
            // Calculate the progress based on the current time and the fill duration
            float progress = timer / reloadTime - 0.5f;

            // Use Mathf.Lerp to smoothly interpolate between the current value and the max value
            currentValue = Mathf.Lerp(slider.value, slider.maxValue, progress);

            // Update the slider value
            slider.value = currentValue;

            // Increment the timer
            timer += Time.deltaTime;

            yield return null;
        }

        // Ensure the slider reaches its maximum value exactly
        slider.value = magzineSize;
        StartCoroutine(FadeImage(sliderImage, sliderFillImage, sliderImage.color.a, 0f, 0.2f));
    }

    IEnumerator DisableLineRendererAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        lineRenderer.enabled = false;
    }
}

