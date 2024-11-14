using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<WeaponSystem.Weapon> weaponInventory = new();
    public int currentWeaponIndex = 0;

}
