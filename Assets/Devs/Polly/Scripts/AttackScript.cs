using System.Collections;
using System.Collections.Generic;
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
        GetComponentInChildren<Animator>().SetTrigger(weaponUsed.attackType switch
        {
            WeaponSystem.AttackType.Punch => "Punch",
            WeaponSystem.AttackType.Spin360 => "Spin",
            WeaponSystem.AttackType.OneArmSwing => "OneArmSwing",
            WeaponSystem.AttackType.ReinhardtSwing => "ReinhardtSwing",
            WeaponSystem.AttackType.Stab => "Stab",
            WeaponSystem.AttackType.OneArmBonk => "OneArmBonk",
            _ => ""
        });

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
        yield break;
    }

    public IEnumerator ThrowAttack(WeaponSystem.Weapon weaponUsed)
    {
        GetComponentInChildren<Animator>().SetTrigger("Throw");
        attackOnCooldown = true;
        yield return new WaitForSeconds(weaponUsed.startup);
        GameObject thrownWeapon = Instantiate(weaponUsed.weaponToThrow, transform);
        thrownWeapon.GetComponent<Hitbox>().hitPlayers.Add(gameObject);
        thrownWeapon.GetComponent<Hitbox>().hitboxData = weaponUsed.hitboxes[0];
        thrownWeapon.GetComponent<Hitbox>().attackData = weaponUsed;
        thrownWeapon.transform.localPosition = thrownWeapon.transform.localPosition + weaponUsed.hitboxes[0].positionOffset;
        thrownWeapon.transform.SetParent(null, true);
        thrownWeapon.transform.localScale = new Vector3(weaponUsed.hitboxes[0].xSize, weaponUsed.hitboxes[0].xSize, weaponUsed.hitboxes[0].xSize);
        yield return null;
        thrownWeapon.transform.LookAt(thrownWeapon.transform.localPosition + new Vector3(GetComponent<InputHandler>().lookDir.x, thrownWeapon.transform.position.y, GetComponent<InputHandler>().lookDir.y));
        yield return null;
        thrownWeapon.GetComponent<Rigidbody>().AddForce(transform.forward * 20 + transform.up * 5, ForceMode.Impulse);
        yield return null;
        inventory.RemoveCurrentWeapon();
        yield return new WaitForSeconds(weaponUsed.recovery);
        attackOnCooldown = false;
        yield break;
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
        if(context.performed && !GetComponent<InputHandler>().isRagdolling && inventory.weaponInventory.Count >= inventory.currentWeaponIndex + 1)
        {
            if (inventory.weaponInventory[inventory.currentWeaponIndex].throwable)
            {
                // If the weapon the player's holding is throwable, throw the weapon instead.
                StartCoroutine(ThrowAttack(inventory.weaponInventory[inventory.currentWeaponIndex]));
            }
        }
    }
}
