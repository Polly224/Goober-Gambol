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
        if (PlayerDataStorage.connectedControllers.Count > 0)
        {
            PlayerDataStorage.connectedPlayerObjects.Clear();
            for (int i = 0; i < PlayerDataStorage.connectedControllers.Count; i++)
            {
                PlayerDataStorage.connectedPlayerObjects.Add(pIM.JoinPlayer(i, -1, null, PlayerDataStorage.connectedControllers[i]).gameObject);
            }
        }
    }
    // When a controller presses the start button, that controller joins as a player.
    public void SpawnNewPlayerByButton(CallbackContext context)
    {
        pIM.JoinPlayerFromAction(context);
    }
}
