using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDataStorage : MonoBehaviour
{
    public static PlayerDataStorage instance;
    public static List<InputDevice> connectedControllers = new();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToControllers(InputDevice controllerToAdd)
    {
        connectedControllers.Add(controllerToAdd);
    }
        
}
