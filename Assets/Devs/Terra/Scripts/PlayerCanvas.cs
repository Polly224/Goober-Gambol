using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanvas : MonoBehaviour
{
    private GameObject originPlayer;
    [SerializeField] GameObject[] playerCanvases;   
    // Start is called before the first frame update
    void Start()
    {
        playerCanvases = FindObjectsOfType(typeof(MainGameUI)) as GameObject[];
        switch (PlayerDataStorage.connectedPlayerObjects.Count)
        {
            case 1:
                playerCanvases[0].GetComponent<Canvas>().enabled = true;
                break;
            case 2:
                playerCanvases[1].GetComponent<Canvas>().enabled = true;
                break;
            case 3:
                playerCanvases[2].GetComponent<Canvas>().enabled = true;
                break;
            case 4:
                playerCanvases[3].GetComponent<Canvas>().enabled = true;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
