using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public WeaponSystem.Hitbox hitboxData;
    public WeaponSystem.Weapon attackData;
    public List<GameObject> hitPlayers = new();

    private void Start()
    {
        if(!attackData.throwable)
        Destroy(gameObject, hitboxData.duration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !hitPlayers.Contains(other.gameObject))
        {
            hitPlayers.Add(other.gameObject);
            other.GetComponent<DamageSystem>().GotHit(attackData, transform.position);
            if(attackData.throwable) Destroy(gameObject);
        }
    }
}
