using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedRagdoll : MonoBehaviour
{
    public bool hasHitCollision = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Collision")) hasHitCollision = true;
    }
}
