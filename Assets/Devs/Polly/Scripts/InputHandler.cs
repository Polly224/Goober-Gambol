using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public Vector2 movementDir;
    public Vector2 lookDir;
    private float acceleration = 7;
    private float movementSpeed = 0;
    private float maxMovementSpeed = 7;
    private float jumpRollSpeed = 40;
    public bool isLooking = false;
    public bool isMoving = false;
    public bool isRagdolling = false;
    public bool canStopRagdolling = true;
    private Animator modelAnim;
    private Rigidbody rb;
    [SerializeField]
    private float turnSpeed = 1;
    private GameObject playModel;
    public GameObject ragdollModel;
    private GameObject spawnedRagdoll;
    private List<Transform> ragdollPositions;
    private void Awake()
    {
        // Gets the player input and the player's material for later usage.
        playerInput = GetComponent<PlayerInput>();
        cubeMat = GetComponent<MeshRenderer>().material;
        rb = GetComponent<Rigidbody>();
        modelAnim = GetComponentInChildren<Animator>();
        playModel = transform.GetChild(0).gameObject;
        playModel.SetActive(true);
    }

    private void Start()
    {
        playerInput.ActivateInput();
        // When a player is added, their controller gets added to the controller list.
        PlayerDataStorage.instance.AddToControllers(playerInput.GetDevice<InputDevice>());
        PlayerDataStorage.connectedPlayerObjects.Add(gameObject);
    }

    private void OnEnable()
    {
        playerInput.ActivateInput();
    }
    private void OnDisable()
    {
        playerInput.DeactivateInput();
    }

    private void OnDestroy() => PlayerDataStorage.connectedPlayerObjects.Remove(gameObject);
    private void FixedUpdate()
    {
        // Moves in the held direction, looks in the held direction.
        if(isMoving && !isRagdolling && movementSpeed <= maxMovementSpeed)
            movementSpeed += acceleration * Time.deltaTime;
        if(movementSpeed > maxMovementSpeed) movementSpeed = maxMovementSpeed;
        if (!isMoving) movementSpeed = 0;

        lookDir = (isMoving && !isLooking) ? movementDir : lookDir;
        if(!isRagdolling && isMoving) SetMoveDirection(movementDir);
        if(!isRagdolling && (isLooking || isMoving)) SetLookDirection(lookDir);

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
        if (isRagdolling && IsGrounded() && !GetComponent<DamageSystem>().isDizzy) StopRagdolling();
        if(!isRagdolling && !isMoving) rb.velocity /= 2;
        // if (!isRagdolling) rb.AddTorque(-rb.GetAccumulatedTorque() * 10);

        // Animation functions.
        modelAnim.SetBool("IsMoving", isMoving);
        modelAnim.SetFloat("MovementSpeed", movementSpeed / maxMovementSpeed * (Mathf.Abs(movementDir.x) + Mathf.Abs(movementDir.y)));
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
        Quaternion newRotation = Quaternion.LookRotation(new Vector3(input.x, 0, input.y));
        transform.rotation = Quaternion.Slerp(rb.rotation, newRotation, turnSpeed * Time.deltaTime);
    }

    public bool IsGrounded()
    {
        // Draws a line from the center of the player's model, 2 units in length, straight down. If it hits a surface, the player's considered grounded.
        return Physics.Raycast(rb.position, -Vector3.up, 1.7f) && rb.velocity.y <= 0;
    }

    public void JumpRoll(CallbackContext context)
    {
        if(context.performed) 
        {
            if(!isRagdolling && IsGrounded())
            {
                // If the player's not already ragdolling and also grounded, the player jumps forward with a large burst of speed and ragdolls.
                transform.LookAt(rb.position + new Vector3(lookDir.x, 0, lookDir.y) + Vector3.up / 5);
                Ragdoll(true);
                // If the player cancelled an attack with their jumproll, it cancels said attack.
                GetComponent<AttackScript>().StopAllCoroutines();
                GetComponent<AttackScript>().attackOnCooldown = false;
            }
        }
    }

    public void Ragdoll(CallbackContext context)
    {
        if (!isRagdolling && context.performed)
        {
            Ragdoll();
            rb.AddForce(UnityEngine.Random.Range(2, 10), UnityEngine.Random.Range(2, 10), UnityEngine.Random.Range(2, 10));
        }
    }
    public void Ragdoll(bool jumpRolled = false)
    {
        // Activates ragdoll, stopping all control input until the ground is hit.
        if (!isRagdolling) isRagdolling = true;
        playModel.SetActive(false);
        spawnedRagdoll = Instantiate(ragdollModel, transform.position, Quaternion.identity);
        spawnedRagdoll.GetComponentInChildren<Rigidbody>().AddRelativeForce(new Vector3(0, 0, jumpRollSpeed), ForceMode.Impulse);
        spawnedRagdoll.GetComponentInChildren<Rigidbody>().AddRelativeTorque(new Vector3(UnityEngine.Random.Range(10f, 20), 0, UnityEngine.Random.Range(10f, 20)), ForceMode.Impulse);
        canStopRagdolling = false;
        StartCoroutine(DelayedRagdoll());
    }

    public void StopRagdolling()
    {
        if (canStopRagdolling)
        {
            isRagdolling = false;
            playModel.SetActive(true);
        }
    }

    public IEnumerator DelayedRagdoll()
    {
        yield return new WaitForSeconds(1f);
        canStopRagdolling = true;
        yield break;
    }
}
