using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

public class InputManagerTest : MonoBehaviour
{
    PlayerInputManager pIM;
    void Start()
    {
        pIM = GetComponent<PlayerInputManager>();
        if(pIM.joinBehavior == PlayerJoinBehavior.JoinPlayersManually)
        {
            for (int i = 0; i < PlayerDataStorage.connectedControllers.Count; i++)
            {
                pIM.JoinPlayer(i, -1, null, PlayerDataStorage.connectedControllers[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) SpawnPlayer();
    }

    [ContextMenu("SpawnPlayer")] 
    public void SpawnPlayer()
    {
        SceneManager.LoadScene(1);
    }

    public void SpawnNewPlayerByButton(CallbackContext context)
    {
        pIM.JoinPlayerFromAction(context);
    }
}
