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
    public int currentRound = 0;
    private List<GameObject> playerSpawnpoints;

    private int MaxRounds = 3;
    public static RoundManager instance;

    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(this); 
        Canvas = FindObjectOfType<CanvasFinder>();
        Players = FindObjectOfType<PlayerDataStorage>();
        PlayerPlacement = FindObjectOfType<PlayerPlacement>();
        MainGameUI = FindObjectOfType<MainGameUI>();
    }

    private void Start()
    {
        playerSpawnpoints = new();
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("PlayerSpawnpoint")) playerSpawnpoints.Add(g);   
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Player").Length; i++)
        {
            GameObject.FindGameObjectsWithTag("Player")[i].transform.position = playerSpawnpoints[i].transform.position;
            GameObject.FindGameObjectsWithTag("Player")[i].SetActive(true);
        }
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
        currentRound++;
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
        if (currentRound == MaxRounds)
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
