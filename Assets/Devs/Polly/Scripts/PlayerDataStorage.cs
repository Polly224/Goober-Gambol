using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDataStorage : MonoBehaviour
{
    public static PlayerDataStorage instance;
    public static List<InputDevice> connectedControllers = new();

    // Keeps the connected controllers stored between scenes.
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void AddToControllers(InputDevice controllerToAdd)
    {
        connectedControllers.Add(controllerToAdd);
    }
        
}
