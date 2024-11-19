using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class AttackScript : MonoBehaviour
{
    PlayerInventory inventory;
    [SerializeField] GameObject sphereHitbox;
    [SerializeField] GameObject cubeHitbox;
    private void Start()
    {
        inventory = GetComponent<PlayerInventory>();
    }
    public IEnumerator StartAttack(WeaponSystem.Weapon weaponUsed)
    {
        // Play animation first, then wait to spawn hitboxes.
        yield return new WaitForSeconds(weaponUsed.startup);
        for (int i = 0; i < weaponUsed.hitboxes.Count; i++) 
        {
            Instantiate(weaponUsed.hitboxes[i].hitboxShape == WeaponSystem.HitboxShape.Square ? cubeHitbox : sphereHitbox, transform.position + weaponUsed.hitboxes[i].positionOffset, transform.rotation);
        }
        yield return null;
    }

    public void Attack(CallbackContext context)
    {
        if (context.performed)
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


}
