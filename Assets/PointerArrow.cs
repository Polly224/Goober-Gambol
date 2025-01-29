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
        if(transform.root.gameObject.CompareTag("Player")) transform.localScale = new(1, 1f / 3f, 1);
        transform.LookAt(cameraObj.transform.position);
    }
}
