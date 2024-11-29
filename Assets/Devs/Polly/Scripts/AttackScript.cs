using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class AttackScript : MonoBehaviour
{
    PlayerInventory inventory;
    [SerializeField] GameObject sphereHitbox;
    [SerializeField] GameObject cubeHitbox;
    public bool attackOnCooldown = false;
    private void Start()
    {
        inventory = GetComponent<PlayerInventory>();
    }
    public IEnumerator StartAttack(WeaponSystem.Weapon weaponUsed)
    {
        // Triggers the animation bound to the weapon the player is holding.
        GetComponentInChildren<Animator>().SetTrigger(weaponUsed.name);

        // Puts the attack on cooldown, so it can't be used for a bit.
        attackOnCooldown = true;
        
        // Waits for an amount of time equal to the weapon's defined startup, then spawns the hitboxes connected to the weapon.
        yield return new WaitForSeconds(weaponUsed.startup);
        for (int i = 0; i < weaponUsed.hitboxes.Count; i++) 
        {
            // For every hitbox object in the weapon used, spawn a hitbox of the correct shape.
            GameObject spawnedHitbox = Instantiate(weaponUsed.hitboxes[i].hitboxShape == WeaponSystem.HitboxShape.Square ? cubeHitbox : sphereHitbox, transform);
            // Set the hitbox's position and scale to the correct values.
            spawnedHitbox.transform.localPosition = weaponUsed.hitboxes[i].positionOffset;
            spawnedHitbox.transform.localScale = new Vector3(weaponUsed.hitboxes[i].xSize, 1, weaponUsed.hitboxes[i].ySize);
            // Set the hitbox's damage and duration, as well as preventing from the hitboxes hitting the player that used it.
            spawnedHitbox.GetComponent<Hitbox>().hitPlayers.Add(gameObject);
            spawnedHitbox.GetComponent<Hitbox>().hitboxData = weaponUsed.hitboxes[i];
            spawnedHitbox.GetComponent<Hitbox>().attackData = weaponUsed;
            yield return null;
        }
        // After the weapon's recovery is up, the weapon can be swung again.
        yield return new WaitForSeconds(weaponUsed.recovery);
        attackOnCooldown = false;
        yield return null;
    }

    public IEnumerator ThrowAttack(WeaponSystem.Weapon weaponUsed)
    {
        GameObject thrownWeapon = Instantiate(weaponUsed.weaponToThrow, transform.position, transform.rotation);
        thrownWeapon.transform.localPosition = weaponUsed.hitboxes[0].positionOffset;
        thrownWeapon.transform.localScale = new Vector3(weaponUsed.hitboxes[0].xSize, weaponUsed.hitboxes[0].xSize, weaponUsed.hitboxes[0].xSize);
        thrownWeapon.GetComponent<Rigidbody>().AddRelativeForce(transform.forward * 5 + transform.up * 2, ForceMode.Impulse);
        thrownWeapon.GetComponent<Hitbox>().hitPlayers.Add(gameObject);
        thrownWeapon.GetComponent<Hitbox>().hitboxData = weaponUsed.hitboxes[0];
        thrownWeapon.GetComponent<Hitbox>().attackData = weaponUsed;
        yield return null;
    }

    public void Attack(CallbackContext context)
    {
        // When the attack button is pressed, if the attack isn't on cooldown and the player isn't ragdolling, the player performs an attack with their equipped weapon.
        if (context.performed && !attackOnCooldown && !GetComponent<InputHandler>().isRagdolling)
        {
            if(inventory.weaponInventory.Count >= inventory.currentWeaponIndex + 1)
            {
                StartCoroutine(StartAttack(inventory.weaponInventory[inventory.currentWeaponIndex]));
            }
            else
            {
                // If the player has no weapon equipped, they just attack with their fists.
                StartCoroutine(StartAttack(WeaponSystem.fists));
            }
        }
    }

    public void AltAttack(CallbackContext context)
    {
        if(context.performed && !GetComponent<InputHandler>().isRagdolling)
        {
            if (inventory.weaponInventory[inventory.currentWeaponIndex].throwable)
            {
                // If the weapon the player's holding is throwable, throw the weapon instead.
                StartCoroutine(ThrowAttack(inventory.weaponInventory[inventory.currentWeaponIndex]));
            }
        }
    }
}
