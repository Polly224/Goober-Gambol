using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
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
            List<GameObject> objects = new();
            List<float> distances = new();
            // Algorithm that gets the 2 furthest objects from the camera from all player objects present.
            for(int i = 0; i < PlayerDataStorage.connectedPlayerObjects.Count; i++)
            {
                GameObject curObj = PlayerDataStorage.connectedPlayerObjects[i];
                if(distances.Count > 0)
                {
                    if (Vector3.Distance(curObj.transform.position, transform.position) > distances[0])
                    {
                        distances.Insert(0, Vector3.Distance(curObj.transform.position, transform.position));
                        objects.Insert(0, curObj);
                    } 
                    else if(distances.Count > 1)
                    {
                        if(Vector3.Distance(curObj.transform.position, transform.position) > distances[1])
                        {
                            distances.Insert(1, Vector3.Distance(curObj.transform.position, transform.position));
                            objects.Insert(1, curObj);
                        }
                    }
                    else
                    {
                        distances.Add(Vector3.Distance(curObj.transform.position, transform.position));
                        objects.Add(curObj);
                    }
                }
                else
                {
                    distances.Add(Vector3.Distance(curObj.transform.position, transform.position));
                    objects.Add(curObj);
                }
            }
            // If there's only 1 player, the camera just focuses on them.
            if (PlayerDataStorage.connectedPlayerObjects.Count == 1)
            {
                transform.position = PlayerDataStorage.connectedPlayerObjects[0].transform.position - transform.forward * zoomDistance * 10;
            }
            // If there's MORE than 1 player, however...
            // It gets the total distance between the 2 furthest player objects, then moves the camera back based on that distance and the difference between those
            // 2 objects' z coordinates. I love coding. Coding is awesome.
            else if(PlayerDataStorage.connectedPlayerObjects.Count > 1)
            {
                Vector3 middlePosition = new();
                for (int i = 0; i < PlayerDataStorage.connectedPlayerObjects.Count; i++)
                {
                    middlePosition += PlayerDataStorage.connectedPlayerObjects[i].transform.position;
                }
                middlePosition /= PlayerDataStorage.connectedPlayerObjects.Count;
                // This line of code took a solid 10 years off my life span.
                transform.position = middlePosition - transform.forward * 2 - transform.forward * (zoomDistance * Vector3.Distance(objects[0].transform.position, objects[1].transform.position)) - transform.forward * (objects[0].transform.position.z - objects[1].transform.position.z) / 3.5f;
            }
        }
    }
}
