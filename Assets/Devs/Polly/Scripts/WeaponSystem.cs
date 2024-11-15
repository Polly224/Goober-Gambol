using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    // This script simply stores 2 struct types; Weapon and Hitbox, the latter of which just being used for the Weapon struct.
    // To define a new weapon, just make a variable of the Weapon type, and enter the Weapon's data in the following order:
    // name, damage, startup, recovery, knockback, hitboxes, attack type, swingable, throwable, and bleed amount.
    [System.Serializable]
    public struct Weapon
    {
        public string name;
        public float damage;
        public float startup;
        public float recovery;
        public float knockback;
        public List<Hitbox> hitboxes;
        public AttackType attackType;
        public bool swingable;
        public bool throwable;
        public float bleedAmount;
        public Weapon(string name, float damage, float startup, float recovery, float knockback, List<Hitbox> hitboxes, AttackType attackType, bool swingable, bool throwable, float bleedAmount)
        {
            this.name = name;
            this.damage = damage;
            this.startup = startup;
            this.recovery = recovery;
            this.knockback = knockback;
            this.hitboxes = hitboxes;
            this.attackType = attackType;
            this.swingable = swingable;
            this.throwable = throwable;
            this.bleedAmount = bleedAmount;
        }
    }

    [System.Serializable]
    public struct Hitbox
    {
        public Vector3 positionOffset;
        public float duration;
        public float xSize;
        public float ySize;
        public HitboxShape hitboxShape;
        public Hitbox(Vector3 positionOffset, float duration, float xSize, float ySize, HitboxShape hitboxShape)
        {
            this.positionOffset = positionOffset;
            this.duration = duration;
            this.xSize = xSize;
            this.ySize = ySize;
            this.hitboxShape = hitboxShape;
        }
    }

    public enum AttackType
    {
        Spin360,
        OneArmSwing,
        ReinhardtSwing,
        Stab,
        OneArmBonk
    }
    public enum WeaponGimmick
    {
        None,
        InflictsBleedOnHit,
        Shotgun,
        MetalPipe
    }
    public enum HitboxShape
    {
        Square,
        Circle
    }
}
