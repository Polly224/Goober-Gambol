using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInventory : MonoBehaviour
{
    public List<WeaponSystem.Weapon> weaponInventory = new();
    public int currentWeaponIndex = 0;
    [SerializeField] Transform handToHoldWeapons;
    [SerializeField] List<GameObject> weaponsToHold;
    // When the player presses either of the bumpers on their controller, they switch weapons.
    public void SwitchWeapons(CallbackContext context)
    {
        if (context.performed) SwitchWeaponsToIndex(currentWeaponIndex == 0 ? 1 : 0);
    }

    public void SwitchWeaponsToIndex(int val)
    {
        currentWeaponIndex = val;
        int indexToAddFrom = -1;
        if(weaponInventory.Count > currentWeaponIndex)
        {
            indexToAddFrom = weaponInventory[currentWeaponIndex].name switch
            {
                "baseballbat" => 0,
                "brick" => 1,
                "cleaver" => 2,
                "greataxe" => 3,
                "metalpipe" => 4,
                "ninjastar" => 5,
                "pike" => 6,
                "plasticbottle" => 7,
                _ => -1
            };
        }
        if(handToHoldWeapons.childCount > 0) Destroy(handToHoldWeapons.GetChild(0).gameObject);
        if(indexToAddFrom != -1)
        {
            GameObject weaponSpawned = Instantiate(weaponsToHold[indexToAddFrom], handToHoldWeapons.transform.position, handToHoldWeapons.rotation);
            weaponSpawned.transform.localScale = Vector3.one;
            weaponSpawned.transform.SetParent(handToHoldWeapons, true);
        }
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
                SwitchWeaponsToIndex(0);
            }
            else if (weaponInventory.Count == 1) 
            {
                weaponInventory.Add(closestPickup.GetComponent<Pickup>().GetPickedUp());
                SwitchWeaponsToIndex(1);
            }
            else if (weaponInventory.Count == 2) 
            {
                GameObject replacementPickup = Instantiate(closestPickup, closestPickup.transform.position, Quaternion.identity);
                replacementPickup.GetComponent<Pickup>().pickupWeaponData = weaponInventory[currentWeaponIndex];
                weaponInventory[currentWeaponIndex] = closestPickup.GetComponent<Pickup>().GetPickedUp();
                SwitchWeaponsToIndex(currentWeaponIndex);
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

    public void RemoveCurrentWeapon()
    {
        if(weaponInventory.Count > currentWeaponIndex) weaponInventory.RemoveAt(currentWeaponIndex);
        SwitchWeaponsToIndex(currentWeaponIndex);
    }
}
