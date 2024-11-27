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
        GetComponentInChildren<Animator>().SetTrigger(weaponUsed.name);
        attackOnCooldown = true;
        yield return new WaitForSeconds(weaponUsed.startup);
        for (int i = 0; i < weaponUsed.hitboxes.Count; i++) 
        {
            GameObject spawnedHitbox = Instantiate(weaponUsed.hitboxes[i].hitboxShape == WeaponSystem.HitboxShape.Square ? cubeHitbox : sphereHitbox, transform);
            spawnedHitbox.transform.localPosition = weaponUsed.hitboxes[i].positionOffset;
            spawnedHitbox.transform.localScale = new Vector3(weaponUsed.hitboxes[i].xSize, 1, weaponUsed.hitboxes[i].ySize);
            spawnedHitbox.GetComponent<Hitbox>().hitPlayers.Add(gameObject);
            spawnedHitbox.GetComponent<Hitbox>().hitboxData = weaponUsed.hitboxes[i];
            spawnedHitbox.GetComponent<Hitbox>().attackData = weaponUsed;
            yield return null;
        }
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
        if (context.performed && !attackOnCooldown && !GetComponent<InputHandler>().isRagdolling)
        {
            if(inventory.weaponInventory.Count >= inventory.currentWeaponIndex + 1)
            {
                StartCoroutine(StartAttack(inventory.weaponInventory[inventory.currentWeaponIndex]));
            }
            else
            {
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
                StartCoroutine(ThrowAttack(inventory.weaponInventory[inventory.currentWeaponIndex]));
            }
        }
    }
}
