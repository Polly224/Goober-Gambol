using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInventory : MonoBehaviour
{
    public List<WeaponSystem.Weapon> weaponInventory = new();
    public int currentWeaponIndex = 0;
    public void SwitchWeapons(CallbackContext context)
    {
        currentWeaponIndex = currentWeaponIndex == 0 ? 1 : 0;
    }

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
