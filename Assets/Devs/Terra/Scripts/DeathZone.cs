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
            RagdollStateCheck = true;
            Debug.Log(RagdollStateCheck);
        }
        else if (other.CompareTag("Pickup"))
        {
            Destroy(other.gameObject);
        }
    }
}
