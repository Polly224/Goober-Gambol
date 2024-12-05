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

    private void Update()
    {
        if (RoundManager.playersDeadThisRound == GameObject.FindGameObjectsWithTag("Player").Length - 1)
        {
            placingValue = 1;
        }
    }
    public void Died()
    {
        placingValue = GameObject.FindGameObjectsWithTag("Player").Length - RoundManager.playersDeadThisRound;
        RoundManager.playersDeadThisRound++;
    }
}
