using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainGameUI : MonoBehaviour
{
    [SerializeField] public int CurrentRound = 1;
    [SerializeField] GameObject RoundText;
    [SerializeField] GameObject Character1HealthText;
    [SerializeField] GameObject Character2HealthText;
    [SerializeField] GameObject Character3HealthText;
    [SerializeField] GameObject Character4HealthText;
    [SerializeField] GameObject Character1cross, Character2cross, Character3cross, Character4cross;

    //Temp stuff    
    public bool P1elim = false;
    public bool P2elim = false;
    public bool P3elim = false;
    public bool P4elim = false;
    [SerializeField] float PlayerHealthPercentage = 0;


    private void Update()
    {
        RoundText.GetComponent<TMP_Text>().text = "Round: " + CurrentRound.ToString();
        Character1HealthText.GetComponent<TMP_Text>().text = PlayerHealthPercentage.ToString() + "%";
        Character1HealthText.GetComponent<TMP_Text>().color = Color.HSVToRGB(1, Mathf.Clamp(PlayerHealthPercentage, 1, 100) /100, 1);
        Character2HealthText.GetComponent<TMP_Text>().text = PlayerHealthPercentage.ToString() + "%";
        Character3HealthText.GetComponent<TMP_Text>().text = PlayerHealthPercentage.ToString() + "%";
        Character4HealthText.GetComponent<TMP_Text>().text = PlayerHealthPercentage.ToString() + "%";

        if (PlayerHealthPercentage < 0)
        {
            PlayerHealthPercentage = 0;
        }

        if (PlayerHealthPercentage > 230)
        {
            PlayerHealthPercentage = 230;
        }

        if (P1elim)
        {
            Character1cross.SetActive(true);
        }
    }
}
