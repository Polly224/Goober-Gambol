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
    private void Awake()
    {
        pIM = GetComponent<PlayerInputManager>();
        StartCoroutine(SpawnRoutine());
    }

    // When a controller presses the start button, that controller joins as a player.
    public void SpawnNewPlayerByButton(CallbackContext context)
    {
        pIM.JoinPlayerFromAction(context);
    }

    private IEnumerator SpawnRoutine()
    {
        // If controllers were already connected in a previous scene, automatically reconnect them in the same order.
        if (SceneManager.GetActiveScene().name == "CharacterSelect")
        {
            if (PlayerDataStorage.connectedControllers.Count > 0)
            {
                pIM.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
                PlayerDataStorage.connectedPlayerObjects.Clear();
                for (int i = 0; i < PlayerDataStorage.connectedControllers.Count; i++)
                {
                    PlayerDataStorage.connectedPlayerObjects.Add(pIM.JoinPlayer(i, -1, null, PlayerDataStorage.connectedControllers[i]).gameObject);
                }
            }
        }
        else
        {
            // Gets the character the players have picked, and spawns the correct model for each.
            PlayerDataStorage.connectedPlayerObjects.Clear();
            for (int i = 0; i < PlayerDataStorage.connectedControllers.Count; i++)
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
                pIM.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
                if (pIM.playerPrefab != null) PlayerDataStorage.connectedPlayerObjects.Add(pIM.JoinPlayer(i, -1, null, PlayerDataStorage.connectedControllers[i]).gameObject);
            }
        }
        yield return null;
        if(SceneManager.GetActiveScene().name != "CharacterSelect")
        GameObject.Find("Camera").GetComponent<PlayerCameraFollower>().playersDoneSpawing = true;
        yield break;
    }
}
