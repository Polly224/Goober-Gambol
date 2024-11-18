using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColorWithDamage : MonoBehaviour
{
    [SerializeField]
    private float damageVal = 0;
    private TextMeshProUGUI tMP;
    void Start()
    {
        tMP = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        tMP.text = damageVal.ToString();
        tMP.color = Color.HSVToRGB(1, Mathf.Clamp(damageVal, 1, 100) / 100, 1);
    }
}
