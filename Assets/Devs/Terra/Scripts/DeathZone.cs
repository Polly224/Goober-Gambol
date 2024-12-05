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
            other.transform.root.gameObject.GetComponent<SpawnedRagdoll>().originPlayer.GetComponent<PlayerPlacement>().Died();
            other.transform.root.gameObject.GetComponent<SpawnedRagdoll>().originPlayer.GetComponent<PlayerInventory>().RemoveCurrentWeapon();
            other.transform.root.gameObject.GetComponent<SpawnedRagdoll>().originPlayer.SetActive(false);
            Destroy(other.transform.root.gameObject);
        }
    }
}
