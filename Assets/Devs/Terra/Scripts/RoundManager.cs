using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoundManager : MonoBehaviour
{
    public GameObject winningPlayer;
    private PlayerDataStorage Players;
    private CanvasFinder Canvas;

    private void Awake()
    {
        Canvas = FindObjectOfType<CanvasFinder>();
        Players = FindObjectOfType<PlayerDataStorage>();
    }
    private void Update()
    {
        foreach (GameObject g in PlayerDataStorage.connectedPlayerObjects)
        {
          //   if (g.GetComponent<RoundManager>().placingValue == 1) winningPlayer = g;
        }
    }
}
