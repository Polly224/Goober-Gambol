using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainGameUI : MonoBehaviour
{
    [SerializeField] int CurrentRound = 0;
    [SerializeField] int PlayerHealthPercentage = 0;
    [SerializeField] int MaxDamage = 200;
    [SerializeField] GameObject RoundText;
    [SerializeField] GameObject Character1HealthText;
    [SerializeField] GameObject Character2HealthText;
    [SerializeField] GameObject Character3HealthText;
    [SerializeField] GameObject Character4HealthText;


    private void Update()
    {
        RoundText.GetComponent<TMP_Text>().text = "Round: " + CurrentRound.ToString();
        Character1HealthText.GetComponent<TMP_Text>().text = PlayerHealthPercentage.ToString() + "%";
        Character1HealthText.GetComponent<TMP_Text>().color = Color.HSVToRGB(1, Mathf.Clamp(0, PlayerHealthPercentage, MaxDamage), 100);
        Character2HealthText.GetComponent<TMP_Text>().text = PlayerHealthPercentage.ToString() + "%";
        Character3HealthText.GetComponent<TMP_Text>().text = PlayerHealthPercentage.ToString() + "%";
        Character4HealthText.GetComponent<TMP_Text>().text = PlayerHealthPercentage.ToString() + "%";
    }
}
