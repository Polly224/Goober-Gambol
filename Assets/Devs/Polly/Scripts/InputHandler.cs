using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEditor.Timeline.TimelinePlaybackControls;
using static UnityEngine.InputSystem.InputAction;

public class InputHandler : MonoBehaviour
{
    Material cubeMat;
    private PlayerInput playerInput;
    private Vector2 movementDir;
    private Vector2 lookDir;
    private float movementSpeed = 5;
    private float jumpRollSpeed = 50;
    public bool isLooking = false;
    public bool isMoving = false;
    public bool isRagdolling = false;
    public bool canStopRagdolling = true;
    private Rigidbody rb;
    [SerializeField]
    private float turnSpeed = 1;
    private void Awake()
    {
        // Gets the player input and the player's material for later usage.
        playerInput = GetComponent<PlayerInput>();
        cubeMat = GetComponent<MeshRenderer>().material;
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // When a player is added, their controller gets added to the controller list.
        PlayerDataStorage.instance.AddToControllers(playerInput.GetDevice<InputDevice>());
        PlayerDataStorage.connectedPlayerObjects.Add(gameObject);
    }

    private void OnDestroy() => PlayerDataStorage.connectedPlayerObjects.Remove(gameObject);
    private void FixedUpdate()
    {
        // Moves in the held direction, looks in the held direction.
        lookDir = (isMoving && !isLooking) ? movementDir : lookDir;
        if(!isRagdolling) SetMoveDirection(movementDir);
        if(!isRagdolling) SetLookDirection(lookDir);
        // Sets cube's color dependant on player slot, makes it easier to distinguish players. For testing.
        cubeMat.color = playerInput.playerIndex switch
        {
            0 => Color.red,
            1 => Color.green,
            2 => Color.blue,
            3 => Color.magenta,
            _ => Color.white
        };
        if ((rb.rotation.x != 0 || rb.rotation.z != 0) && !isRagdolling)
        {
            rb.AddTorque(new Vector3(-rb.rotation.x * 3, 0, -rb.rotation.z * 3));
        }
        if (isRagdolling && IsGrounded() && canStopRagdolling) isRagdolling = false;
        if(!isRagdolling && !isMoving) rb.velocity /= (1 / (2 * Time.deltaTime));
    }
    public void ProcessMovement(CallbackContext context)
    {
        movementDir = context.ReadValue<Vector2>();
        isMoving = !context.canceled;
    }
    public void ProcessAiming(CallbackContext context)
    {
        if(context.performed) lookDir = context.ReadValue<Vector2>();
        isLooking = !context.canceled;
    }
    public void SetMoveDirection(Vector2 input)
    { 
        rb.transform.position += movementSpeed * Time.deltaTime * new Vector3(input.x, 0, input.y);
    }

    private void SetLookDirection(Vector2 input)
    {
        // transform.LookAt(transform.position + new Vector3(input.x, transform.position.y, input.y));
        Quaternion newRotation = Quaternion.LookRotation(new Vector3(input.x, rb.position.y, input.y));
        transform.rotation = Quaternion.Slerp(rb.rotation, newRotation, turnSpeed * Time.deltaTime);
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(rb.position, -Vector3.up, 0.6f) && rb.velocity.y <= 0;
    }

    public void JumpRoll(CallbackContext context)
    {
        if(context.performed) 
        {
            if(!isRagdolling && IsGrounded())
            {
                rb.AddRelativeForce(new Vector3(0, jumpRollSpeed / 5, jumpRollSpeed), ForceMode.Impulse);
                rb.AddRelativeTorque(new Vector3(UnityEngine.Random.Range(10f, 20), 0, UnityEngine.Random.Range(10f, 20)), ForceMode.Impulse);
                transform.LookAt(rb.position + (transform.up / 3 + transform.forward) * jumpRollSpeed);
                Ragdoll();
                StartCoroutine(DelayedRagdoll());
                GetComponent<AttackScript>().StopAllCoroutines();
                GetComponent<AttackScript>().attackOnCooldown = false;
            }
        }
    }

    public void Ragdoll(CallbackContext context)
    {
        if(!isRagdolling && context.performed) isRagdolling = true;
    }
    public void Ragdoll()
    {
        if (!isRagdolling) isRagdolling = true;
    }

    public IEnumerator DelayedRagdoll()
    {
        yield return new WaitForSeconds(0.1f);
        Ragdoll();
        yield break;
    }
}
