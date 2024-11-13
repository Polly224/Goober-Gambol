using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public struct Weapon
    {
        public string name;
        public float damage;
        public float startup;
        public float recovery;
        public float knockback;
        public List<Hitbox> hitboxes;
        public AttackType attackType;
        public bool throwable;
        public Weapon(string name, float damage, float startup, float recovery, float knockback, List<Hitbox> hitboxes, AttackType attackType, bool throwable)
        {
            this.name = name;
            this.damage = damage;
            this.startup = startup;
            this.recovery = recovery;
            this.knockback = knockback;
            this.hitboxes = hitboxes;
            this.attackType = attackType;
            this.throwable = throwable;
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
    public struct Hitbox
    {
        public Vector3 positionOffset;
        public float duration;
        public Hitbox(Vector3 positionOffset, float duration)
        {
            this.positionOffset = positionOffset;
            this.duration = duration;
        }
    }
}
