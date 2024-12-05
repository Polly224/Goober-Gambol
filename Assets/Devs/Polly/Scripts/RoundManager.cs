using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoundManager : MonoBehaviour
{
    public GameObject winningPlayer;
    public int playersDeadThisRound;
    public int currentRound = 0;
    private List<GameObject> playerSpawnpoints;

    private int MaxRounds = 3;
    public static RoundManager instance;

    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(this);
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

    private void StartRound()
    {
        playersDeadThisRound = 0;
        currentRound++;
        PickupSpawningSystem.instance.StartCoroutine(PickupSpawningSystem.instance.StartSpawnRoutine());
        for(int i = 0; i < PlayerDataStorage.connectedPlayerObjects.Count; i++)
        {
            PlayerDataStorage.connectedPlayerObjects[i].transform.position = playerSpawnpoints[i].transform.position;
            PlayerDataStorage.connectedPlayerObjects[i].SetActive(true);
            PlayerDataStorage.connectedPlayerObjects[i].GetComponent<PlayerPlacement>().placingValue = 0;
        }
        
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

    public void EndRound()
    {
        PickupSpawningSystem.instance.StopAllCoroutines();
        StartCoroutine(ShowRoundResults());
    }
}
