using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    public GameObject winningPlayer;
    public int playersDeadThisRound;
    public int currentRound = 0;
    private List<GameObject> playerSpawnpoints;
    [SerializeField] private ParticleSystem confetti;

    private int maxRounds = 3;
    public static RoundManager instance;
    public bool canEndRound = true;

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
        GameObject.Find("Camera").transform.GetChild(1).gameObject.SetActive(true);
        GameObject.Find("Camera").transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = "Round " + (currentRound + 1).ToString();
        StartCoroutine(RemoveRoundText(2));
        PickupSpawningSystem.instance.StartCoroutine(PickupSpawningSystem.instance.StartSpawnRoutine());
    }

    private IEnumerator StartRound()
    {
        playersDeadThisRound = 0;
        currentRound++;
        PickupSpawningSystem.instance.StartCoroutine(PickupSpawningSystem.instance.StartSpawnRoutine());
        GameObject.Find("Camera").transform.GetChild(1).gameObject.SetActive(true);
        GameObject.Find("Camera").transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = "Round " + (currentRound + 1).ToString();
        StartCoroutine(RemoveRoundText(2));
        yield return null;
        for (int i = 0; i < PlayerDataStorage.connectedPlayerObjects.Count; i++)
        {
            PlayerDataStorage.connectedPlayerObjects[i].transform.position = playerSpawnpoints[i].transform.position;
            PlayerDataStorage.connectedPlayerObjects[i].transform.rotation = Quaternion.Euler(0, 0, 0);
            PlayerDataStorage.connectedPlayerObjects[i].SetActive(true);
            yield return null;
            PlayerDataStorage.connectedPlayerObjects[i].GetComponent<InputHandler>().isRagdolling = false;
            PlayerDataStorage.connectedPlayerObjects[i].GetComponent<InputHandler>().StopRagdolling(true);
            PlayerDataStorage.connectedPlayerObjects[i].transform.GetChild(0).gameObject.SetActive(true);
            yield return null;
            PlayerDataStorage.connectedPlayerObjects[i].GetComponent<PlayerPlacement>().placingValue = 0;
            PlayerDataStorage.connectedPlayerObjects[i].GetComponent<PlayerInventory>().RemoveAllWeapons();
            PlayerDataStorage.connectedPlayerObjects[i].GetComponent<PlayerInput>().SwitchCurrentControlScheme(PlayerDataStorage.connectedControllers[i]);
            PlayerDataStorage.connectedPlayerObjects[i].SetActive(true);
            yield return null;
        }
        canEndRound = true;
        yield return null;
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Pickup")) Destroy(g);
        yield return null;
        yield break;
    }

    IEnumerator RemoveRoundText(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject.Find("Camera").transform.GetChild(1).gameObject.SetActive(false);
        yield return null;
        yield break;
    }
    private IEnumerator SwitchRoundsCooldown()
    {
        yield return new WaitForSecondsRealtime(2);
        StartCoroutine(StartRound());
        yield break;
    }

    private IEnumerator ShowRoundResults()
    {
        yield return new WaitForSecondsRealtime(3);
        if (currentRound == maxRounds - 1)
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
        yield return new WaitForSecondsRealtime(3);
        GameObject winningPlayer = null;
        foreach(GameObject g in PlayerDataStorage.connectedPlayerObjects)
        {
            if (winningPlayer == null) winningPlayer = g;
            else if (g.GetComponent<PlayerPlacement>().totalPlacingValue > winningPlayer.GetComponent<PlayerPlacement>().totalPlacingValue) winningPlayer = g;
        }
        GameObject spawnedWinner = Instantiate(winningPlayer.transform.GetChild(0).gameObject, GameObject.Find("Camera").transform);
        spawnedWinner.transform.localPosition = new Vector3(0, -2, 2);
        spawnedWinner.SetActive(true);
        spawnedWinner.transform.LookAt(GameObject.Find("Camera").transform.position);
        spawnedWinner.transform.localRotation = Quaternion.Euler(0, -90, 0);
        spawnedWinner.transform.localScale = Vector3.one;
        GameObject.Find("Camera").transform.GetChild(0).gameObject.SetActive(true);
        confetti.gameObject.transform.position = spawnedWinner.transform.position + transform.forward * 4 - transform.up;
        confetti.gameObject.transform.SetParent(GameObject.Find("Camera").transform, true);
        confetti.Play();
        if (winningPlayer.name == "PlayerBunny" || winningPlayer.name == "PlayerRobot")
        {
            spawnedWinner.transform.localScale = Vector3.one / 10f;
        }
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("TitleScreen");
        yield return null;
    }

    public void EndRound()
    {
        if (canEndRound)
        {
            canEndRound = false;
            PickupSpawningSystem.instance.StopAllCoroutines();
            foreach (GameObject g in PlayerDataStorage.connectedPlayerObjects) 
            {
                g.GetComponent<PlayerPlacement>().totalPlacingValue += PlayerDataStorage.connectedPlayerObjects.Count - g.GetComponent<PlayerPlacement>().placingValue;
            }
            StartCoroutine(ShowRoundResults());
        }
    }
}
