using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanvas : MonoBehaviour
{
    private GameObject originPlayer;
    [SerializeField] GameObject playerCanvas;   
    // Start is called before the first frame update
    void Start()
    {
        playerCanvas = FindObjectOfType(typeof(MainGameUI)) as GameObject;
        /*switch (PlayerDataStorage.connectedPlayerObjects.Count)
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
        }*/
        for(int i = 0; i < PlayerDataStorage.connectedPlayerObjects.Count; i++)
        {
            playerCanvas.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
