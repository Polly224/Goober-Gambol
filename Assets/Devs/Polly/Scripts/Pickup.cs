using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public WeaponSystem.Weapon pickupWeaponData;
    public float lifeTime = 10f;
    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0) DestroyPickup();
    }
    private void FixedUpdate()
    {
        bool playerInRange = false;
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Player"))
        {
            if(Vector3.Distance(g.transform.position, transform.position) < 2)
            {
                playerInRange = true;
            }
        }
        if (playerInRange) GetComponent<MeshRenderer>().material.color = Color.red;
        else GetComponent<MeshRenderer>().material.color = Color.white;
    }

    private void DestroyPickup() => Destroy(gameObject);

    public WeaponSystem.Weapon GetPickedUp()
    {
        Destroy(gameObject, 0.1f);
        return pickupWeaponData;
    }
}
