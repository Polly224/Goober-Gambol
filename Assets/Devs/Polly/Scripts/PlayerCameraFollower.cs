using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraFollower : MonoBehaviour
{
    [SerializeField] float zoomDistance;
    [SerializeField] float zoomSpeed;
    PlayerInputManager pIM;

    private void Start()
    {
        pIM = GameObject.Find("PlayerInputManager").GetComponent<PlayerInputManager>();
    }
    // Update is called once per frame
    void Update()
    {
        if (pIM != null)
        {
            if (PlayerDataStorage.connectedPlayerObjects.Count == 1)
            {
                transform.position = PlayerDataStorage.connectedPlayerObjects[0].transform.position - transform.forward * zoomDistance;
            }
            else if(PlayerDataStorage.connectedPlayerObjects.Count > 1)
            {
                Vector3 middlePosition = new();
                for (int i = 0; i < PlayerDataStorage.connectedPlayerObjects.Count; i++)
                {
                    middlePosition += PlayerDataStorage.connectedPlayerObjects[i].transform.position;
                }
                middlePosition /= PlayerDataStorage.connectedPlayerObjects.Count;
                transform.position = middlePosition - transform.forward * zoomDistance;
            }
        }
    }
}
