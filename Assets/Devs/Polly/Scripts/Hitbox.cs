using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public WeaponSystem.Hitbox hitboxData;
    public WeaponSystem.Weapon attackData;
    public List<GameObject> hitPlayers = new();
    [SerializeField] GameObject pickupForThrowable;

    private void Start()
    {
        if (!attackData.throwable)
        {
            Destroy(gameObject, hitboxData.duration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the hitbox hasn't hit the other player before, deal damage and knockback to them. If the hitbox is that of a thrown weapon, it destroys the thrown weapon.
        if(other.gameObject.CompareTag("Player") && !hitPlayers.Contains(other.gameObject) && !other.gameObject.GetComponent<InputHandler>().isRagdolling)
        {
            if (!other.GetComponent<InputHandler>().isInvincible)
            {
                hitPlayers.Add(other.gameObject);
                other.GetComponent<DamageSystem>().GotHit(attackData, transform.position);
            }
            if(attackData.throwable) Destroy(gameObject);
        }

        if ((other.gameObject.CompareTag("Ragdoll") || other.gameObject.CompareTag("RagdollLimb")) && !hitPlayers.Contains(other.gameObject)) 
        {
            if (!hitPlayers.Contains(other.transform.root.gameObject.GetComponent<SpawnedRagdoll>().originPlayer))
            {
                hitPlayers.Add(other.gameObject);
                Vector3 knockbackDir = other.transform.position - transform.position;
                knockbackDir.Normalize();
                knockbackDir *= 3;
                other.GetComponent<Rigidbody>().AddForce(attackData.knockback * other.transform.root.gameObject.GetComponent<SpawnedRagdoll>().originPlayer.GetComponent<DamageSystem>().damageTaken * knockbackDir, ForceMode.VelocityChange);
            }
        }
        if (attackData.throwable)
        {
            if (other.gameObject.CompareTag("Collision"))
            {
                Instantiate(pickupForThrowable, transform.position + Vector3.up, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
