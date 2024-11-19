using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public WeaponSystem.Hitbox hitboxData;
    public WeaponSystem.Weapon attackData;
    private List<GameObject> hitPlayers = new();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !hitPlayers.Contains(other.gameObject))
        {

        }
    }
}
