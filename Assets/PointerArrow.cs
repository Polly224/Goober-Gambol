using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerArrow : MonoBehaviour
{
    GameObject cameraObj;

    private void Awake()
    {
        cameraObj = GameObject.Find("Camera");
    }
    void Update()
    {
        transform.localPosition = Vector3.zero;
        transform.position += transform.up * 3;
        transform.LookAt(cameraObj.transform.position);
    }
}
