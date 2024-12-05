using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainGameUI : MonoBehaviour
{
    [SerializeField] GameObject CharacterHealthText;
    [SerializeField] GameObject Charactercross;

    private List<GameObject> InventorySlots;

    //Temp stuff    
    public bool playerElim = false;
    [SerializeField] float PlayerHealthPercentage = 0;


    private void Update()
    {
        CharacterHealthText.GetComponent<TMP_Text>().text = PlayerHealthPercentage.ToString() + "%";
        CharacterHealthText.GetComponent<TMP_Text>().color = Color.HSVToRGB(1, Mathf.Clamp(PlayerHealthPercentage, 1, 100) / 100, 1);

        if (PlayerHealthPercentage < 0)
        {
            PlayerHealthPercentage = 0;
        }

        if (PlayerHealthPercentage > 230)
        {
            PlayerHealthPercentage = 230;
        }

        if (playerElim)
        {
            Charactercross.SetActive(true);
        }
    }
}
