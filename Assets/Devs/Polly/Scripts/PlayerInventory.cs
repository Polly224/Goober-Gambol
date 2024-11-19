using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInventory : MonoBehaviour
{
    public List<WeaponSystem.Weapon> weaponInventory = new();
    public int currentWeaponIndex = 0;
    private void Update()
    {
        if(weaponInventory.Count >= currentWeaponIndex + 1)
        {
            Debug.Log(weaponInventory[currentWeaponIndex]);
        }
    }
    // When the player presses either of the bumpers on their controller, they switch weapons.
    public void SwitchWeapons(CallbackContext context)
    {
        if(context.performed) currentWeaponIndex = currentWeaponIndex == 0 ? 1 : 0;
    }

    // When the pickup button is pressed, this script checks whether there's a weapon to pick up in the first place. If there is, it picks up said weapon.
    public void PickUpTest(CallbackContext context)
    {
        bool pickupInRange = false;
        foreach(GameObject p in GameObject.FindGameObjectsWithTag("Pickup"))
        {
            if(Vector3.Distance(transform.position, p.transform.position) < 2)
            {
                pickupInRange = true;
            }
        }
        if (pickupInRange && context.performed) PickUpWeapon();
    }

    // Only gets called if there's a weapon in range to pick up. Picks up the closest weapon, and either adds it to the player inventory or swaps the current gun.
    public void PickUpWeapon()
    {
        GameObject closestPickup = GetClosestPickup();
        if (closestPickup != null)
        {
            if(weaponInventory.Count == 0)
            {
                weaponInventory.Add(closestPickup.GetComponent<Pickup>().GetPickedUp());
                currentWeaponIndex = 0;
            }
            else if (weaponInventory.Count == 1) 
            {
                weaponInventory.Add(closestPickup.GetComponent<Pickup>().GetPickedUp());
                currentWeaponIndex = 1;
            }
            else if (weaponInventory.Count == 2) 
            {
                GameObject replacementPickup = Instantiate(closestPickup, closestPickup.transform.position, Quaternion.identity);
                replacementPickup.GetComponent<Pickup>().pickupWeaponData = weaponInventory[currentWeaponIndex];
                weaponInventory[currentWeaponIndex] = closestPickup.GetComponent<Pickup>().GetPickedUp();
            }

        }
    }
    // Checks which weapon within range of the player is closest to them, then returns that gameObject.
    public GameObject GetClosestPickup()
    {
        float lowestDistance = 10000;
        GameObject closestPickup = null;
        foreach (GameObject p in GameObject.FindGameObjectsWithTag("Pickup"))
        {
            if (Vector3.Distance(p.transform.position, transform.position) < lowestDistance)
            {
                closestPickup = p;
                lowestDistance = Vector3.Distance(p.transform.position, transform.position);
            }
        }
        return closestPickup;
    }
}
