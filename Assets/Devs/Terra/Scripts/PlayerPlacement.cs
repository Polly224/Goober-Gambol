using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlacement : MonoBehaviour
{
    public int placingValue;
    public int totalPlacingValue = 0;
    private RoundManager RoundManager;
    int deadPlayerAmount = 0;

    private void Awake()
    {
        RoundManager = FindObjectOfType<RoundManager>();
    }

    private void FixedUpdate()
    {
        deadPlayerAmount = 0;
        foreach(GameObject g in PlayerDataStorage.connectedPlayerObjects)
        {
            if (!g.activeSelf) deadPlayerAmount++;
        }
        if (deadPlayerAmount >= PlayerDataStorage.connectedPlayerObjects.Count - 1 && placingValue != 1)
        {
            placingValue = 1;
            RoundManager.instance.EndRound();
        }
    }
    public void Died()
    {
        placingValue = PlayerDataStorage.connectedPlayerObjects.Count - deadPlayerAmount;
        RoundManager.playersDeadThisRound++;
    }
}
