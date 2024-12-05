using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

public class PlayerSelectArrow : MonoBehaviour
{
    PlayerInput pI;
    public int currentSelectionIndex = 0;
    Vector3 intendedPos;
    bool picked;
    public enum PickedCharacter
    {
        Cat,
        Dog,
        Robot,  
        Bunny,
        Unselected
    }
    public PickedCharacter pickedCharacter;

    private void Start()
    {
        // Defines the controller in the datastorage, setting it to unselected.
        pI = GetComponent<PlayerInput>();
        pickedCharacter = PickedCharacter.Unselected;
        PlayerDataStorage.playerCharacters[pI.playerIndex] = pickedCharacter;
        transform.localScale = new Vector3(0.5f + pI.playerIndex * 0.05f, 0.5f + pI.playerIndex * 0.05f, 0.5f + pI.playerIndex * 0.05f);
    }

    public void SwitchPosition(CallbackContext context)
    {
        // When the dpad is clicked left or right, the cursor moves left or right.
        if (context.performed && !picked)
        {
            if (context.ReadValue<float>() == 1)
                if (currentSelectionIndex < 3)
                    currentSelectionIndex++;
                else currentSelectionIndex = 0;
            else if (context.ReadValue<float>() == -1)
                if (currentSelectionIndex > 0)
                    currentSelectionIndex--;
                else currentSelectionIndex = 3;
        }
    }
    
    public void PickCharacter(CallbackContext context)
    {
        // Picks the character the selectarrow is currently hovering on, saving said character to a personal variable and playerdatastorage.
        if (context.performed && !picked)
        {
            picked = true;
            pickedCharacter = currentSelectionIndex switch
            {
                0 => PickedCharacter.Cat,
                1 => PickedCharacter.Dog,
                2 => PickedCharacter.Robot,
                3 => PickedCharacter.Bunny,
                _ => PickedCharacter.Unselected
            };
            PlayerDataStorage.playerCharacters[pI.playerIndex] = pickedCharacter;
        }
    }

    public void CancelPick(CallbackContext context)
    {
        // Undoes a picked character, going back to unselected.
        if(context.performed && picked)
        {
            picked = false;
            pickedCharacter = PickedCharacter.Unselected;
        }
        PlayerDataStorage.playerCharacters[pI.playerIndex] = pickedCharacter;
    }

    public void ContinueToMatch(CallbackContext context)
    {
        // If at least 2 players have picked a character, when start is pressed the game continues to the fighting scene.
        if (context.performed)
        {
            int count = 0;
            foreach(PickedCharacter character in PlayerDataStorage.playerCharacters)
            {
                if (character != PickedCharacter.Unselected) count++;
            }
            if(count >= 2)
            {
                SceneManager.LoadScene("StageSelect");
            }
        }
    }

    private void Update()
    {
        // Controls the position of the cursor.
        intendedPos = new Vector3(-4.5f + currentSelectionIndex * 3f, 1.7f, 0);
        if(intendedPos != null)
        {
            transform.position = Vector3.Slerp(transform.position, intendedPos, 0.2f);
        }
        // If the player's picked a character, the cursor's yellow. Otherwise, it's their player color.
        if (picked) GetComponent<SpriteRenderer>().color = Color.yellow;
        else
        {
            GetComponent<SpriteRenderer>().color = pI.playerIndex switch
            {
                0 => Color.red,
                1 => Color.blue,
                2 => Color.green,
                3 => Color.magenta,
                _ => Color.white
            };
        }
    }
}
