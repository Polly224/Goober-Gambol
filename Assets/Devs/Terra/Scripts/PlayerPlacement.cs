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
        if (RoundManager.playersDeadThisRound == 3)
        {
            placingValue = 1;
        }
    }
    public void Died()
    {
        RoundManager.playersDeadThisRound++;

        switch (RoundManager.playersDeadThisRound)
        {
            case 1:
                placingValue = 4;
                break;

            case 2:
                placingValue = 3;
                break;

            case 3:
                placingValue = 2;
                break;
        }
    }
}
