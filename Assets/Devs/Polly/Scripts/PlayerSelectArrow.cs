using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR.Haptics;
using static UnityEngine.InputSystem.InputAction;

public class PlayerSelectArrow : MonoBehaviour
{
    PlayerInput pI;
    public int currentSelectionIndex = 0;

    private void Start()
    {
        pI = GetComponent<PlayerInput>();
    }

    public void SwitchPosition(CallbackContext context)
    {

    }

    private void Update()
    {
        transform.position = new Vector3(-4.5f + currentSelectionIndex * 1.5f, 1.7f, 0);
    }
}
