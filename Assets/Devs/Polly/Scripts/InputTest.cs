using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
    Material cubeMat;
    [Header("Inputs")]
    [SerializeField] private InputAction testInput;
    InputHandler iP;

    private void Awake()
    {
        cubeMat = GetComponent<MeshRenderer>().material;
        iP = GetComponent<InputHandler>();
    }

    private void OnEnable()
    {
        testInput.Enable();
    }
    private void OnDisable()
    {
        testInput.Disable();
    }
    void Start()
    {
        cubeMat.color = Color.red;
    }
}
