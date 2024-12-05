using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoundManager : MonoBehaviour
{
    public GameObject winningPlayer;
    public int playersDeadThisRound;
    private PlayerPlacement PlayerPlacement;
    private PlayerDataStorage Players;
    private CanvasFinder Canvas;
    private MainGameUI MainGameUI;

    private List<GameObject> playerSpawnpoints;

    private int MaxRounds = 3;

    private void Awake()
    {
        Canvas = FindObjectOfType<CanvasFinder>();
        Players = FindObjectOfType<PlayerDataStorage>();
        PlayerPlacement = FindObjectOfType<PlayerPlacement>();
        MainGameUI = FindObjectOfType<MainGameUI>();
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("PlayerSpawnpoint")) playerSpawnpoints.Add(g);
    }
    private void Update()
    {
        foreach (GameObject g in PlayerDataStorage.connectedPlayerObjects)
        {
            if (g.GetComponent<PlayerPlacement>().placingValue == 1)
            {
                winningPlayer = g;
                StartCoroutine(ShowRoundResults());
            }
        }
    }


    private void StartRound()
    {
        MainGameUI.CurrentRound++;
        for(int i = 0; i < PlayerDataStorage.connectedPlayerObjects.Count; i++)
        {
            PlayerDataStorage.connectedPlayerObjects[i].transform.position = playerSpawnpoints[i].transform.position;
            PlayerDataStorage.connectedPlayerObjects[i].SetActive(true);
        }
        Debug.Log("StartRound Works");
    }
    private IEnumerator SwitchRoundsCooldown()
    {
        yield return new WaitForSecondsRealtime(5);
        StartRound();
        yield break;
    }

    private IEnumerator ShowRoundResults()
    {
        yield return new WaitForSecondsRealtime(5);
        if (MainGameUI.CurrentRound == MaxRounds)
        {
            StartCoroutine(ShowEndResults());
        }
        else
        {
            StartCoroutine(SwitchRoundsCooldown());
        }
        yield break;
    }

    private IEnumerator ShowEndResults()
    {
        yield return new WaitForSecondsRealtime(5);


    }
}
