using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedRagdollLimb : MonoBehaviour
{
    SpawnedRagdoll sR;
    void Start()
    {
        sR = GetComponentInParent<SpawnedRagdoll>();
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        if (sR.originPlayer.GetComponent<InputHandler>().canStopRagdolling)
        {
            if(collision.gameObject.CompareTag("Collision")) sR.hasHitCollision = true;
        }
    }
}
