using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.ComponentModel;
using UnityEngine.InputSystem;

public class MainGameUI : MonoBehaviour
{
    [SerializeField] GameObject CharacterHealthText;
    [SerializeField] GameObject Charactercross;

    [SerializeField] List<GameObject> InventorySlots;
    [SerializeField] List<Sprite> PlayerSprites;
    [SerializeField] List<Sprite> WeaponSprites;
    List<int> objectToIndex = new();

    private void Start()
    {
        for (int i = 0; i < PlayerDataStorage.connectedPlayerObjects.Count; i++) 
        {
            objectToIndex.Add(PlayerDataStorage.connectedPlayerObjects[i].GetComponent<PlayerInput>().playerIndex);
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < objectToIndex.Count; i++)
        {
            transform.GetChild(objectToIndex[i]).GetChild(4).gameObject.GetComponent<UnityEngine.UI.Image>().sprite = PlayerDataStorage.playerCharacters[objectToIndex[i]] switch
            {
                PlayerSelectArrow.PickedCharacter.Cat => PlayerSprites[0],
                PlayerSelectArrow.PickedCharacter.Dog => PlayerSprites[1],
                PlayerSelectArrow.PickedCharacter.Robot => PlayerSprites[2],
                PlayerSelectArrow.PickedCharacter.Bunny => PlayerSprites[3],
                _ => PlayerSprites[0]
            };
            transform.GetChild(objectToIndex[i]).gameObject.SetActive(true);
            transform.GetChild(objectToIndex[i]).GetChild(3).gameObject.GetComponent<TMP_Text>().text = Mathf.Round(PlayerDataStorage.connectedPlayerObjects[i].GetComponent<DamageSystem>().damageTaken).ToString() + "%";
            if(PlayerDataStorage.connectedPlayerObjects[i].GetComponent<DamageSystem>().damageTaken > 0)
            {
                transform.GetChild(objectToIndex[i]).GetChild(3).gameObject.GetComponent<TMP_Text>().color = Color.HSVToRGB(0, 0 + Mathf.Clamp(PlayerDataStorage.connectedPlayerObjects[i].GetComponent<DamageSystem>().damageTaken, 0, 30) / 30, 1);
            }
            else
            {
                transform.GetChild(objectToIndex[i]).GetChild(3).gameObject.GetComponent<TMP_Text>().color = Color.white;
            }
        }
        for(int i = 0; i < objectToIndex.Count; i++)
        {
            for(int j = 0; j < PlayerDataStorage.connectedPlayerObjects[i].GetComponent<PlayerInventory>().weaponInventory.Count; j++)
            {
                transform.GetChild(objectToIndex[i]).GetChild(j).gameObject.SetActive(true);
                transform.GetChild(objectToIndex[i]).GetChild(j).GetComponent<UnityEngine.UI.Image>().sprite = PlayerDataStorage.connectedPlayerObjects[i].GetComponent<PlayerInventory>().weaponInventory[j].name switch
                {
                    "fists" => WeaponSprites[0],
                    "baseballbat" => WeaponSprites[0],
                    "brick" => WeaponSprites[1],
                    "cleaver" => WeaponSprites[2],
                    "greataxe" => WeaponSprites[3],
                    "metalpipe" => WeaponSprites[4],
                    "ninjastar" => WeaponSprites[5],
                    "pike" => WeaponSprites[6],
                    "plasticbottle" => WeaponSprites[7],
                    _ => WeaponSprites[i]
                };
            }
            if (PlayerDataStorage.connectedPlayerObjects[i].GetComponent<PlayerInventory>().weaponInventory.Count == 1)
            {
                transform.GetChild(objectToIndex[i]).GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                if (PlayerDataStorage.connectedPlayerObjects[i].GetComponent<PlayerInventory>().weaponInventory.Count == 0)
                {
                    transform.GetChild(objectToIndex[i]).GetChild(0).gameObject.SetActive(false);
                    transform.GetChild(objectToIndex[i]).GetChild(1).gameObject.SetActive(false);
                }
            }
        }
    }
}
