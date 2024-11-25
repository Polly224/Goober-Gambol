using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    public float damageTaken = 0;
    public float bleedAmount = 0;
    public bool isDizzy = false;
    public float dizzyTimer = 0;
    public float dizzyTimerSpeed = 1;
    private List<float> bleedStacks = new();

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
        if(isDizzy) dizzyTimer -= Time.deltaTime;
        if (isDizzy && dizzyTimer <= 0) ExitDizzy();
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
        if (isDizzy) knockbackDirection *= 3f;
        knockbackDirection += Vector3.up * 5;
        GetComponent<Rigidbody>().AddForce(knockbackDirection * totalKnockback);
        GetComponent<InputHandler>().Ragdoll();
        GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(Random.Range(damageTaken, damageTaken * 2), 0, Random.Range(damageTaken, damageTaken * 2)), ForceMode.Impulse);
        if (damageTaken > 50 && !isDizzy) EnterDizzy();
    }
    private void EnterDizzy()
    {
        isDizzy = true;
        dizzyTimer = damageTaken / dizzyTimerSpeed;
    }

    private void ExitDizzy()
    {
        isDizzy = false;
        dizzyTimer = 0;
    }
}
