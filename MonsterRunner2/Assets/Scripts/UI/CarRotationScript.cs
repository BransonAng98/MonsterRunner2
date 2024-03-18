using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRotationScript : MonoBehaviour
{
    public float rotationSpeed;

    void Update()
    {
        Rotate();
    }

    void Rotate()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
