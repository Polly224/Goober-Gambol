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
        pI = GetComponent<PlayerInput>();
        pickedCharacter = PickedCharacter.Unselected;
        PlayerDataStorage.playerCharacters[pI.playerIndex] = pickedCharacter;
    }

    public void SwitchPosition(CallbackContext context)
    {
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
        if(context.performed && picked)
        {
            picked = false;
            pickedCharacter = PickedCharacter.Unselected;
        }
        PlayerDataStorage.playerCharacters[pI.playerIndex] = pickedCharacter;
    }

    public void ContinueToMatch(CallbackContext context)
    {
        if (context.performed)
        {
            int count = 0;
            foreach(PickedCharacter character in PlayerDataStorage.playerCharacters)
            {
                if (character != PickedCharacter.Unselected) count++;
            }
            if(count >= 2)
            {
                // Code for loading the actual fighting scene.
            }
        }
    }

    private void Update()
    {
        intendedPos = new Vector3(-4.5f + currentSelectionIndex * 3f, 1.7f, 0);
        if(intendedPos != null)
        {
            transform.position = Vector3.Slerp(transform.position, intendedPos, 0.2f);
        }
        if (picked) GetComponent<SpriteRenderer>().color = Color.yellow;
        else
        {
            GetComponent<SpriteRenderer>().color = pI.playerIndex switch
            {
                0 => Color.red,
                1 => Color.green,
                2 => Color.blue,
                3 => Color.magenta,
                _ => Color.white
            };
        }
    }
}
