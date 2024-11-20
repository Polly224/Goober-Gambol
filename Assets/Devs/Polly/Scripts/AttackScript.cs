using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class AttackScript : MonoBehaviour
{
    PlayerInventory inventory;
    [SerializeField] GameObject sphereHitbox;
    [SerializeField] GameObject cubeHitbox;
    private bool attackOnCooldown = false;
    private void Start()
    {
        inventory = GetComponent<PlayerInventory>();
    }
    public IEnumerator StartAttack(WeaponSystem.Weapon weaponUsed)
    {
        // Play animation first, then wait to spawn hitboxes.
        attackOnCooldown = true;
        yield return new WaitForSeconds(weaponUsed.startup);
        for (int i = 0; i < weaponUsed.hitboxes.Count; i++) 
        {
            GameObject spawnedHitbox = Instantiate(weaponUsed.hitboxes[i].hitboxShape == WeaponSystem.HitboxShape.Square ? cubeHitbox : sphereHitbox, transform);
            spawnedHitbox.transform.localPosition = weaponUsed.hitboxes[i].positionOffset;
            spawnedHitbox.transform.localScale = new Vector3(weaponUsed.hitboxes[i].xSize, 1, weaponUsed.hitboxes[i].ySize);
            spawnedHitbox.GetComponent<Hitbox>().hitPlayers.Add(gameObject);
            spawnedHitbox.GetComponent<Hitbox>().hitboxData = weaponUsed.hitboxes[i];
            spawnedHitbox.GetComponent<Hitbox>().attackData = weaponUsed;
            yield return null;
        }
        yield return new WaitForSeconds(weaponUsed.recovery);
        attackOnCooldown = false;
        yield return null;
    }

    public void Attack(CallbackContext context)
    {
        if (context.performed && !attackOnCooldown && !GetComponent<InputHandler>().isRagdolling)
        {
            if(inventory.weaponInventory.Count >= inventory.currentWeaponIndex + 1)
            {
                StartCoroutine(StartAttack(inventory.weaponInventory[inventory.currentWeaponIndex]));
            }
            else
            {
                StartCoroutine(StartAttack(WeaponSystem.fists));
            }
        }
    }


}
