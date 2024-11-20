using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    public float damageTaken = 0;
    public float bleedAmount = 0;
    private List<float> bleedStacks = new();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < bleedStacks.Count; i++)
        {
            if(bleedStacks[i] > 0)
            {
                bleedStacks[i] -= Time.deltaTime;
                damageTaken += Time.deltaTime;
            }
        }
    }

    public void GotHit(WeaponSystem.Weapon weaponHitBy, Vector3 hitboxPosition)
    {
        float totalDamage = weaponHitBy.damage * Random.Range(0.8f, 1.2f);
        float totalKnockback = weaponHitBy.knockback;
        switch (weaponHitBy.weaponGimmick)
        {
            case WeaponSystem.WeaponGimmick.InflictsBleedOnHit:
                bleedStacks.Add(weaponHitBy.bleedAmount * Random.Range(0.8f, 1.2f));
                break;
            case WeaponSystem.WeaponGimmick.MetalPipe:
                if(Random.Range(1, 11) == 10)
                {
                    // Play metal pipe sound effect.
                    totalKnockback *= 3;
                    totalDamage *= 3;
                }
                break;
            default:
                break;
        }
        damageTaken += totalDamage;
        totalKnockback *= damageTaken;
        Debug.Log(totalKnockback);
        Vector3 knockbackDirection = transform.position - hitboxPosition;
        knockbackDirection.Normalize();
        knockbackDirection *= 2;
        if (GetComponent<InputHandler>().isRagdolling) knockbackDirection *= 1.5f;
        knockbackDirection += Vector3.up * 5;
        GetComponent<Rigidbody>().AddForce(knockbackDirection * totalKnockback);
        GetComponent<InputHandler>().isRagdolling = true;
    }
}
