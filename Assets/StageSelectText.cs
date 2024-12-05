using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectText : MonoBehaviour
{
    float val = 0;
    public float rotationSpeed = 1;
    public float rotationModifier = 1;

    private void Update()
    {
        val += Time.deltaTime * rotationSpeed;
        transform.rotation = Quaternion.Euler(50, 0, Mathf.Sin(val * rotationModifier));
    }
}
