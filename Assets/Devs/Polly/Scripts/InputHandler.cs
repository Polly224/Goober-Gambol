using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;
using static UnityEngine.InputSystem.InputAction;

public class InputHandler : MonoBehaviour
{
    Material cubeMat;
    private PlayerInput playerInput;
    private int index = 0;
    private Vector2 movementDir;
    private Vector2 lookDir;
    private float movementSpeed = 5;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        cubeMat = GetComponent<MeshRenderer>().material;
    }
    private void Update()
    {
        SetMoveDirection(movementDir);
        SetLookDirection(lookDir);
        cubeMat.color = playerInput.playerIndex switch
        {
            0 => Color.red,
            1 => Color.green,
            2 => Color.blue,
            3 => Color.magenta,
            _ => Color.white
        };
    }

    public void ProcessMovement(CallbackContext context)
    {
        movementDir = context.ReadValue<Vector2>();
    }
    public void ProcessAiming(CallbackContext context)
    {
        lookDir = context.ReadValue<Vector2>();
    }
    public void SetMoveDirection(Vector2 input)
    { 
        transform.position += movementSpeed * Time.deltaTime * new Vector3(input.x, 0, input.y);
    }

    private void SetLookDirection(Vector2 input)
    {
        transform.LookAt(transform.position + new Vector3(input.x, transform.position.y, input.y));
    }
}
