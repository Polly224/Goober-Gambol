using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] GameObject deathExplosion;
    [SerializeField] AudioClip deathSound;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerPlacement>().Died();
            other.gameObject.GetComponent<DamageSystem>().damageTaken = 0;
            other.gameObject.GetComponent<PlayerInventory>().RemoveCurrentWeapon();
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Pickup"))
        {
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Ragdoll") || other.CompareTag("RagdollLimb"))
        {
            SpawnedRagdoll sR = other.transform.root.gameObject.GetComponent<SpawnedRagdoll>();
            sR.originPlayer.GetComponent<PlayerPlacement>().Died();
            sR.originPlayer.GetComponent<PlayerInventory>().RemoveCurrentWeapon();
            sR.originPlayer.GetComponent<DamageSystem>().isDizzy = false;
            sR.originPlayer.GetComponent<DamageSystem>().dizzyTimer = 0;
            sR.originPlayer.GetComponent<DamageSystem>().bleedStacks.Clear();
            sR.originPlayer.GetComponent<DamageSystem>().damageTaken = 0;
            sR.originPlayer.GetComponent<DamageSystem>().bleedAmount = 0;
            sR.hasHitCollision = true;
            sR.originPlayer.SetActive(false);
            sR.originPlayer.GetComponent<InputHandler>().StopRagdolling();
            Destroy(other.transform.root.gameObject, 0.1f);
        }
        if(other.CompareTag("Player") || other.CompareTag("Ragdoll"))
        {
            ParticleSystem pS = Instantiate(deathExplosion, other.transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
            pS.Play();
            GameObject spawnedSoundPlayer = new("HitSound");
            spawnedSoundPlayer.transform.position = other.transform.position;
            spawnedSoundPlayer.AddComponent<AudioSource>();
            spawnedSoundPlayer.GetComponent<AudioSource>().clip = deathSound;
            spawnedSoundPlayer.GetComponent<AudioSource>().Play();
            Destroy(spawnedSoundPlayer, deathSound.length);
        }
    }
}
