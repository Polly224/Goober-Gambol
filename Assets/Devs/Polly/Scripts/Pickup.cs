using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public WeaponSystem.Weapon pickupWeaponData;
    public float lifeTime = 10f;
    public float rotationSpeed = 1f;
    private void Update()
    {
        // Pickup destroys itself if it's left untouched for too long.
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0) DestroyPickup();
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y + rotationSpeed, -45));
    }
    private void FixedUpdate()
    {
        // Every few frames, a pickup checks whether any of the players are within picking up range of it. If it finds a player, it turns red to show that.
        bool playerInRange = false;
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Player"))
        {
            if(Vector3.Distance(g.transform.position, transform.position) < 2)
            {
                playerInRange = true;
            }
        }
        if (playerInRange) GetComponent<MeshRenderer>().material.color = Color.red;
        // If there isn't a player in range, the pickup will be white instead.
        else GetComponent<MeshRenderer>().material.color = Color.white;
    }

    // Called when the pickup is left alone for too long. Currently just a placeholder, whoops! Should probably add some effects at some point.
    private void DestroyPickup() => Destroy(gameObject);

    public WeaponSystem.Weapon GetPickedUp()
    {
        // Returns the weapon type that got picked up to the player's inventory.
        Destroy(gameObject, 0.1f);
        return pickupWeaponData;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<PlayerInventory>().weaponInventory.Count < 2)
            other.gameObject.GetComponent<PlayerInventory>().PickUpWeapon();
        }
    }
}
