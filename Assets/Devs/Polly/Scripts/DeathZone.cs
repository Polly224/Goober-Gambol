using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public bool RagdollStateCheck = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerPlacement>().Died();
            other.gameObject.GetComponent<PlayerInventory>().RemoveCurrentWeapon();
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Pickup"))
        {
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Ragdoll"))
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
    }
}
