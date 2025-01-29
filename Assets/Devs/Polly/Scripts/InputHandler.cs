using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Unity.VisualScripting;
using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.InputSystem;
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
    private float jumpRollSpeed = 15;
    public bool isLooking = false;
    public bool isMoving = false;
    public bool isRagdolling = false;
    public bool isInvincible = false;
    public bool canGoInvincible = false;
    public bool canStopRagdolling = true;
    private Animator modelAnim;
    private Rigidbody rb;
    [SerializeField]
    private float turnSpeed = 1;
    private GameObject playModel;
    public GameObject ragdollModel;
    public GameObject spawnedRagdoll;
    private List<Transform> ragdollPositions;
    private bool buttonPressedForRagdoll = false;
    private ParticleSystem dizzyParticle;
    private ParticleSystem bloodParticle;
    [SerializeField] GameObject dashParticle;
    private GameObject pointerArrow;
    private void Awake()
    {
        // Gets the player input and the player's material for later usage.
        playerInput = GetComponent<PlayerInput>();
        cubeMat = GetComponent<MeshRenderer>().material;
        rb = GetComponent<Rigidbody>();
        modelAnim = GetComponentInChildren<Animator>();
        playModel = transform.GetChild(0).gameObject;
        dizzyParticle = transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
        bloodParticle = transform.GetChild(2).gameObject.GetComponent<ParticleSystem>();
        pointerArrow = transform.GetChild(3).gameObject;
        playModel.SetActive(true);
    }

    private void Start()
    {
        playerInput.ActivateInput();
        // When a player is added, their controller gets added to the controller list.
        if(!PlayerDataStorage.connectedControllers.Contains(playerInput.GetDevice<InputDevice>()))
        PlayerDataStorage.instance.AddToControllers(playerInput.GetDevice<InputDevice>());
            transform.GetChild(3).GetComponent<SpriteRenderer>().color = playerInput.playerIndex switch
            {
                0 => Color.red,
                1 => Color.blue,
                2 => Color.green,
                3 => Color.magenta,
                _ => Color.white
            };
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
        if (isMoving && !isRagdolling && movementSpeed <= maxMovementSpeed)
            movementSpeed += acceleration * Time.deltaTime;
        if (movementSpeed > maxMovementSpeed) movementSpeed = maxMovementSpeed;
        if (!isMoving) movementSpeed = 0;

        lookDir = (isMoving && !isLooking) ? movementDir : lookDir;
        if (!isRagdolling && isMoving) SetMoveDirection(movementDir);
        if (!isRagdolling && (isLooking || isMoving)) SetLookDirection(lookDir);

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
        if (spawnedRagdoll != null) {
            if (spawnedRagdoll.GetComponent<SpawnedRagdoll>().hasHitCollision)
            {
                if (isRagdolling && !GetComponent<DamageSystem>().isDizzy && canStopRagdolling && !buttonPressedForRagdoll) StartCoroutine(DelayedExitRagdoll(0.4f));
            }
        }
        if(!isRagdolling)
        {
            float yVal = rb.velocity.y;
            rb.velocity /= 1.2f;
            rb.velocity = new Vector3(rb.velocity.x, yVal, rb.velocity.z);
        }
        // if (!isRagdolling) rb.AddTorque(-rb.GetAccumulatedTorque() * 10);

        // Animation functions.
        modelAnim.SetBool("IsMoving", isMoving);
        modelAnim.SetFloat("MovementSpeed", movementSpeed / maxMovementSpeed * (Mathf.Abs(movementDir.x) + Mathf.Abs(movementDir.y)));
        if (GetComponent<DamageSystem>().isDizzy)
        {
            if (!dizzyParticle.isPlaying) dizzyParticle.Play();
        }
        else if (dizzyParticle.isPlaying) dizzyParticle.Stop();
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
                Instantiate(dashParticle, transform.position, Quaternion.Euler(0, -transform.rotation.eulerAngles.y, 0));
                Ragdoll(true, new Vector3(lookDir.x, 0, lookDir.y) * jumpRollSpeed * 1.25f + Vector3.up * 0.5f * jumpRollSpeed, false);
                // If the player cancelled an attack with their jumproll, it cancels said attack.
                GetComponent<AttackScript>().StopAllCoroutines();
                GetComponent<AttackScript>().attackOnCooldown = false;
            }
        }
    }

    public void Ragdoll(CallbackContext context)
    {
        if (isRagdolling && context.performed) buttonPressedForRagdoll = false;
        if (!isRagdolling && context.performed)
        {
            Ragdoll();
            buttonPressedForRagdoll = true;
            foreach (Rigidbody r in spawnedRagdoll.GetComponentsInChildren<Rigidbody>())
            {
                r.AddForce(UnityEngine.Random.Range(2, 10), UnityEngine.Random.Range(2, 10), UnityEngine.Random.Range(2, 10));
            }
        }
    }
    public void Ragdoll(bool addForce = false, Vector3? forceAdded = null, bool gainsInvulOnExit = true)
    {
        // Activates ragdoll, stopping all control input until the ground is hit.
        canGoInvincible = gainsInvulOnExit;
        if (!isRagdolling) isRagdolling = true;
        playModel.SetActive(false);
        spawnedRagdoll = Instantiate(ragdollModel, transform.position, Quaternion.identity);
        dizzyParticle.transform.localPosition = Vector3.zero;
        dizzyParticle.transform.SetParent(spawnedRagdoll.transform.GetChild(4).GetChild(0), false);
        bloodParticle.transform.SetParent(spawnedRagdoll.transform.GetChild(4).GetChild(0), false);
        pointerArrow.transform.SetParent(spawnedRagdoll.transform.GetChild(4).GetChild(0), true);
        spawnedRagdoll.GetComponent<SpawnedRagdoll>().originPlayer = gameObject;
        spawnedRagdoll.transform.rotation = Quaternion.LookRotation(new Vector3(lookDir.x, 0, lookDir.y));
        if (addForce)
        {
            foreach(Rigidbody r in spawnedRagdoll.GetComponentsInChildren<Rigidbody>())
            {
                r.AddForce((Vector3)forceAdded, ForceMode.Impulse);
            }
        }
        canStopRagdolling = false;
        StartCoroutine(DelayedRagdoll());
    }

    public void StopRagdolling(bool fromRoundChange = false)
    {
        if (canGoInvincible)
        {
            isInvincible = true;
            StartCoroutine(LoseInvul(2));
            canGoInvincible = false;
        }
        if(!fromRoundChange)
        if (!GetComponent<DamageSystem>().isDizzy && spawnedRagdoll.GetComponent<SpawnedRagdoll>().hasHitCollision)
        {
            isRagdolling = false;
            transform.position = spawnedRagdoll.transform.GetChild(4).GetChild(0).position;
            playModel.SetActive(true);
            dizzyParticle.transform.SetParent(transform, false);
            dizzyParticle.transform.position += transform.up * 0.8f;
            bloodParticle.transform.SetParent(transform, false);
            pointerArrow.transform.SetParent(transform, true);
            Destroy(spawnedRagdoll);
        }
        if (fromRoundChange && spawnedRagdoll != null)
        {
            isRagdolling = false;
            transform.position = spawnedRagdoll.transform.GetChild(4).GetChild(0).position;
            playModel.SetActive(true);
            dizzyParticle.transform.SetParent(transform, false);
            dizzyParticle.transform.position += transform.up * 0.8f;
            bloodParticle.transform.SetParent(transform, false);
            pointerArrow.transform.SetParent(transform, true);
            Destroy(spawnedRagdoll);
        }
    }

    public IEnumerator DelayedRagdoll()
    {
        yield return new WaitForSeconds(1f);
        canStopRagdolling = true;
        yield break;
    }

    public IEnumerator DelayedExitRagdoll(float delay)
    {
        canStopRagdolling = false;
        yield return new WaitForSeconds(delay);
        StopRagdolling();
        yield break;
    }

    public IEnumerator LoseInvul(float delay)
    {
        yield return new WaitForSeconds(delay);
        isInvincible = false;
        yield break;
    }
}
