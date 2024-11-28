using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

public class InputManagerTest : MonoBehaviour
{
    PlayerInputManager pIM;
    [SerializeField] List<GameObject> playerTypesToSpawn;
    // If controllers were already connected in a previous scene, automatically reconnect them in the same order.
    void Start()
    {
        pIM = GetComponent<PlayerInputManager>();
        if(SceneManager.GetActiveScene().name != "CharacterSelect")
        {
            if (PlayerDataStorage.connectedControllers.Count > 0)
            {
                PlayerDataStorage.connectedPlayerObjects.Clear();
                for (int i = 0; i < PlayerDataStorage.connectedControllers.Count; i++)
                {
                    PlayerDataStorage.connectedPlayerObjects.Add(pIM.JoinPlayer(i, -1, null, PlayerDataStorage.connectedControllers[i]).gameObject);
                }
            }
        }
        else
        {
            
            PlayerDataStorage.connectedPlayerObjects.Clear();
            for (int i = 0; i < PlayerDataStorage.connectedPlayerObjects.Count; i++)
            {
                pIM.playerPrefab = PlayerDataStorage.playerCharacters[i] switch
                {
                    PlayerSelectArrow.PickedCharacter.Cat => playerTypesToSpawn[0],
                    PlayerSelectArrow.PickedCharacter.Dog => playerTypesToSpawn[1],
                    PlayerSelectArrow.PickedCharacter.Robot => playerTypesToSpawn[2],
                    PlayerSelectArrow.PickedCharacter.Bunny => playerTypesToSpawn[3],
                    PlayerSelectArrow.PickedCharacter.Unselected => null,
                    _ => null
                };
                if(pIM.playerPrefab != null) PlayerDataStorage.connectedPlayerObjects.Add(pIM.JoinPlayer(i, -1, null, PlayerDataStorage.connectedControllers[i]).gameObject);
            }
        }
    }
    // When a controller presses the start button, that controller joins as a player.
    public void SpawnNewPlayerByButton(CallbackContext context)
    {
        pIM.JoinPlayerFromAction(context);
    }
}
