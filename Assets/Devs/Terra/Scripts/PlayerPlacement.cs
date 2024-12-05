using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlacement : MonoBehaviour
{
    public int placingValue;
    private RoundManager RoundManager;

    private void Awake()
    {
        RoundManager = FindObjectOfType<RoundManager>();
    }

    private void FixedUpdate()
    {
        if (RoundManager.playersDeadThisRound == PlayerDataStorage.connectedPlayerObjects.Count - 1 && placingValue != 1)
        {
            placingValue = 1;
            RoundManager.instance.EndRound();
        }
    }
    public void Died()
    {
        placingValue = GameObject.FindGameObjectsWithTag("Player").Length - RoundManager.playersDeadThisRound;
        RoundManager.playersDeadThisRound++;
    }
}
