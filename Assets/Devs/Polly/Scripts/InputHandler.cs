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
    private float movementSpeed = 5;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        cubeMat = GetComponent<MeshRenderer>().material;
    }
    private void Update()
    {
        ProcessInput(movementDir);
        cubeMat.color = playerInput.playerIndex switch
        {
            0 => Color.red,
            1 => Color.green,
            2 => Color.blue,
            3 => Color.magenta,
            _ => Color.white
        };
    }

    public void ProcessInputSystem(CallbackContext context)
    {
        /*if (context.ReadValue<Vector2>().y > 0)
            ProcessInput("up");
        if (context.ReadValue<Vector2>().y < 0)
            ProcessInput("down");
        if (context.ReadValue<Vector2>() == new Vector2(0, 0))
            ProcessInput(default);
        if (context.ReadValue<Vector2>().x > 0)
            ProcessInput("right");
        if (context.ReadValue<Vector2>().x < 0)
            ProcessInput("left");*/
        movementDir = context.ReadValue<Vector2>();
    }
    public void ProcessInput(Vector2 input)
    {
        /*cubeMat.color = input switch
        {
            "up" => new Color(255, 0, 0),
            "down" => new Color(0, 255, 0),
            "left" => new Color(0, 50, 255),
            "right" => new Color(0, 0, 255),
            _ => new Color(255, 255, 255),
        };*/
        transform.position += movementSpeed * Time.deltaTime * new Vector3(input.x, 0, input.y);

        Debug.Log(input);
    }
}
