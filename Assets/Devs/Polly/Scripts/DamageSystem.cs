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
    private ParticleSystem bleedEffect;

    private void Start()
    {
        bleedEffect = transform.GetChild(2).gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        float totalBleed = bleedAmount;
        for(int i = 0; i < bleedStacks.Count; i++)
        {
            if(bleedStacks[i] > 0)
            {
                bleedStacks[i] -= Time.deltaTime;
                damageTaken += Time.deltaTime;
                totalBleed += bleedStacks[i];
            }
        }
        var emission = bleedEffect.emission;
        emission.rateOverTime = totalBleed;
        if(isDizzy) dizzyTimer -= Time.deltaTime;
        if (isDizzy && dizzyTimer <= 0) ExitDizzy();
    }

    public void GotHit(WeaponSystem.Weapon weaponHitBy, Vector3 hitboxPosition)
    {
        float totalDamage = weaponHitBy.damage * Random.Range(0.8f, 1.2f);
        float totalKnockback = weaponHitBy.knockback;
        bool metalPipeTriggered = false;
        switch (weaponHitBy.weaponGimmick)
        {
            case WeaponSystem.WeaponGimmick.InflictsBleedOnHit:
                bleedStacks.Add(weaponHitBy.bleedAmount * Random.Range(0.8f, 1.2f));
                break;
            case WeaponSystem.WeaponGimmick.MetalPipe:
                metalPipeTriggered = Random.Range(0, 10) == 9;
                break;
            default:
                break;
        }
        AudioClip hitSoundToPlay = weaponHitBy.hitSound;
        GameObject spawnedSoundPlayer = new("HitSound");
        spawnedSoundPlayer.AddComponent<AudioSource>();
        if (weaponHitBy.weaponGimmick == WeaponSystem.WeaponGimmick.MetalPipe)
        {
            if (metalPipeTriggered)
            {
                totalKnockback *= 3;
                totalDamage *= 3;
            }
        }
        spawnedSoundPlayer.GetComponent<AudioSource>().clip = hitSoundToPlay;
        spawnedSoundPlayer.GetComponent<AudioSource>().Play();
        damageTaken += totalDamage;
        totalKnockback *= damageTaken;
        Vector3 knockbackDirection = transform.position - hitboxPosition;
        knockbackDirection.Normalize();
        if (GetComponent<InputHandler>().isRagdolling) knockbackDirection *= 1.5f;
        if (isDizzy) knockbackDirection *= 3f;
        knockbackDirection += Vector3.up;
        GetComponent<InputHandler>().Ragdoll(true, knockbackDirection * totalKnockback / 5);
        /*GetComponent<Rigidbody>().AddForce(knockbackDirection * totalKnockback);
        GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(Random.Range(damageTaken, damageTaken * 2), 0, Random.Range(damageTaken, damageTaken * 2)), ForceMode.Impulse);*/
        if (damageTaken > 50 && !isDizzy) EnterDizzy();
        GetComponent<AttackScript>().StopAllCoroutines();
        GetComponent<AttackScript>().attackOnCooldown = false;
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
