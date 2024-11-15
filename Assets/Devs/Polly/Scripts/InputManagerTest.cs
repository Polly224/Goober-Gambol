using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

public class InputManagerTest : MonoBehaviour
{
    PlayerInputManager pIM;
    // If controllers were already connected in a previous scene, automatically reconnect them in the same order.
    void Start()
    {
        pIM = GetComponent<PlayerInputManager>();
        if(PlayerDataStorage.connectedControllers.Count > 0)
        {
            for (int i = 0; i < PlayerDataStorage.connectedControllers.Count; i++)
            {
                pIM.JoinPlayer(i, -1, null, PlayerDataStorage.connectedControllers[i]);
            }
        }
    }

    // Test thing. When you press Enter, it runs the SpawnPlayer script.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) SpawnPlayer();
    }
    
    // Test thing. Just loads the 2nd scene in the builder, used this for testing keeping the same controllers assigned in the same positions between scenes.
    public void SpawnPlayer()
    {
        SceneManager.LoadScene(1);
    }
    // When a controller presses the start button, that controller joins as a player.
    public void SpawnNewPlayerByButton(CallbackContext context)
    {
        pIM.JoinPlayerFromAction(context);
    }
}
