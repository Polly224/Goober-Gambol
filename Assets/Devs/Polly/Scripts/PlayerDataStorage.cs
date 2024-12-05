using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDataStorage : MonoBehaviour
{
    public static PlayerDataStorage instance;
    public static List<InputDevice> connectedControllers = new();
    public static List<GameObject> connectedPlayerObjects = new();
    public static List<PlayerSelectArrow.PickedCharacter> playerCharacters = new();
    public enum PickedStage
    {
        None,
        Rooftop,
        Docks
    }
    public static PickedStage pickedStage;

    // Keeps the connected controllers stored between scenes.
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        // Makes sure the list of playercharacters is 4 in length.
        if (playerCharacters.Count == 0) for (int i = 0; i < 4; i++) playerCharacters.Add(PlayerSelectArrow.PickedCharacter.Unselected);
    }

    public void AddToControllers(InputDevice controllerToAdd)
    {
        connectedControllers.Add(controllerToAdd);
    }
}
