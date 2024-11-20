using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    public float damageTaken = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GotHit(WeaponSystem.Weapon weaponHitBy)
    {
        damageTaken += weaponHitBy.damage * Random.Range(0.8f, 1.2f);
    }
}
