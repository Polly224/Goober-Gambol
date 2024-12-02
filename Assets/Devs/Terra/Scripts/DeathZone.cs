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
            other.gameObject.transform.position = new Vector3(0, -100, 0);
            other.gameObject.GetComponent<PlayerPlacement>().Died();
            other.gameObject.GetComponent<PlayerInventory>().RemoveCurrentWeapon();
        }
        else if (other.CompareTag("Pickup"))
        {
            Destroy(other.gameObject);
        }
    }
}
