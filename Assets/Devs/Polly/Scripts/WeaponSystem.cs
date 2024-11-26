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
        public WeaponGimmick weaponGimmick;
        public bool swingable;
        public bool throwable;
        public float bleedAmount;
        public Weapon(string name, float damage, float startup, float recovery, float knockback, List<Hitbox> hitboxes, AttackType attackType, WeaponGimmick weaponGimmick, bool swingable, bool throwable, float bleedAmount)
        {
            this.name = name;
            this.damage = damage;
            this.startup = startup;
            this.recovery = recovery;
            this.knockback = knockback;
            this.hitboxes = hitboxes;
            this.attackType = attackType;
            this.weaponGimmick = weaponGimmick;
            this.swingable = swingable;
            this.throwable = throwable;
            this.bleedAmount = bleedAmount;
        }
    }

    [System.Serializable]
    public struct Hitbox
    {
        // Hitboxes are spawned when you perform an attack. positionOffset is based off the player's position.
        // For example, if positionOffset was set to (0, 0, 1) would spawn a hitbox 1 unit in front of the player.
        // hitboxShape denotes whether the hitbox will be a box collider or a sphere collider. Sphere colliders always use the xSize, always being a 1:1:1 size ratio.
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
        // Denotes the animation the attack will use when the player attacks.
        Punch,
        Spin360,
        OneArmSwing,
        ReinhardtSwing,
        Stab,
        OneArmBonk
    }
    public enum WeaponGimmick
    {
        // Names should speak for themselves. The weapon's bleedAmount variable does nothing unless its WeaponGimmick is set to InflictsBleedOnHit.
        None,
        InflictsBleedOnHit,
        Shotgun,
        MetalPipe
    }
    public enum HitboxShape
    {
        // All these squares make a circle...
        Square,
        Circle
    }

    public static Weapon fists = new()
    {
        name = "fists",
        damage = 5,
        startup = 0.05f,
        recovery = 1,
        knockback = 2,
        hitboxes = new()
        {
            new Hitbox()
            {
                positionOffset = new Vector3(0, 0, 1),
                duration = 0.1f,
                xSize = 2,
                ySize = 1,
                hitboxShape = HitboxShape.Circle
            }
        },
        attackType = AttackType.Punch,
        swingable = true,
        throwable = false,
        bleedAmount = 0
    };
}
