using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class TitleScreenMovement : MonoBehaviour
{
    PlayerInput playerInput;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void JoystickMoved(CallbackContext context)
    {
        MoveIndicator(context.ReadValue<Vector2>().y);
    }

    public void MoveIndicator(float yVal)
    {
        if(yVal > 0.8f)
        {

        }
    }
}
